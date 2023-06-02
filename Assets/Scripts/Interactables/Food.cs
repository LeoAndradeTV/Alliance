using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item, IUsable
{
    protected float energy;

    public virtual void Use()
    {
        ItemManager.Instance.IsHoldingItem = false;
        Destroy(gameObject); 
    }

    public override void Hold() 
    {
        base.Hold();
        Debug.Log("You are holding food");
    }

    public override void Drop() 
    {
        base.Drop();
        Debug.Log("You dropped food");
    }
}
