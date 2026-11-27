using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    public GameObject fireProblem;
    private ParticleSystem fireParticleSystem;

    private const string FoamTag = "Foam";

    private void OnEnable()
    {
        PlayerStatus._instance.fireProblem = true;        
    }

    private void Start()
    {
        fireParticleSystem = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag(FoamTag))
        {
            fireParticleSystem.Stop();
            StartCoroutine(DestroyAfterParticles());
        }
    }

    private IEnumerator DestroyAfterParticles()
    {
        while (fireParticleSystem.IsAlive())
        {
            yield return null;
        }

        if(fireProblem != null)
        {
            PlayerStatus._instance.fireProblem = false;
            fireProblem.SetActive(false); // Deactivate the fire problem to trigger cleanup
        }
    }
}
