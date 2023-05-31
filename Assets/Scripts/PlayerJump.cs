using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles all of the player jump logic
/// </summary>
public class PlayerJump : MonoBehaviour
{
    private PlayerInputActions inputActions;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody playerRigidbody;

    // Jump variables
    private float upGravity = 25;
    private float downGravity = 35;
    private float jumpForce = 1000f;

    // Max distance for box to check if player is grounded
    private float boxCastMaxDistance = 0.15f;

    private void Start()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += Jump_performed;
    }

    private void Update()
    {
        HandleGravity();
    }

    /// <summary>
    /// Changes gravity based on what phase of the jump the player is in
    /// </summary>
    private void HandleGravity()
    {
        if (playerRigidbody.velocity.y > 0)
        {
            playerRigidbody.velocity -= (new Vector3(playerRigidbody.velocity.x, upGravity, playerRigidbody.velocity.z)) * Time.deltaTime;
        } else
        {
            playerRigidbody.velocity -= (new Vector3(playerRigidbody.velocity.x, downGravity, playerRigidbody.velocity.z)) * Time.deltaTime;
        }
    }

    /// <summary>
    /// Listens to jump input
    /// </summary>
    /// <param name="obj"></param>
    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Checks to see if player is grounded
    /// </summary>
    /// <returns>True if player is on the floor</returns>
    public bool IsGrounded()
    {
        Vector3 boxCenter = new Vector3(playerRigidbody.transform.position.x, playerRigidbody.transform.position.y - 0.65f, playerRigidbody.transform.position.z);
        Vector3 boxSize = new Vector3(playerRigidbody.transform.localScale.x, playerRigidbody.transform.localScale.y / 2, playerRigidbody.transform.localScale.z);
        return Physics.BoxCast(boxCenter, boxSize/2, Vector3.down, transform.rotation, boxCastMaxDistance, groundLayer);
    }
}