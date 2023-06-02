using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputReader))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachine;
    private PlayerInputReader _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInputReader>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        Vector2 input = _playerInput.GetCameraRotationFromController();
        cinemachine.m_YAxis.m_InputAxisValue = input.y;
        cinemachine.m_XAxis.m_InputAxisValue = input.x;
    }
}
