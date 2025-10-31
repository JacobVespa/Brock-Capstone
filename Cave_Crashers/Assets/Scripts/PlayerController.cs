using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float gravity = 9.8f;

    [Header("Look Sensitivity")]
    [SerializeField] private float lookSensitivity = 2.0f;
    [SerializeField] private float verticalRange = 80.0f;

    [Header("Player Camera")]
    [SerializeField] private Camera mainCamera;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;

    private bool isMoving;
    public float verticalRotation;
    private Vector3 currentMovement = Vector3.zero;
    private CharacterController characterController;

    private InputAction moveAction;
    private InputAction lookAction;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        moveAction = playerControls.FindActionMap("Player").FindAction("Move");
        lookAction = playerControls.FindActionMap("Player").FindAction("Look");

        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;

        lookAction.performed += context => lookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float verticalSpeed = moveInput.y * walkSpeed;
        float horizontalSpeed = moveInput.x * walkSpeed;

        Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0, verticalSpeed);
        horizontalMovement = transform.rotation * horizontalMovement;

        currentMovement.y -= gravity * Time.deltaTime;
        currentMovement.x = horizontalMovement.x;
        currentMovement.z = horizontalMovement.z;

        characterController.Move(currentMovement * Time.deltaTime);

        isMoving = moveInput.y != 0 || moveInput.x != 0;
    }

    private void HandleRotation()
    {
        float lookXRotation = lookInput.x * lookSensitivity;
        transform.Rotate(0, lookXRotation, 0);

        verticalRotation -= lookInput.y * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRange, verticalRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

    }
}