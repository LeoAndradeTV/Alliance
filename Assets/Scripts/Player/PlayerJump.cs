using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
/// <summary>
/// Handles all of the player jump logic
/// </summary>
public class PlayerJump : MonoBehaviour
{
    private PlayerInputReader _playerInput;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody playerRigidbody;

    // Jump variables
    private float upGravity = 30;
    private float downGravity = 40;
    private float jumpForce = 15f;

    // Max distance for box to check if player is grounded
    private float boxCastMaxDistance = 0.15f;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInputReader>();
        _playerInput.OnPlayerJumped += playerInput_OnPlayerJumped;
    }

    /// <summary>
    /// Listens to jump input
    /// </summary>
    /// <param name="obj"></param>
    private void playerInput_OnPlayerJumped(object sender, System.EventArgs e)
    {
        if (IsGrounded())
        {
            //playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpForce, playerRigidbody.velocity.z);
        }
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
            playerRigidbody.velocity -= (new Vector3(0, upGravity, 0)) * Time.deltaTime;
        } else
        {
            playerRigidbody.velocity -= (new Vector3(0, downGravity, 0)) * Time.deltaTime;
        }
    }

    /// <summary>
    /// Checks to see if player is grounded
    /// </summary>
    /// <returns>True if player is on the floor</returns>
    public bool IsGrounded()
    {
        Vector3 boxCenter = new Vector3(playerRigidbody.transform.position.x, playerRigidbody.transform.position.y + 0.3f, playerRigidbody.transform.position.z);
        Vector3 boxSize = new Vector3(playerRigidbody.transform.localScale.x, playerRigidbody.transform.localScale.y / 2, playerRigidbody.transform.localScale.z);
        return Physics.BoxCast(boxCenter, boxSize/2, Vector3.down, transform.rotation, boxCastMaxDistance, groundLayer);
    }
}
