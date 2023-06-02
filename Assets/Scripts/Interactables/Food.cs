using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IUsable
{
    protected float energy;

    public virtual void Use(IInteractable interactable) { }

    public virtual void Interact() { }
}
