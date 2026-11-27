using System.Diagnostics;
using UnityEngine;
using FMODUnity;

public class Wrench : MonoBehaviour
{
    private Item item;
    // [SerializeField] private PlayerInteraction playerScript;
    private StudioEventEmitter wrenchSound;

    private const string LeakTag = "SteamLeak";

    private void OnEnable()
    {
        item = GetComponent<Item>();
        if (item != null)
        {
            item.OnUsed += WrenchUse;
        }
    }

    private void OnDisable()
    {
        if (item != null)
        {
            item.OnUsed -= WrenchUse;
        }
    }

    private void Start()
    {
        wrenchSound = GetComponent<StudioEventEmitter>();
    }

    private void WrenchUse()
    {
        wrenchSound.Play();

        if(PlayerStatus._instance.steamLeak != null && PlayerStatus._instance.steamLeak.isInteractable)
        {
            PlayerStatus._instance.steamLeak.FixLeak();
        }
        
        // GameObject target = playerScript.LookingAt();
        // if(target != null && target.CompareTag(LeakTag))
        // {
        //     target.GetComponent<SteamLeak>().FixLeak();
        // }
    }
}
