using UnityEngine;
using FMODUnity;

public class Tape : MonoBehaviour
{
    private Item item;
    private StudioEventEmitter tapeSound;

    private const string LeakTag = "WaterLeak";
    private const string severeLeakTag = "SevereWaterLeak";

    private void OnEnable()
    {
        item = GetComponent<Item>();
        if (item != null)
        {
            item.OnUsed += TapeUse;
        }
    }

    private void OnDisable()
    {
        if (item != null)
        {
            item.OnUsed -= TapeUse;
        }
    }

    private void Start()
    {
        tapeSound = GetComponent<StudioEventEmitter>();
    }

    private void TapeUse()
    {
        tapeSound.Play();

        if(PlayerStatus._instance.waterLeak != null && PlayerStatus._instance.waterLeak.isInteractable)
        {
            PlayerStatus._instance.waterLeak.FixLeak();
        }
        
        if(PlayerStatus._instance.severeWaterLeak != null && PlayerStatus._instance.severeWaterLeak.isInteractable)
        {
            PlayerStatus._instance.severeWaterLeak.FixLeak();
        }
    }
}
