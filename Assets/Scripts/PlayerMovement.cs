using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private float speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Enable();
        inputActions.Player.Sprint.performed += Sprint_performed;
        inputActions.Player.Sprint.canceled += Sprint_canceled;
    }

    private void Sprint_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        speed = 3f;
    }

    private void Sprint_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 movement = inputActions.Player.Movement.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(movement.x, 0f, movement.y);

        transform.position += moveDir * speed * Time.deltaTime;
    }
}
