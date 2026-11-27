using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class WaterFlowController : MonoBehaviour
{
    // [SerializeField] private Slider openingSlider;
    // [SerializeField] private TextMeshProUGUI openingText; 
    [SerializeField] private RectTransform valveImage;
    private Vector3 pivotPosition;

    [SerializeField] private RectTransform waterMeterImage;

    [SerializeField] private float changeRate = 0.1f;
    [SerializeField] private StudioEventEmitter valveSound;
    
    private InputAction clickAction, pointerPositionAction;
    private bool isDragging;
    private float deltaX, deltaY, angle, angleChange;

    [Range(0f, 100f)] // Gonna alter the Change after
    private float waterValveLevel = 0f;

    private void OnEnable()
    {
        isDragging = false;
    }

    private void Start()
    {
        clickAction = InputSystem.actions.FindAction("Click");
        pointerPositionAction = InputSystem.actions.FindAction("Point");

        pivotPosition = valveImage.position;
        Debug.Log("Valve pivot position: " + pivotPosition);
    }

    private void Update() 
    {
        ReadInput();
    }

    public void ChangeValue()
    {
        if(!isDragging) return;

        // waterValveLevel = openingSlider.value * 100;
        // openingText.text = (waterValveLevel/100).ToString("P0");
        waterValveLevel = Mathf.Clamp(waterValveLevel + (-angleChange * changeRate), 0f, 100f); // Negative sign in front of angleChange so rotating clockwise opens the valve
        Reaction._instance.waterValveLevel = waterValveLevel;
        waterMeterImage.localScale = new Vector3(1f, waterValveLevel / 100f, 1f);
        Debug.Log(waterValveLevel);
    }

    private void ReadInput()
    {
        if(clickAction.triggered)
        {
            if(valveSound != null)
            {
                valveSound.Play();
            }
            isDragging = true;
            deltaX = pointerPositionAction.ReadValue<Vector2>().x - pivotPosition.x;
            deltaY = pointerPositionAction.ReadValue<Vector2>().y - pivotPosition.y;
            angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
            
        }

        if(clickAction.WasReleasedThisFrame())
        {
            if(valveSound != null)
            {
                valveSound.Stop();
            }
            isDragging = false;
        }

        if(isDragging)
        {
            float deltaX = pointerPositionAction.ReadValue<Vector2>().x - pivotPosition.x;
            float deltaY = pointerPositionAction.ReadValue<Vector2>().y - pivotPosition.y;
            float currentAngle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
            angleChange = currentAngle - angle;
            if(angleChange > 180f) angleChange -= 360f;
            if(angleChange < -180f) angleChange += 360f;

            if(valveSound != null)
            {
                valveSound.SetParameter("valve", (waterValveLevel/100f));
            }

            ChangeValue();
            if(!(waterValveLevel <= 0 && angleChange > 0) && !(waterValveLevel >= 100 && angleChange < 0)) valveImage.localRotation = Quaternion.Euler(0f, 0f, valveImage.localRotation.eulerAngles.z + angleChange);

            angle = currentAngle;
        }
        else
        {
            deltaX = 0f;
            deltaY = 0f;
            angle = 0f;
            angleChange = 0f;
        }
    }

}
