using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using FMODUnity;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    private InputAction moveAction;
    private Vector2 moveInput;

    private Transform camTransform;
    private CamController camController;

    [SerializeField] private float walkSpeed = 5f;
    private float rotationSpeed = 10f; // radians per second (I think)
    private FMOD.Studio.EventInstance fs;
    private float timeStep = 0f;

    private void OnEnable()
    {
        InputSystem.actions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputSystem.actions.FindActionMap("Player").Disable();
    }

    private void Start()
    {
        fs = FMODUnity.RuntimeManager.CreateInstance("event:/footstep");
        moveAction = InputSystem.actions.FindAction("Move");

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        camController = Camera.main.GetComponent<CamController>();
        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        Walk();
        Rotate();
    }

    private void Walk()
    {
        Vector3 movement = CamRelativeToPlayer() * walkSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        animator.SetFloat("speed", moveInput.magnitude);
        if (timeStep < 0.25f)
        {
            timeStep += Time.deltaTime;
            return;
        }
        else if(moveInput.magnitude > 0.5f)
        {
            fs.start();
            timeStep = 0f;
        }
        else
        {
            fs.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            timeStep = 0f;
        }
    }

    private void Rotate() // While blend tree isn't implemented, shouldn't be needed after
    {
        if (moveInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(CamRelativeToPlayer());
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotationSpeed); // Slerp for smooth rotation
        }
        else
        {
            rb.angularVelocity = Vector3.zero; // Stop rotation when not moving
        }
    }

    private Vector3 CamRelativeToPlayer()
    {
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        
        camForward.Normalize();
        camRight.Normalize();

        return (camForward*moveInput.y) + (camRight*moveInput.x);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Area"))
        {
            Area area = other.GetComponent<Area>();
            if(area != null)
            {
                camController.areaCameraPosition = area.middleCameraPosition;
                camController.areaPlayerPosition = area.middlePlayerPosition;
                if(area.obstructingWall != null)
                {
                    area.obstructingWall.isOnCamera = true;
                }
                camController.CumMovement();
            }
        }
        if(other.CompareTag("Flood"))
        {
           fs.setParameterByName("flood", 0.9f, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if(other.CompareTag("Flood"))
        {
           fs.setParameterByName("flood", 0f);
        }
    }

    private void ReadInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

}
