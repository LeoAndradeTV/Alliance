using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Handles player movement and sprint when on the ground
/// </summary>
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

    /// <summary>
    /// Sets sprint speed while sprint button is being held
    /// </summary>
    /// <param name="obj"></param>
    private void Sprint_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        speed = 5f;
    }

    /// <summary>
    /// Resets speed when sprint button is released
    /// </summary>
    /// <param name="obj"></param>
    private void Sprint_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        speed = 3f;
    }


    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// Reads the input and moves the player in the correct direction
    /// </summary>
    private void HandleMovement()
    {
        Vector2 movement = inputActions.Player.Movement.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(movement.x, 0f, movement.y);

        transform.position += moveDir * speed * Time.deltaTime;
    }
}
