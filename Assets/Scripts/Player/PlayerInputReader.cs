using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputReader : MonoBehaviour
{
    public event EventHandler OnPlayerJumped;

    private PlayerInputActions _inputActions;
    private float _speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _inputActions = new PlayerInputActions();

        _inputActions.Player.Enable();
        _inputActions.Player.Sprint.performed += Sprint_performed;
        _inputActions.Player.Sprint.canceled += Sprint_canceled;
        _inputActions.Player.Jump.performed += Jump_performed;

    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        OnPlayerJumped?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Sets sprint speed while sprint button is being held
    /// </summary>
    /// <param name="obj"></param>
    private void Sprint_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _speed = 5f;
    }

    /// <summary>
    /// Resets speed when sprint button is released
    /// </summary>
    /// <param name="obj"></param>
    private void Sprint_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _speed = 3f;
    }

    public Vector2 GetMovementInput()
    {
        Vector2 movement = _inputActions.Player.Movement.ReadValue<Vector2>();
        return movement;
    }

    public Vector2 GetCameraRotationFromController()
    {
        Vector2 cameraMovement = _inputActions.Player.Camera.ReadValue<Vector2>();
        return cameraMovement;
    }

    public float GetMovementSpeed()
    {
        return _speed;
    }
}
