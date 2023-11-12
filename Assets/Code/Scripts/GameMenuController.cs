using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    public RectTransform inventorySelector;
    public Image[] itemIcons;

    void Awake()
    {
        Array.ForEach(itemIcons, itemIcon => itemIcon.enabled = false);
    }

    public void SetInventorySlot(Sprite itemIcon, int slot)
    {
        Array.ForEach(itemIcons, itemIcon => itemIcon.enabled = false);

        itemIcons[slot].enabled = true;
        itemIcons[slot].sprite = itemIcon;
    }

    public void SelectSlot(int slot)
    {
        inventorySelector.anchoredPosition = new Vector2(-150f + (100f * slot), inventorySelector.anchoredPosition.y);
    }
}
