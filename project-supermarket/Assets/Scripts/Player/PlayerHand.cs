using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHand : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction handInputPosition;
    private InputAction handInputPickUpObject;
    private Vector2 mousePosition;
    private Camera camera;

    [SerializeField] private Vector2 handDetectorSize;
    [SerializeField] private Transform handTransform;
    [SerializeField] private Vector3 handCollisionDetectorOffset;
    [SerializeField] private LayerMask layerToHit;
    private Vector2 handCollisionDetectorOrigin;
    private RaycastHit2D[] rayHitArray = new RaycastHit2D[5];
    private int rayHitAmount;

    private Transform objectHolder;
    private bool holdItem = false;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        handInputPosition = playerInput.PlayerMovement.HandPosition;
        handInputPickUpObject = playerInput.PlayerMovement.PickUpObject;
        handInputPosition.Enable();
        handInputPickUpObject.Enable();
    }

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        MousePositionUpdate();
        RayFindObject();
        if (holdItem == true)
        {
            MoveObjectInHand();
        }
        DropObjectInHand();
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

        if (rayHitAmount > 0 && holdItem == false)
        {
            if (objectHolder == null)
            {
                for (int i = 0; i < rayHitAmount; i++)
                {
                    objectHolder = rayHitArray[i].transform;
                    Debug.Log(objectHolder);
                }
            }
            else
            {
                return;
            }
        }
    }

    private void MoveObjectInHand()
    {

        objectHolder.position = handCollisionDetectorOrigin;
    }

    private void DropObjectInHand()
    {
        if (handInputPickUpObject.WasPressedThisFrame())
        {
            if (holdItem == false && objectHolder != null)
            {
                holdItem = true;
            }
            else
            {
                objectHolder = null;
                holdItem = false;
            }
        }
    }
}
