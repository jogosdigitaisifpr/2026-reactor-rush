using UnityEngine;
using System.Collections;

public class SevereWaterLeak : MonoBehaviour
{
    public GameObject coolingPipeProblem;
    private PlayerInteraction playerInteraction;
    private ParticleSystem waterParticleSystem;
    public bool isInteractable = false;

    private const string PlayerTag = "Player";
    private const string TapeItemName = "Tape";

    private void OnEnable()
    {
        PlayerStatus._instance.coolingProblem = true;
    }

    private void Start()
    {
        playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        waterParticleSystem = GetComponent<ParticleSystem>();
    }

    public void FixLeak()
    {
        if(playerInteraction == null)
        {
            Debug.LogWarning("PlayerInteraction not found in scene. Cannot fix water leak.");
            return;
        }

        if(playerInteraction.heldItemObject != null && playerInteraction.heldItemObject.name == TapeItemName)
        {
            waterParticleSystem.Stop();
            StartCoroutine(DestroyAfterParticles());
        }
    }

    private IEnumerator DestroyAfterParticles()
    {
        while (waterParticleSystem.IsAlive())
        {
            yield return null;
        }

        if(coolingPipeProblem != null)
        {
            PlayerStatus._instance.coolingProblem = false;
            coolingPipeProblem.SetActive(false); // Deactivate the cooling pipe problem to trigger cleanup
        }
    }    
    
    private void OnTriggerEnter(Collider other)
    {
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
