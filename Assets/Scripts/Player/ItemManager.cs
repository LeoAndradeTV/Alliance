using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance {  get; private set; }

    private PlayerInputReader _playerInput;

    private float maxInteractDistance = 1.5f;
    private Vector3 halfExtends = new Vector3(2, 1, 0.25f);
    private bool isHoldingItem;
    private GameObject foundObject;

    public bool IsHoldingItem
    {
        get { return isHoldingItem; }
        set { isHoldingItem = value; }
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInputReader>();
        _playerInput.OnPlayerInteracted += _playerInput_OnPlayerInteracted;
        _playerInput.OnPlayerUsed += _playerInput_OnPlayerUsed;
    }

    private void _playerInput_OnPlayerUsed(object sender, System.EventArgs e)
    {
        if (!isHoldingItem)
            return;
        if (foundObject.TryGetComponent(out IUsable usable))
        {
            usable.Use();
        }
    }

    private void _playerInput_OnPlayerInteracted(object sender, System.EventArgs e)
    {
        if (foundObject == null)
            return;
        if (foundObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHoldingItem)
        {
            foundObject = FindInteractableObject();
        }
    }

    private GameObject FindInteractableObject()
    {
        RaycastHit hit;
        Vector3 center = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        if (Physics.BoxCast(center, halfExtends, transform.forward, out hit, transform.rotation, maxInteractDistance))
        {
            return hit.transform.gameObject;
        }
        return null;
    }
}
