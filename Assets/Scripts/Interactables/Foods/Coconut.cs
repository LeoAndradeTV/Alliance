using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : Food
{

    private void OnEnable()
    {
        energy = 5;
    }

    public override void Use()
    {
        base.Use();
        Debug.Log($"You ate the coconut and recovered {energy} energy!");
    }
}
