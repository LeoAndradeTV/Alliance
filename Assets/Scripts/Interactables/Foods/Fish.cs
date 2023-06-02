using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Fish : Food
{
    private void OnEnable()
    {
        energy = 15;
    }

    public override void Use()
    {
        base.Use();
        Debug.Log($"You ate the fish and recovered {energy} energy!");
    }
}
