using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Toolbox;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
    /**
     * Contains four items with the following mapping:
     * 0 -> WeaponType.Melee
     * 1 -> WeaponType.Range
     * 2 -> WeaponType.Special
     * 3 -> Consumable
     * 4 -> invisible Key slot
     */
    private Item[] items;

    private int selectedSlot;

    private GameMenuController gameMenuController;

    private Animator animator;
    private AnimatorOverrideController animatorOverrideController;
    private AnimationClipOverrides attackClipOverrides;

    void Awake()
    {
        items = new Item[5];

        SetSelectedSlot(0);
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        attackClipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(attackClipOverrides);

        gameMenuController = FindFirstObjectByType<GameMenuController>();

        gameMenuController.SelectSlot(selectedSlot);
    }

    public Item GetSelectedItem()
    {
        return items[selectedSlot];
    }

    public Item GetItem(int slot)
    {
        return items[slot];
    }

    public void SetItem(Item item, int slot)
    {
        items[slot] = item;
        gameMenuController.SetInventorySlot(item.Icon, slot);
    }

    public void ClearSlot(int slot)
    {
        items[slot] = null;
        gameMenuController.SetInventorySlot(null, slot);
    }

    public bool DropSlot(int slot)
    {
        if (items[slot] == null) return false;

        items[slot].Drop();
        items[slot] = null;
        gameMenuController.SetInventorySlot(null, slot);
        return true;
    }

    public Key GetKey()
    {
        return items[4] as Key;
    }

    public bool CarriesKey()
    {
        return items[4] != null;
    }

    public void AdvanceSelectedSlot()
    {
        if (selectedSlot < 3)
        {
            selectedSlot++;
            UpdateItemAnimations();
            gameMenuController.SelectSlot(selectedSlot);
        }
    }

    public void RetreadSelectedSlot()
    {
        if (selectedSlot > 0)
        {
            selectedSlot--;
            UpdateItemAnimations();
            gameMenuController.SelectSlot(selectedSlot);
        }
    }

    public void SetSelectedSlot(int slot)
    {
        if (selectedSlot == slot) return;

        selectedSlot = slot;
        UpdateItemAnimations();
        gameMenuController.SelectSlot(selectedSlot);
    }

    public void PickUpItem(Item item)
    {
        item.PickUp(transform);

        Item previousItem = null;
        var slot = -1;
        if (item is Weapon)
        {
            switch ((item as Weapon).WeaponType)
            {
                case WeaponType.Melee:
                    slot = 0;
                    break;
                case WeaponType.Range:
                    slot = 1;
                    break;
                case WeaponType.Special:
                    slot = 2;
                    break;
            }
        }
        else if (item is Consumable)
        {
            slot = 3;
        }
        else if (item is Key)
        {
            slot = 4;
        }
        previousItem = items[slot];
        items[slot] = item;

        if (previousItem != null) previousItem.Drop();

        // Update UI
        if (item is not Key) gameMenuController.SetInventorySlot(item.Icon, slot);

        UpdateItemAnimations();
    }

    private void UpdateItemAnimations()
    {
        if (items[selectedSlot] is Weapon)
        {
            var weapon = items[selectedSlot] as Weapon;

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

    public void OnPlayerScaleChange(PlayerScalingInfo scalingLevelInfo)
    {
        Array.ForEach(items, item => { if (item != null) item.OnPlayerScaleChange(scalingLevelInfo); });

        var weapon = items[selectedSlot] as Weapon;
        if (weapon != null) animator.SetFloat("attackSpeedMultiplier", weapon.AttackSpeedMultiplier);
    }

    public void ClearInventory()
    {
        gameMenuController.SetInventorySlot(null, 0);
        gameMenuController.SetInventorySlot(null, 1);
        gameMenuController.SetInventorySlot(null, 2);
        gameMenuController.SetInventorySlot(null, 3);

        items = new Item[5];
    }
}
