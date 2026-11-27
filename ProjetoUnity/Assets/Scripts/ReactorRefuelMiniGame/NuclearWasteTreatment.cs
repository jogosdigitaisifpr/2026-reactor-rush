using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class NuclearWasteTreatment : MonoBehaviour
{
    public StudioEventEmitter PelletInWaterSFX, PelletOutWaterSFX;
    public ClawMovement claw;

    [SerializeField] private GameObject usedPellet;
    [SerializeField] private GameObject usedPelletSpawnPoint;

    public float treatTime = 10f;
    public int pelletsTreated = 0;
    public int gottenPellets = 0;

    private Queue<GameObject> pelletsInTheWatta = new Queue<GameObject>();
    private Queue<float> pelletTimers = new Queue<float>();
    private int availablePellets = 0;

    private void Awake()
    {
        ClearTreatmentQueues();
    }

    private void Update()
    {
        TreatPellets();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag("Claw")) return;

        if (claw.isHoldingNuclear)
        {
            PelletInWaterSFX.Play();

            GameObject pellet = Instantiate(
                usedPellet,
                usedPelletSpawnPoint.transform.position,
                usedPelletSpawnPoint.transform.rotation,
                transform); 

            pelletsInTheWatta.Enqueue(pellet);
            pelletTimers.Enqueue(treatTime);

            claw.fixedNuclearPallet.gameObject.SetActive(false);
            claw.isHoldingNuclear = false;
        }
        else
        {
            GiveTreatedPellet();
        }
    }

    private void TreatPellets()
    {
        if (pelletTimers.Count == 0) return;

        int pelletsCount = pelletTimers.Count;

        for (int i = 0; i < pelletsCount; i++)
        {
            float timer = pelletTimers.Dequeue();
            timer -= Time.unscaledDeltaTime;

            if (timer <= 0f)
            {
                availablePellets++;
                pelletsTreated++;
                Debug.Log("Pellet tratado com sucesso!");
            }
            else
            {
                pelletTimers.Enqueue(timer);
            }
        }
    }

    private void GiveTreatedPellet()
    {
        if (availablePellets <= 0) return;

        PelletOutWaterSFX.Play();
        claw.fixedNuclearPallet.gameObject.SetActive(true);

        if (pelletsInTheWatta.Count > 0)
        {
            Destroy(pelletsInTheWatta.Dequeue());
        }

        availablePellets--;
        gottenPellets++;
        claw.isHoldingNuclear = true;
        claw.holdingTreatedPellet = true;
    }

    public void PunishUntreatedPellets()
    {
        int pendingPellets = pelletTimers.Count;
        if (pendingPellets > 0)
        {
            for (int i = 0; i < pendingPellets; i++)
            {
                ReloadController.Instance.RadiationIntoxication();
            }
        }
    }

    public void ClearTreatmentQueues()
    {
        while (pelletsInTheWatta.Count > 0)
        {
            var p = pelletsInTheWatta.Dequeue();
            if (p != null) Destroy(p);
        }
        pelletTimers.Clear();
        availablePellets = 0;
        pelletsTreated = 0;
        gottenPellets = 0;
    }
}