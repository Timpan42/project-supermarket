using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHand : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction handInputPosition;
    private Vector2 handPosition;
    private Camera camera;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        handInputPosition = playerInput.PlayerMovement.HandPosition;
        handInputPosition.Enable();
    }

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        HandPositionUpdate();
    }

    private void HandPositionUpdate()
    {
        handPosition = handInputPosition.ReadValue<Vector2>();
        float xPos = handPosition.x;
        float yPos = handPosition.y;

        transform.position = camera.ScreenToWorldPoint(new Vector3(xPos, yPos, 1f));
        print(camera.ScreenToWorldPoint(new Vector2(xPos, yPos)));
    }
}
