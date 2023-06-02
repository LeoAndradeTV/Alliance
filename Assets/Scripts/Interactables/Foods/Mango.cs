using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Mango : Food
{
    private void OnEnable()
    {
        energy = 5;
    }

    public override void Use()
    {
        base.Use();
        Debug.Log($"You ate the mango and recovered {energy} energy!");
    }
}
