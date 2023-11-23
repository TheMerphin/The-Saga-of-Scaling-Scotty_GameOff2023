using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using static Toolbox;

public class PlayerController : MonoBehaviour
{
    public float movementSpeedFactor = 1f;
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private Animator animator;
    private AnimatorOverrideController animatorOverrideController;
    private AnimationClipOverrides attackClipOverrides;

    private float horizontalMovement, verticalMovement;
    private bool isFacingTL, isFacingBR, isFacingBL, isFacingTR;
    private int selectedSlot;

    private GameMenuController gameMenuController;
    private AudioManager audioManager;

    private float interactCooldown;

    private PlayerScalingInfo scalingLevelInfo;
    public PlayerScalingInfo ScalingLevelInfo { get { return scalingLevelInfo; } set { scalingLevelInfo = value; } }

    public int maxHealth = 5;
    public int currentHealth;

    private float scaleCooldown;

    private bool disableInputs = false;

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
        scaleCooldown = 0f;
        scalingLevelInfo = GetScaleStructByScaleLevel(ScaleLevel.Normal);
        transform.localScale = Vector3.one;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        attackClipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(attackClipOverrides);

        gameMenuController = FindFirstObjectByType<GameMenuController>();
        audioManager = FindFirstObjectByType<AudioManager>();

        gameMenuController.SelectSlot(selectedSlot);
        gameMenuController.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
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
        if (disableInputs) return;

        var interactable = Physics2D.CircleCast(transform.position, 1.5f, Vector2.zero, 0f, LayerMask.GetMask("Interactable"));
        if (interactable.collider != null)
        {
            var item = interactable.collider.GetComponent<Item>();
            var door = interactable.collider.GetComponent<DoorController>();
            var chest = interactable.collider.GetComponent<ChestController>();
            var prompter = interactable.collider.GetComponent<ObjectPrompter>();

            if (Input.GetKeyDown(KeyCode.F) && interactCooldown <= 0f && item != null)
            {
                interactCooldown = 1f;
                PickUpItem(item);
                return;
            }

            if (chest != null)
            {
                if (chest.KeyRequired && chest.KeyReference == items[4])
                {
                    chest.UpdatePrompt(true);
                }
                else
                {
                    chest.UpdatePrompt(false);
                }

                if (Input.GetKeyDown(KeyCode.F) && chest.OpenChest(items[4]) && chest.KeyRequired)
                {
                    items[4] = null;
                }
            }

            if (door != null)
            {
                if (door.KeyReference == items[4])
                {
                    door.UpdatePrompt(true);
                }
                else
                {
                    door.UpdatePrompt(false);
                }

                if (Input.GetKeyDown(KeyCode.F) && door.OpenDoor(items[4]))
                {
                    items[4] = null;
                }
            }

            if (prompter != null)
            {
                prompter.ShowPrompt(true);
            }
        }

        var trapCast = Physics2D.CircleCastAll(transform.position, 0.01f, Vector2.zero, 0f, LayerMask.GetMask("Trap"));
        Array.ForEach(trapCast, trap =>
        {
            var trapController = trap.collider.GetComponent<Trap>();
            if (trapController != null) trapController.TriggerTrap(this);
        });
    }

    private void FixedUpdate()
    {
        if (horizontalMovement > 0.01f || horizontalMovement < -0.01f)
        {
            rb.AddForce(new Vector2(horizontalMovement * movementSpeedFactor * 2f, 0f), ForceMode2D.Impulse);
        }

        if (verticalMovement > 0.01f || verticalMovement < -0.01f)
        {
            rb.AddForce(new Vector2(0f, verticalMovement * movementSpeedFactor * 1.75f), ForceMode2D.Impulse);
        }
    }

    private void HandleInput()
    {
        if (disableInputs) return;

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
                (items[selectedSlot] as Consumable).Consume();
                items[3] = null;
                gameMenuController.DeleteFromSlot(3);



                // Delete item from array + update ui
            }
        }

        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftShift)) && scaleCooldown > 0f)
        {
            audioManager.Play("OnCooldown");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && scaleCooldown <= 0f && (int)scalingLevelInfo.ScaleLevel < 1)
        {
            ScalePlayerUp();
            scaleCooldown = 4f;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && scaleCooldown <= 0f && (int)scalingLevelInfo.ScaleLevel > -1)
        {
            ScalePlayerDown();
            scaleCooldown = 4f;
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

        if (interactCooldown > 0f) interactCooldown -= 0.01f;
        if (scaleCooldown > 0f) scaleCooldown -= 0.01f;

        horizontalMovement = verticalMovement = 0f;
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
            UpdatePlayerScaling();
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



            previousItem = items[slot];
            items[slot] = consumable;

        }
        else if (item is Key)
        {
            previousItem = items[4];
            items[4] = item;
        }

        if (previousItem != null) previousItem.Drop();

        // Update UI
        if (item is not Key) gameMenuController.SetInventorySlot(item.Icon, slot);
    }

    private void ScalePlayerUp()
    {
        ScaleLevel targetScaleLevel;
        if (scalingLevelInfo.ScaleLevel.Equals(ScaleLevel.Normal))
        {
            targetScaleLevel = ScaleLevel.Big;
        }
        else  // ScaleLevel.Small
        {
            targetScaleLevel = ScaleLevel.Normal;
        }

        StartCoroutine(Scale(targetScaleLevel));
    }

    private void ScalePlayerDown()
    {
        ScaleLevel targetScaleLevel;
        if (scalingLevelInfo.ScaleLevel.Equals(ScaleLevel.Normal))
        {
            targetScaleLevel = ScaleLevel.Small;
        }
        else // ScaleLevel.Big
        {
            targetScaleLevel = ScaleLevel.Normal;
        }

        StartCoroutine(Scale(targetScaleLevel));
    }

    public DiagonalDirection GetPlayerFacingDirection()
    {
        if (isFacingTL) return DiagonalDirection.UpLeft;
        if (isFacingTR) return DiagonalDirection.UpRight;
        if (isFacingBL) return DiagonalDirection.DownLeft;
        return DiagonalDirection.DownRight;
    }

    private IEnumerator Scale(ScaleLevel targetScaleLevel)
    {
        var targetScalingInfo = GetScaleStructByScaleLevel(targetScaleLevel);
        var currentTransformScale = scalingLevelInfo.TransformScale;
        var currentMovementSpeed = scalingLevelInfo.MovementSpeedModifier;

        var totalDuration = 1f;
        var ticks = 20f;

        if ((int)scalingLevelInfo.ScaleLevel < (int)targetScalingInfo.ScaleLevel)
        {
            audioManager.Play("Inflate");
        }
        else
        {
            audioManager.Play("Deflate");
        }

        for (int i = 0; i <= ticks; i++)
        {
            var transformLerp = Mathf.Lerp(currentTransformScale, targetScalingInfo.TransformScale, i / ticks);
            transform.localScale = new Vector2(transformLerp, transformLerp);

            var movementSpeedLerp = Mathf.Lerp(currentMovementSpeed, targetScalingInfo.MovementSpeedModifier, i / ticks);
            animator.SetFloat("movementSpeedMultiplier", movementSpeedLerp);
            movementSpeedFactor = movementSpeedLerp;

            if (i == ticks / 2f) scalingLevelInfo = targetScalingInfo;
            yield return new WaitForSeconds(totalDuration / ticks);
        }

        // Update items
        Array.ForEach(items, item => { if (item != null) item.OnPlayerScaleChange(scalingLevelInfo); });

        var weapon = items[selectedSlot] as Weapon;
        if (weapon != null) animator.SetFloat("attackSpeedMultiplier", weapon.AttackSpeedMultiplier);
    }

    private void UpdatePlayerScaling()
    {
        animator.SetFloat("movementSpeedMultiplier", scalingLevelInfo.MovementSpeedModifier);
        movementSpeedFactor = scalingLevelInfo.MovementSpeedModifier;

        var weapon = items[selectedSlot] as Weapon;
        if (weapon != null) animator.SetFloat("attackSpeedMultiplier", weapon.AttackSpeedMultiplier);
    }
    public void FallOffGround(Vector2 respawnPosition)
    {
        FallOffGround(respawnPosition, 0);
    }

    public void FallOffGround(Vector2 respawnPosition, float initialDelay)
    {
        disableInputs = true;
        StartCoroutine(FallOffGround_Coroutine(respawnPosition, initialDelay));
    }

    private IEnumerator FallOffGround_Coroutine(Vector2 respawnPosition, float initialDelay)
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(initialDelay);

        playerCollider.enabled = false;
        var startingPosY = transform.position.y;
        var originalDrag = rb.drag;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var cinemachineCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
        cinemachineCamera.Follow = null;

        rb.gravityScale = 0.7f;
        rb.drag = 0f;
        rb.velocityY = -1f;

        audioManager.Play("PlayerFalling");

        while (startingPosY - 5 < transform.position.y)
        {
            if (Mathf.Abs(transform.position.y - (int)transform.position.y) >= 0.875f && spriteRenderer.sortingLayerName.Equals("1_OnGround"))
            {
                spriteRenderer.sortingLayerName = "-1_BelowGround";
                spriteRenderer.sortingOrder = 1;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    /*
     * positive for heal 
     * negative for damage
    */
    public void updateHealth(int newHealth)
    {
        if (currentHealth + newHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("GAME OVER");
        }
        else if (currentHealth + newHealth >= maxHealth)
        {
            currentHealth = maxHealth;

        }
        else
        {
            currentHealth = currentHealth + newHealth;
        }
        gameMenuController.SetHealth(currentHealth);
    }
}
