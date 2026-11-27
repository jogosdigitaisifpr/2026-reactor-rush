using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class ClawMovement : MonoBehaviour
{
    public GameObject loadedNuclearPellet;
    public GameObject fixedNuclearPallet;
    
    public StudioEventEmitter MoveClawDownSFX;

    private InputAction nuclearPelletAction;

    //private Vector2 moveInput;

    [SerializeField] private float speed = 5f, verticalSpeed = 5f, pelletFallTime, timeRemaining;
    [SerializeField] private float minX = -4f, maxX = 4f, maxY = 1.8f, minY = -1.5f;

    private bool isMovingVertical = false, isMovingDownwards = true;
    public bool isHoldingNuclear = false;
    public bool holdingTreatedPellet = false; // Indica se a pastilha atual foi tratada na água
    private float currentMinX;
    private float currentMaxX;

    private void Start()
    {
        // Define limites iniciais padrões
        currentMinX = minX;
        currentMaxX = maxX;
    }

    private void OnEnable()
    {
        InputSystem.actions.FindActionMap("Claw").Enable();
    }

    private void OnDisable()
    {
        InputSystem.actions.FindActionMap("Claw").Disable();
    }

    private void Awake()
    {
        nuclearPelletAction = InputSystem.actions.FindAction("UseClaw");
    }

    private void Update()
    {
        if(!ReloadController.Instance.isReloading)return;
        if (!isMovingVertical)
        {
            MoveXClaw();

            if (!ReloadController.Instance.allPelletsDiscarded)
            {
                if (nuclearPelletAction.WasPressedThisFrame())
                {
                    isMovingVertical = true;
                    isMovingDownwards = true;
                    MoveClawDownSFX.Play();
                }
            }
            else if (!ReloadController.Instance.allPelletsReloaded)
            {
                timeRemaining = Mathf.Max(0, timeRemaining - Time.unscaledDeltaTime);
                if(nuclearPelletAction.WasPressedThisFrame() && timeRemaining == 0) ThrowPellet();
            }
            else
            {
                Debug.Log("Minigame Finished");
            }
        }
        else
        {
            if (!ReloadController.Instance.allPelletsDiscarded)
            {
                MoveYClaw();
            }
            else
            {
                ReturnToTop();
            }
        }
    }

    private void ThrowPellet()
    {
        Instantiate(loadedNuclearPellet, transform.position, loadedNuclearPellet.transform.rotation);
        timeRemaining = pelletFallTime;
    }
    public void ResetClawState()
    {
        isMovingVertical = false;
        isMovingDownwards = true;
        isHoldingNuclear = false;
        holdingTreatedPellet = false;
        if (fixedNuclearPallet != null) 
            fixedNuclearPallet.SetActive(false);


        transform.position = new Vector3((minX + maxX) / 2f, maxY, transform.position.z);
    }   

    private void MoveXClaw()
    {
                if (!ReloadController.Instance.allPelletsDiscarded)
        {
            currentMinX = minX;
            currentMaxX = maxX;
        }
        else
        {
            currentMinX = minX;
            currentMaxX = 0f; 
        }

        transform.Translate(new Vector3(1, 0, 0) * Time.unscaledDeltaTime * speed);

        float clampedX = Mathf.Clamp(transform.position.x, currentMinX, currentMaxX);

        if(clampedX >= currentMaxX || clampedX <= currentMinX) 
        {
            speed *= -1;
        }

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
    private void MoveYClaw()
    {
        float targetY = isMovingDownwards ? minY : maxY;

        float newY = Mathf.MoveTowards(transform.position.y, targetY, verticalSpeed * Time.unscaledDeltaTime);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (isMovingDownwards && Mathf.Approximately(transform.position.y, minY))
        {
            isMovingDownwards = false;
        }
        else if (!isMovingDownwards && Mathf.Approximately(transform.position.y, maxY))
        {
            isMovingVertical = false;
            speed*=-1;
        }
    }

    private void ReturnToTop()
    {
        float newY = Mathf.MoveTowards(transform.position.y, maxY, verticalSpeed * Time.unscaledDeltaTime);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (Mathf.Approximately(transform.position.y, maxY))
        {
            isMovingVertical = false;
        }
    }

    public void OnTriggerEnter(Collider container)
    {
        if (isMovingDownwards)
        {
            isMovingDownwards = false;
        }
    }
}