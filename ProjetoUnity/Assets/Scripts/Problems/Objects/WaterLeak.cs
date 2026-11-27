using UnityEngine;
using System.Collections;

public class WaterLeak : MonoBehaviour
{
    public GameObject waterPipeProblem;
    // private PlayerInteraction playerInteraction;
    private ParticleSystem waterParticleSystem;
    public bool isInteractable = false; // Set to true if the player gets in the leak collider area

    private const string PlayerTag = "Player";
    private const string TapeItemName = "Tape";

    private void OnEnable()
    {
        PlayerStatus._instance.waterProblem = true;
    }
    
    private void Start()
    {
        // playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        waterParticleSystem = GetComponent<ParticleSystem>();
    }

    public void FixLeak()
    {
        // if(playerInteraction == null)
        // {
        //     Debug.LogWarning("PlayerInteraction not found in scene. Cannot fix water leak.");
        //     return;
        // }

        // if(playerInteraction.heldItemObject != null && playerInteraction.heldItemObject.name == TapeItemName)
        // {
            waterParticleSystem.Stop();
            StartCoroutine(DestroyAfterParticles());
        // }
    }

    private IEnumerator DestroyAfterParticles()
    {
        while (waterParticleSystem.IsAlive())
        {
            yield return null;
        }

        if(waterPipeProblem != null)
        {
            PlayerStatus._instance.waterProblem = false;
            waterPipeProblem.SetActive(false); // Deactivate the water pipe problem to trigger cleanup
        }
    }    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered water leak area.");
        if (other.CompareTag(PlayerTag))
        {
            isInteractable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            isInteractable = false;
        }
    }
}
