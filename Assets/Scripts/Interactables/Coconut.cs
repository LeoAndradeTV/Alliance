using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : Food
{

    private void OnEnable()
    {
        energy = 5;
    }

    public override void Use(IInteractable interactable)
    {
        Debug.Log("you Ate the coconut");
    }

    public override void Interact()
    {
        Debug.Log("You picked up with Coconut");
    }
}
