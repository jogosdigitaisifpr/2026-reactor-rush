using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class ReloadController : MonoBehaviour
{
    public static ReloadController Instance;

    [Header("Audio")]
    [SerializeField] private StudioEventEmitter pelletDiscardedSFX;
    [SerializeField] private StudioEventEmitter pelletReloadedSFX;

    [Header("References")]
    [SerializeField] private GameObject reloadMiniGame;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject miniGameCamera;
    [SerializeField] private GameObject clawGameObject;
    [SerializeField] private GameObject nuclearTreatment;
    [SerializeField] private GameObject xReload;
    [SerializeField] private GameObject[] pellets;

    [Header("Settings")]
    [SerializeField] private int numOfSockets;

    private int totalNumOfSockets;
    public int numOfSocketsToReload;

    public bool allPelletsReloaded { get; private set; }
    public bool allPelletsDiscarded { get; private set; }
    public bool isReloading;
    public bool needReloading;

    public float radiationCounter = 0f;
    private const float RadiationDamage = 0.5f;

    private NuclearWasteTreatment wasteTreatment;
    private InputActionMap playerActionMap;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        totalNumOfSockets = numOfSockets;
        
        radiationCounter = 0f; 
        numOfSocketsToReload = totalNumOfSockets;

        if (nuclearTreatment != null)
        {
            wasteTreatment = nuclearTreatment.GetComponent<NuclearWasteTreatment>();
        }
        playerActionMap = InputSystem.actions.FindActionMap("Player");
    }

    public void OpenMiniGame()
    {
        isReloading = true;
        reloadMiniGame.SetActive(true);
        miniGameCamera?.SetActive(true);
        mainCamera?.SetActive(false);
        clawGameObject.SetActive(true);
        xReload.SetActive(true);

        if (!needReloading)
        {
            RestartPellets();
            wasteTreatment?.ClearTreatmentQueues();
        }
        else
        {
            ForceFullResetPellets();
            RestartPellets();
        }

        if (clawGameObject.TryGetComponent<ClawMovement>(out ClawMovement claw))
        {
            claw.ResetClawState();
        }
    }

    public void DiscardPellet()
    {
        if (numOfSockets <= 0) return;

        numOfSockets--;
        Debug.Log($"DISCARDED A PELLET ({numOfSockets} remaining)");
        pelletDiscardedSFX?.Play();

        if (numOfSockets == 0)
        {
            allPelletsDiscarded = true;
            Debug.Log("ALL PELLETS DISCARDED");
        }
    }

    public void ReloadPellet()
    {
        if (numOfSocketsToReload <= 0) return;

        numOfSocketsToReload--;
        Debug.Log($"RELOADED A PELLET ({numOfSocketsToReload} remaining)");
        pelletReloadedSFX?.Play();

        if (numOfSocketsToReload == 0)
        {
            allPelletsReloaded = true;
            Debug.Log("END OF MINIGAME");
            CloseMiniGame(true);
        }
    }

    public void CloseMiniGame(bool reloadCompleted)
    {
        PlayerEnable();
        isReloading = false;
        needReloading = !reloadCompleted;

        if (reloadCompleted)
        {
            numOfSockets = totalNumOfSockets;
            numOfSocketsToReload = totalNumOfSockets;
            allPelletsDiscarded = false;
            allPelletsReloaded = false;

            if(Reaction._instance != null)
                Reaction._instance.IsReloaded = true;

            ForceFullResetPellets();
            RestartPellets();
            
            wasteTreatment?.ClearTreatmentQueues();
        }
        else
        {
            wasteTreatment?.PunishUntreatedPellets();
        }

        if (clawGameObject.TryGetComponent<ClawMovement>(out ClawMovement claw))
        {
            claw.ResetClawState();
        }

        miniGameCamera?.SetActive(false);
        mainCamera?.SetActive(true);
        //reloadMiniGame.SetActive(false);
        clawGameObject.SetActive(false);
        xReload.SetActive(false);
    }

    private void RestartPellets()
    {
        foreach (GameObject pellet in pellets)
        {
            if (pellet == null) continue;
            if (pellet.TryGetComponent<Nuclearsocket>(out Nuclearsocket socket))
            {
                if (pellet.transform.childCount > 0)
                {
                    Transform pelletPlace = pellet.transform.GetChild(0);
                    if (pelletPlace.childCount > 0) pelletPlace.GetChild(0).gameObject.SetActive(true);
                    if (pelletPlace.childCount > 1) pelletPlace.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }

    private void ForceFullResetPellets()
    {
        foreach (GameObject pellet in pellets)
        {
            if (pellet.TryGetComponent<Nuclearsocket>(out Nuclearsocket socket))
            {
                socket.ResetSocket();
            }
        }
    }

    public void RadiationIntoxication()
    {
        radiationCounter += RadiationDamage;
        Debug.Log($"Radiação total: {radiationCounter}Sv");

        bool shouldDie = radiationCounter >= 8f || (radiationCounter >= 5f && PlayerStatus._instance != null && PlayerStatus._instance.rippedClothes);
        if (shouldDie)
        {
            Debug.Log("PLAYER DIED");
        }
    }

    #region Player Input

    public void PlayerDisable()
    {
        playerActionMap?.Disable();
        Debug.Log("Inputs Disabled");
    }

    public void PlayerEnable()
    {
        playerActionMap?.Enable();
        Debug.Log("Inputs Enabled");
    }

    #endregion
}