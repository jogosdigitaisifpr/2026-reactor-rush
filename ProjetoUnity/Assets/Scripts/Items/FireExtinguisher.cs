using UnityEngine;
using FMODUnity;

public class FireExtinguisher : MonoBehaviour
{
    private Item item;
    [SerializeField] private GameObject foam;
    private ParticleSystem foamParticleSystem;
    private Collider foamCollider;

    private StudioEventEmitter extinguisherSound;

    private void OnEnable()
    {
        item = GetComponent<Item>();
        if (item != null)
        {
            item.OnUsed += HandleUse;
            item.OnReleased += HandleRelease;
        }

        if (foam != null)
        {
            foamCollider = foam.GetComponent<Collider>();
            if (foamCollider != null)
            {
                foamCollider.enabled = false; // Start with the collider disabled
            }
            foamParticleSystem = foam.GetComponent<ParticleSystem>();
        }

        foamParticleSystem.Stop();
    }

    private void OnDisable()
    {
        if (item != null)
        {
            item.OnUsed -= HandleUse;
            item.OnReleased -= HandleRelease;
        }
    }

    private void Start()
    {
        extinguisherSound = GetComponent<StudioEventEmitter>();
    }

    private void HandleUse()
    {
        Debug.Log("Fire extinguisher used.");
        foamParticleSystem.Play();
        extinguisherSound.Play();
        if (foamCollider != null)
        {
            foamCollider.enabled = true;
        }
    }

    private void HandleRelease()
    {
        Debug.Log("Fire extinguisher released.");
        foamParticleSystem.Stop();
        extinguisherSound.SetParameter("hold", 1f);
        if (foamCollider != null)
        {
            foamCollider.enabled = false;
        }
    }
}
