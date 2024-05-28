using System;
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

    [SerializeField] private Vector2 handSize;
    [SerializeField] private Transform handTransform;
    [SerializeField] private LayerMask layerToHit;
    private RaycastHit2D[] rayHitArray = new RaycastHit2D[5];
    private int rayHitAmount;


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
        RayFindObject();
    }

    private void HandPositionUpdate()
    {
        handPosition = handInputPosition.ReadValue<Vector2>();
        float xPos = handPosition.x;
        float yPos = handPosition.y;

        transform.position = camera.ScreenToWorldPoint(new Vector3(xPos, yPos, 1f));
        print(camera.ScreenToWorldPoint(new Vector2(xPos, yPos)));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        // Then use it one a default cube which is not translated nor scaled
        Gizmos.DrawWireCube(handTransform.position, handSize);

    }

    private void RayFindObject()
    {
        rayHitAmount = Physics2D.BoxCastNonAlloc(handTransform.position, handSize, 0, Vector2.zero, rayHitArray);

        if (rayHitAmount > 0)
        {
            for (int i = 0; i < rayHitAmount; i++)
            {
                Destroy(rayHitArray[i].collider.gameObject);
            }
        }
    }
}
