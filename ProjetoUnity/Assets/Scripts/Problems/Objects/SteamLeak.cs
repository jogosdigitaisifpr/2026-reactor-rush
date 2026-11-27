using UnityEngine;
using System.Collections;

public class SteamLeak : MonoBehaviour
{
    public GameObject steamPipeProblem;
    private PlayerInteraction playerInteraction;
    private ParticleSystem steamParticleSystem;
    public bool isInteractable = false;

    private const string PlayerTag = "Player";
    private const string WrenchItemName = "Wrench";

    private void OnEnable()
    {
        PlayerStatus._instance.steamProblem = true;        
    }

    private void Start()
    {
        playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        steamParticleSystem = GetComponent<ParticleSystem>();
    }

    public void FixLeak()
    {
        // if(playerInteraction == null)
        // {
        //     Debug.LogWarning("PlayerInteraction not found in scene. Cannot fix steam leak.");
        //     return;
        // }

        // if(playerInteraction.heldItemObject != null && playerInteraction.heldItemObject.name == WrenchItemName)
        // {
            steamParticleSystem.Stop();
            StartCoroutine(DestroyAfterParticles());
        // }
    }

    private IEnumerator DestroyAfterParticles()
    {
        while (steamParticleSystem.IsAlive())
        {
            yield return null;
        }

        if(steamPipeProblem != null)
        {
            PlayerStatus._instance.steamProblem = false;
            steamPipeProblem.SetActive(false); // Deactivate the steam pipe problem to trigger cleanup
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
