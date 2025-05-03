using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemState : MonoBehaviour
{
    public string itemInHand;
    public bool isItemInHand;
    public bool itemInUse;
    public bool holdingFood;

    public bool IsHoldingItem() => isItemInHand;

    public void ClearItem()
    {
        itemInHand = null;
        isItemInHand = false;
        itemInUse = false;
        holdingFood = false;
    }

    public void SetItem(string newItem, bool isFoodItem)
    {
        itemInHand = newItem;
        isItemInHand = true;
        itemInUse = false;
        holdingFood = isFoodItem;
    }
}
