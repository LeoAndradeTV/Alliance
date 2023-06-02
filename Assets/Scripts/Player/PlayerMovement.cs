using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerInputReader))]
/// <summary>
/// Handles player movement and sprint when on the ground
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform cam;

    private PlayerInputReader _playerInput;
    private float _rotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInputReader>();
    }


    // Update is called once per frame
    void Update()
    {
        HandleMovement(_playerInput.GetMovementInput());
    }

    /// <summary>
    /// Reads the input and moves the player in the correct direction
    /// </summary>
    private void HandleMovement(Vector2 movement)
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        Vector3 forwardRelative = movement.y * camForward;
        Vector3 rightRelative = movement.x * camRight;

        Vector3 moveDir = (forwardRelative + rightRelative).normalized;

        transform.position += moveDir * _playerInput.GetMovementSpeed() * Time.deltaTime;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * _rotationSpeed);
    }
}
