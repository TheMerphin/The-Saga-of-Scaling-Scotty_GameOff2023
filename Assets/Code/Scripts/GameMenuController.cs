using System;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    public RectTransform inventorySelector;
    public Image[] itemIcons;
    public Image[] itemPlaceholder;

    public Slider health;

    void Awake()
    {
        Array.ForEach(itemIcons, itemIcon => itemIcon.enabled = false);
        Array.ForEach(itemPlaceholder, itemPlaceholder => itemPlaceholder.enabled = true);
        health = gameObject.GetComponentInChildren<Slider>();
    }

    public void SetInventorySlot(Sprite itemIcon, int slot)
    {
        if (slot >= itemIcons.Length) return;

        itemIcons[slot].enabled = itemIcon != null;
        itemIcons[slot].sprite = itemIcon;

        itemPlaceholder[slot].enabled = itemIcon == null;
    }

    public void SelectSlot(int slot)
    {
        inventorySelector.anchoredPosition = new Vector2(-75f + (50f * slot), inventorySelector.anchoredPosition.y);
    }

    public void SetMaxHealth(int maxHealth)
    {
        health.maxValue = maxHealth;
        health.value = maxHealth;
    }
    public void SetHealth(int tHealth)
    {
        health.value = tHealth;
    }
}
