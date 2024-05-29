using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHand : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction handInputPosition;
    private Vector2 mousePosition;
    private Camera camera;

    [SerializeField] private Vector2 handDetectorSize;
    [SerializeField] private Transform handTransform;
    [SerializeField] private Vector3 handCollisionDetectorOffset;
    [SerializeField] private LayerMask layerToHit;
    private Vector2 handCollisionDetectorOrigin;
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
        MousePositionUpdate();
        RayFindObject();
    }

    private void MousePositionUpdate()
    {
        mousePosition = handInputPosition.ReadValue<Vector2>();
        float xPos = mousePosition.x;
        float yPos = mousePosition.y;

        transform.position = camera.ScreenToWorldPoint(new Vector3(xPos, yPos, 1f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(handCollisionDetectorOrigin, handDetectorSize);
    }

    private void RayFindObject()
    {
        handCollisionDetectorOrigin = handTransform.position + handCollisionDetectorOffset;
        rayHitAmount = Physics2D.BoxCastNonAlloc(handCollisionDetectorOrigin, handDetectorSize, 0, Vector2.zero, rayHitArray);

        if (rayHitAmount > 0)
        {
            for (int i = 0; i < rayHitAmount; i++)
            {
                Debug.Log(rayHitArray[i]);
                Destroy(rayHitArray[i].collider.gameObject);
            }
        }
    }
}
