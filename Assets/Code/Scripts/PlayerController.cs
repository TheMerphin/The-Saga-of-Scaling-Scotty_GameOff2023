using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Toolbox;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 1f;
    private Rigidbody2D rb;

    private Animator animator;
    private AnimatorOverrideController animatorOverrideController;
    private AnimationClipOverrides attackClipOverrides;

    private float horizontalMovement, verticalMovement;
    private bool isFacingTL, isFacingBR, isFacingBL, isFacingTR;
    private int selectedSlot;

    private GameMenuController gameMenuController;

    private float interactCooldown;

    /**
     * Contains four items with the following mapping:
     * 0 -> WeaponType.Melee
     * 1 -> WeaponType.Range
     * 2 -> WeaponType.Special
     * 3 -> Consumable
     * 4 -> invisible Key slot
     */
    private Item[] items;

    private void Awake()
    {
        items = new Item[5];
        isFacingBL = isFacingBR = isFacingTL = isFacingTR = false;
        selectedSlot = 0;
        interactCooldown = 0f;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        attackClipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(attackClipOverrides);

        gameMenuController = FindFirstObjectByType<GameMenuController>();

        gameMenuController.SelectSlot(selectedSlot);

        SetSelectedSlot(0);
    }

    // Update is called once per frame
    void Update()
    {
        ResetValuesBeforeFrame();
        HandleInput();
    }

    private void LateUpdate()
    {
        var circleCast = Physics2D.CircleCastAll(transform.position, 1.5f, Vector2.zero, 0f, LayerMask.GetMask("Interactable"));

        Array.ForEach(circleCast, interactable => {
            var item = interactable.collider.GetComponent<Item>();
            var chest = interactable.collider.GetComponent<ChestController>();
            var prompter = interactable.collider.GetComponent<ObjectPrompter>();
            if (Input.GetKeyDown(KeyCode.F) && interactCooldown <= 0f && item != null)
            {
                interactCooldown = 1f;
                PickUpItem(item);
                return;
            }

            if (Input.GetKeyDown(KeyCode.F) && chest != null)
            {
                if (chest.OpenChest(items[4]) && chest.KeyRequired)
                {
                    items[4] = null;
                    return;
                }
                return;
            }

            if (prompter != null)
            {
                // Item can not be picked up, skip
                //if (item != null && item.CanBePickedUp == 0) return;

                prompter.ShowPrompt(true);
            }
        });
    }

    private void FixedUpdate()
    {
        if (horizontalMovement > 0.01f || horizontalMovement < -0.01f)
        {
            rb.AddForce(new Vector2(horizontalMovement * movementSpeed, 0f), ForceMode2D.Impulse);
        }

        if (verticalMovement > 0.01f || verticalMovement < -0.01f)
        {
            rb.AddForce(new Vector2(0f, verticalMovement * movementSpeed), ForceMode2D.Impulse);
        }
    }

    private void HandleInput()
    {
        HandleMovement();
        HandleActions();
    }

    private void HandleMovement()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        // Evaluate player direction
        Vector2 playerPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseDirection = (mousePos - playerPos).normalized;

        switch (GetDiagonalDirection(mouseDirection))
        {
            case DiagonalDirection.UpRight:
                isFacingTR = true;
                break;
            case DiagonalDirection.UpLeft:
                isFacingTL = true;
                break;
            case DiagonalDirection.DownRight:
                isFacingBR = true;
                break;
            case DiagonalDirection.DownLeft:
                isFacingBL = true;
                break;
        }

        animator.SetBool("isFacingTR", isFacingTR);
        animator.SetBool("isFacingTL", isFacingTL);
        animator.SetBool("isFacingBR", isFacingBR);
        animator.SetBool("isFacingBL", isFacingBL);

        if (horizontalMovement != 0f | verticalMovement != 0f)
        {
            animator.SetBool("isMoving", true);
        }
    }

    private void HandleActions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (items[selectedSlot] is Weapon)
            {
                var currentClips = animator.GetCurrentAnimatorClipInfo(0);
                var firstClip = currentClips.Length > 0 ? currentClips[0].clip : null;

                if (firstClip == null || (firstClip != null && !firstClip.name.Contains("Attack")))
                {
                    animator.SetTrigger("Attack");
                    (items[selectedSlot] as Weapon).Attack();
                }
            }
            else if (items[selectedSlot] is Consumable)
            {

            }
        }

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll < 0f && selectedSlot < 3)
        {
            SetSelectedSlot(selectedSlot + 1);
        }
        else if (mouseScroll > 0f && selectedSlot > 0)
        {
            SetSelectedSlot(selectedSlot - 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && selectedSlot != 0) SetSelectedSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && selectedSlot != 1) SetSelectedSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && selectedSlot != 2) SetSelectedSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4) && selectedSlot != 3) SetSelectedSlot(3);
    }

    private void ResetValuesBeforeFrame()
    {
        isFacingBL = isFacingBR = isFacingTL = isFacingTR = false;

        animator.SetBool("isFacingTR", false);
        animator.SetBool("isFacingTL", false);
        animator.SetBool("isFacingBR", false);
        animator.SetBool("isFacingBL", false);
        animator.SetBool("isMoving", false);

        if(interactCooldown > 0f) interactCooldown -= 0.01f;
    }

    private void SetSelectedSlot(int slot)
    {
        selectedSlot = slot;
        UpdateItemAnimations();
        gameMenuController.SelectSlot(selectedSlot);
    }

    private void UpdateItemAnimations()
    {
        if (items[selectedSlot] is Weapon)
        {
            var weapon = (Weapon)items[selectedSlot];

            // Swap attack animations
            attackClipOverrides["AttackTR"] = weapon.AttackAnimations[0];
            attackClipOverrides["AttackBR"] = weapon.AttackAnimations[1];
            attackClipOverrides["AttackBL"] = weapon.AttackAnimations[2];
            attackClipOverrides["AttackTL"] = weapon.AttackAnimations[3];

            animator.Rebind();
            animatorOverrideController.ApplyOverrides(attackClipOverrides);
            animator.SetFloat("attackSpeedMultiplier", weapon.AttackSpeedMultiplier);
        }
    }

    private void PickUpItem(Item item)
    {
        item.PickUp(transform);
        UpdateInventory(item);
        UpdateItemAnimations();
    }

    private void UpdateInventory(Item item)
    {
        Item previousItem = null;
        var slot = -1;
        if (item is Weapon)
        {
            var weapon = (Weapon)item;
            switch (weapon.WeaponType)
            {
                case WeaponType.Melee:
                    slot = 0;
                    previousItem = items[slot];
                    items[slot] = item;
                    break;
                case WeaponType.Range:
                    slot = 1;
                    previousItem = items[slot];
                    items[slot] = item;
                    break;
                case WeaponType.Special:
                    slot = 2;
                    previousItem = items[slot];
                    items[slot] = item;
                    break;
            }
        }
        else if (item is Consumable)
        {
            var consumable = (Consumable)item;
            slot = 3;

            if (items[slot] != null && items[slot].GetType().Equals(consumable.GetType()))
            {
                (items[slot] as Consumable).Count++;
            }
            else
            {
                previousItem = items[slot];
                items[slot] = consumable;
            }
        }
        else if (item is Key)
        {
            previousItem = items[4];
            items[4] = item;
        }

        if (previousItem != null) previousItem.Drop();

        // Update UI
        if(item is not Key)gameMenuController.SetInventorySlot(item.Icon, slot);
    }

    public DiagonalDirection GetPlayerFacingDirection()
    {
        if (isFacingTL) return DiagonalDirection.UpLeft;
        if (isFacingTR) return DiagonalDirection.UpRight;
        if (isFacingBL) return DiagonalDirection.DownLeft;
        return DiagonalDirection.DownRight;
    }

}
