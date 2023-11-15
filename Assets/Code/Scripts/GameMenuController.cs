using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    public RectTransform inventorySelector;
    public Image[] itemIcons;
    public Image[] itemPlaceholder;

    void Awake()
    {
        Array.ForEach(itemIcons, itemIcon => itemIcon.enabled = false);
        Array.ForEach(itemPlaceholder, itemPlaceholder => itemPlaceholder.enabled = true);
    }

    public void SetInventorySlot(Sprite itemIcon, int slot)
    {
        Array.ForEach(itemIcons, itemIcon => itemIcon.enabled = false);

        itemIcons[slot].enabled = true;
        itemIcons[slot].sprite = itemIcon;

        itemPlaceholder[slot].enabled = false;
    }

    public void SelectSlot(int slot)
    {
        inventorySelector.anchoredPosition = new Vector2(-111f + (74f * slot), inventorySelector.anchoredPosition.y);
    }
}
