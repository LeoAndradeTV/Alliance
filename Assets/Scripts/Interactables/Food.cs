using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IUsable, IHoldable, IDropable
{
    protected float energy;
    private bool isHoldingItem => ItemManager.Instance.IsHoldingItem;

    public virtual void Use()
    {
        ItemManager.Instance.IsHoldingItem = false;
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

    public void Hold() 
    {
        Debug.Log("You are holding food");
        ItemManager.Instance.IsHoldingItem = true;
    }

    public void Drop() 
    {
        Debug.Log("You dropped food");
        ItemManager.Instance.IsHoldingItem = false;
    }
}
