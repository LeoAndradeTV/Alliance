using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IHoldable, IDropable
{
    private bool isHoldingItem => ItemManager.Instance.IsHoldingItem;

    public virtual void Drop()
    {
        ItemManager.Instance.IsHoldingItem = false;
    }

    public virtual void Hold()
    {
        ItemManager.Instance.IsHoldingItem = true;
    }

    public void Interact()
    {
        if (isHoldingItem)
        {
            Drop();
            return;
        }
        Hold();
    }
}
