using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private Transform playerHand;
    // [SerializeField] private GameObject handIcon;
    [SerializeField] private GameObject itemButtons;
    
    private Animator animator;
    
    private float interactRange = 3f;

    private Interactable currentInteractable;
    private Outline currentOutline;
    private Item heldItem;
    private Collider heldItemCollider;
    public GameObject heldItemObject { get; private set; }
    private Vector3 itemPositionOffset = new Vector3(-0.05f, 0.15f, 0.05f);
    private Quaternion itemRotationOffset = Quaternion.Euler(75f, -90f, 0f);
    private Vector3 itemReleaseOffset;
    private Quaternion itemReleaseRotationOffset;

    private InputAction interactAction, useAction, releaseAction;

    private void Awake() {
        interactAction = InputSystem.actions.FindAction("Interact");
        useAction = InputSystem.actions.FindAction("Use");
        releaseAction = InputSystem.actions.FindAction("Drop");
    }

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckInteractables();
        ReadInput();
    }

    public GameObject LookingAt()
    {
        RaycastHit hit;

        if(Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, interactRange))
        {
            // print("Looking at: " + hit.collider.gameObject);
            return hit.collider.gameObject;  
        }
        else
        {
            // print("Looking at nothing");
            return null;
        }
    }

    private void CheckInteractables()
    {
        GameObject hit = LookingAt();

        if (hit != null)
        {
            Interactable interactable = hit.GetComponentInParent<Interactable>();
            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    ClearCurrentOutline();

                    currentInteractable = interactable;
                    // handIcon.SetActive(true);

                    // Enable outline on current target (or parent)
                    currentOutline = hit.GetComponentInParent<Outline>();
                    if (currentOutline != null)
                    {
                        currentOutline.ShowOutline();
                    }
                }
                return;
            }
        }

        ClearCurrentInteractable();
    }

    private void ClearCurrentOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.HideOutline();
            currentOutline = null;
        }
    }

    private void ClearCurrentInteractable()
    {
        ClearCurrentOutline();
        currentInteractable = null;
        // handIcon.SetActive(false);
    }

    private void HoldItem()
    {
        if(heldItem != null) return;

        heldItem = currentInteractable.item;
        heldItemObject = heldItem.gameObject;
        itemReleaseOffset = heldItemObject.transform.localPosition;
        itemReleaseRotationOffset = heldItemObject.transform.localRotation;

        heldItemObject.transform.SetParent(playerHand);
        heldItemObject.transform.localPosition = itemPositionOffset;
        heldItemObject.transform.localRotation = itemRotationOffset;

        heldItemCollider = heldItemObject.GetComponent<Collider>();
        if(heldItemCollider != null)
        {
            heldItemCollider.enabled = false;
        }
        animator.SetBool("isHolding", true);
        itemButtons.SetActive(true);
    }

    private void ReleaseItem()
    {
        if(heldItemObject == null) return;
        
        heldItemObject.transform.SetParent(currentInteractable.transform);
        heldItemObject.transform.localPosition = itemReleaseOffset;
        heldItemObject.transform.localRotation = itemReleaseRotationOffset;
        if(heldItemCollider != null)
        {
            heldItemCollider.enabled = true;
        }
        heldItem = null;
        heldItemObject = null;
        
        animator.SetBool("isHolding", false);
        itemButtons.SetActive(false);
    }

    private void ReadInput()
    {
        if(currentInteractable != null)
        {
            // Read the interact input
            if(interactAction.triggered){
                currentInteractable.Interact();
                if(heldItem == null && currentInteractable.item != null)
                {
                    HoldItem();
                }
            }
        }

        if(heldItem != null){
            // Read the use input
            if(useAction.triggered)
            {
                // Debug.Log($"Using {heldItem.name}");
                heldItem.Use();
            }
            
            if(useAction.WasReleasedThisFrame())
            {
                // Debug.Log($"Stopped using {heldItem.name}");
                heldItem.StopUsing();
            }

            // Read the release input
            if(releaseAction.triggered)
            {
                if(currentInteractable != null && currentInteractable.item == heldItem)
                {
                    // Debug.Log($"Releasing {heldItem.name}");
                    ReleaseItem();
                }
                else
                {
                    // Debug.Log("Cannot release item here");
                }
            }        
        }
    }
}