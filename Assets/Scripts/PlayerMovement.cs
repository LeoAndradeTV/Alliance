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
    private PlayerInputReader _playerInput;

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
        Vector3 moveDir = new Vector3(movement.x, 0f, movement.y);

        transform.position += moveDir * _playerInput.GetMovementSpeed() * Time.deltaTime;
    }
}
