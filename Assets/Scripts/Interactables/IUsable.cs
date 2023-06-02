using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable : IInteractable
{
   void Use(IInteractable interactable);
}
