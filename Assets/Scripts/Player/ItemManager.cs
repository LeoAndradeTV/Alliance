using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private PlayerInputReader _playerInput;

    private bool isHoldingItem;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInputReader>();
        _playerInput.OnPlayerInteracted += _playerInput_OnPlayerInteracted;
        _playerInput.OnPlayerUsed += _playerInput_OnPlayerUsed;
    }

    private void _playerInput_OnPlayerUsed(object sender, System.EventArgs e)
    {
        
    }

    private void _playerInput_OnPlayerInteracted(object sender, System.EventArgs e)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IInteractable interactable = FindInteractableObject();
        Debug.Log(interactable);
    }

    private IInteractable FindInteractableObject()
    {
        RaycastHit hit;
        if(Physics.BoxCast(transform.position, Vector3.one, transform.forward, out hit))
        {
            if(hit.transform.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                return interactable;
            }
        }
        return null;
    }
}
