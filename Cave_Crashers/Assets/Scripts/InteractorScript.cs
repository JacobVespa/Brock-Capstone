using System;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void Interact();
}
public class InteractorScript : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Transform InteractorSource;
    public float InteractRange;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;

    private InputAction interactAction;

    private void Awake()
    {
        interactAction = playerControls.FindActionMap("Player").FindAction("Interact");

        interactAction.performed += OnInteract;
    }

    private void OnEnable()
    {
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        //Only trigger when button is pressed (not held or released)
        if (!context.performed) return;

        Debug.Log("Here!");

        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }
        }
    }
}
