/*TODO SEARCH THE RESPECTIVES RATES OF CHANGING AND CHANGE THE HARDCODED THINGS.
  IN A FIRST PLACE, EVERYTHING IS CHANGING BY 0-100*/
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using FMODUnity;

public class TempetureController : MonoBehaviour
{    
    // [SerializeField] private Slider rodSlider;
    // [SerializeField] private TextMeshProUGUI rodText; 
    [SerializeField] private RectTransform handleImage;

    [SerializeField] private GameObject powerMeterImage;
    private GameObject[] powerMeterSegments;

    private InputAction clickAction, dragAction;

    [SerializeField] private float handleAmplitude = 60f; // Maximum distance the handle can move from its resting position
    [SerializeField] private float changeRate = 20f; // % per second
    [SerializeField] private float resetSpeed = 10f; // Speed at which the handle returns to its resting position when not being dragged
    [SerializeField] 
    private StudioEventEmitter handleSound;
    private bool isDragging;
    private float deltaY;

    [Range(0f, 100f)] // Gonna alter the range after
    private float rodLevel = 0f; // TODO search the change rate of the tempeture in respect to the rod level;

    private void OnEnable()
    {
        isDragging = false;
    }

    private void Start()
    {
        clickAction = InputSystem.actions.FindAction("Click");
        dragAction = InputSystem.actions.FindAction("Drag");

        powerMeterSegments = new GameObject[powerMeterImage.transform.childCount];
        for (int i = 0; i < powerMeterImage.transform.childCount; i++)
        {
            powerMeterSegments[i] = powerMeterImage.transform.GetChild(i).gameObject;
        }
    }

    private void Update() 
    {
        ReadInput();
        ChangeValue();
    }

    public void ChangeValue()
    {
        if(isDragging)
        {
            rodLevel += (deltaY/handleAmplitude) * changeRate * Time.deltaTime;
            rodLevel = Mathf.Clamp(rodLevel, 0f, 100f);
        }

        Reaction._instance.rodLevel = rodLevel;
        // rodText.text = (rodLevel/100).ToString("P0");

        for (int i = 0; i < powerMeterSegments.Length-1; i++)
        {
            powerMeterSegments[i].SetActive(i < Mathf.RoundToInt(rodLevel*(powerMeterSegments.Length-1)/100f));
        
        }
        if(rodLevel > 0f)
        {
            powerMeterSegments[powerMeterSegments.Length-1].SetActive(true); // Last segment is always on if rodLevel > 0
        }
        else
        {
            powerMeterSegments[powerMeterSegments.Length-1].SetActive(false); // Last segment is only off if rodLevel == 0
        }
    }

    private void ReadInput()
    {
        if(clickAction.triggered)
        {
            isDragging = true;
        }

        if(clickAction.WasReleasedThisFrame())
        {
            if(handleSound != null)
            {
                handleSound.SetParameter("manete",(rodLevel/100f));
                handleSound.Play();
            }
            isDragging = false;
        }

        if(isDragging)
        {
            float currentDeltaY = dragAction.ReadValue<Vector2>().y; // Delta Y of this frame
            deltaY += currentDeltaY; // Delta Y from the resting position of the handle
            deltaY = Mathf.Clamp(deltaY, -handleAmplitude, handleAmplitude);
            handleImage.localPosition = new Vector3(handleImage.localPosition.x, deltaY, handleImage.localPosition.z);
        } 
        else 
        {
            deltaY = 0f;
            handleImage.localPosition = new Vector3(handleImage.localPosition.x, Mathf.Lerp(handleImage.localPosition.y, deltaY, Time.deltaTime * resetSpeed), handleImage.localPosition.z);
        }
    }
}   