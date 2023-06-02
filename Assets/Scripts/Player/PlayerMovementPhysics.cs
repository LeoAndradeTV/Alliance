using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerInputReader))]
/// <summary>
/// Handles player movement and sprint when on the ground
/// </summary>
public class PlayerMovementPhysics : MonoBehaviour
{
    [SerializeField] private Transform cam;

    private PlayerInputReader _playerInput;
    private Rigidbody _playerRb;
    private float _rotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInputReader>();
        _playerRb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void FixedUpdate()
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

        Vector3 moveDir = (forwardRelative + rightRelative).normalized * _playerInput.GetMovementSpeed();

        _playerRb.velocity = new Vector3(moveDir.x, _playerRb.velocity.y, moveDir.z);

        Vector3 finalRotation = new Vector3(_playerRb.velocity.x, 0, _playerRb.velocity.z);

        transform.forward = Vector3.Slerp(transform.forward, finalRotation, Time.deltaTime * _rotationSpeed);
    }
}
