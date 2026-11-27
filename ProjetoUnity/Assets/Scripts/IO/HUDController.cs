using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class HUDController : MonoBehaviour
{
    [SerializeField] private bool debugMode = false;

    [Header("Text UI Components")]
    [SerializeField] private TextMeshProUGUI txtReactorTemp;
    [SerializeField] private TextMeshProUGUI txtReactorPressure;
    [SerializeField] private TextMeshProUGUI txtTurbineTemp;
    [SerializeField] private TextMeshProUGUI txtTurbinePressure;
    // [SerializeField] private TextMeshProUGUI txtWaterFlow;
    // [SerializeField] private TextMeshProUGUI txtRadiation;
    [SerializeField] private TextMeshProUGUI txtEnergyProduced;
    [SerializeField] private Image[] infoImages = new Image[4]; // turbine pressure; turbine temperature; reactor pressure; reactor temperature
    [SerializeField] private Gradient hudGradient;

    // Adjust these in the inspector to get the perfect blur look
    [Range(0f, 1f)] public float targetSoftness = 0.5f;
    [Range(-1f, 1f)] public float targetDilate = 0.2f;

    [Header("Panels & GameObjects")]
    [SerializeField] private GameObject waterFlowPanel;
    [SerializeField] private GameObject rodPanel;
    [SerializeField] private GameObject realoadMiniGame;
    [SerializeField] private GameObject Xreaction;
    [SerializeField] private GameObject androidButtons;

    [Header("Tutorial System")]
    [SerializeField] private GameObject tutorialGameObject;
    [SerializeField] private RawImage presentedImageComponent;
    [SerializeField] private Texture2D[] mainTutorialImages;
    [SerializeField] private Texture2D[] reloadTutorialImages;
    [SerializeField] private Texture2D[] reloadAlertImages;
    
    [SerializeField] private GameObject leaveBotton;
    [SerializeField] private GameObject[] arrows;

    [Header("Cameras")]
    [SerializeField] private Camera normalGameCamera;
    [SerializeField] private Camera miniGameCamera;

    private Texture2D[] imagesPresented;
    private int tutorialIndex = 0;
    private int tutorialController = 0;
    private bool isPlayerInputActive = true;
    
    // Mudado para 'true' para evitar erro de sintaxe e criar a flag de controle
    private bool reloadAlert = false;

    private void Start()
    {
#if UNITY_EDITOR
        if(!debugMode) ToggleTutorial(false);
        else ToggleTutorial(true);
#else
        ToggleTutorial(true);
#endif
    }

    private void FixedUpdate()
    {
        if (!PlayerStatus._instance.infoCrashProblem)
        {
            if (Reaction._instance != null && ReloadController.Instance != null && PlayerStatus._instance != null)
            {
                UpdatePannel(
                    Reaction._instance.ReactorTemperature,
                    Reaction._instance.ReactorPressure,
                    Reaction._instance.TurbineTemperature,
                    Reaction._instance.TurbinePressure,
                    Reaction._instance.WaterFlow,
                    Reaction._instance.TotalEnergyAccumulated,
                    ReloadController.Instance.radiationCounter
                );
            }
        }
        else
        {
            CrashPannel(
                    Reaction._instance.ReactorTemperature,
                    Reaction._instance.ReactorPressure,
                    Reaction._instance.TurbineTemperature,
                    Reaction._instance.TurbinePressure,
                    Reaction._instance.WaterFlow,
                    Reaction._instance.TotalEnergyAccumulated,
                    ReloadController.Instance.radiationCounter
                );
        }
    }

    public void ToggleWaterFlowPanel(bool isOn)
    {
        if (ReloadController.Instance != null && ReloadController.Instance.isReloading) return;
        if (waterFlowPanel != null) waterFlowPanel.SetActive(isOn);
        ToggleInput(!isOn);
    }

    public void ToggleRodPanel(bool isOn)
    {
        if (ReloadController.Instance != null && ReloadController.Instance.isReloading) return;
        if (rodPanel != null) rodPanel.SetActive(isOn);
        ToggleInput(!isOn);
    }

    public void ToggleReloadMiniGame()
    {
        if (ReloadController.Instance == null) return;

        if (!ReloadController.Instance.isReloading && ReloadController.Instance.needReloading)
        {
            ToggleTutorial(true);

            ReloadController.Instance.PlayerDisable();
            
            ReloadController.Instance.isReloading = true;
            if (realoadMiniGame != null) realoadMiniGame.SetActive(true);
            if (miniGameCamera != null) miniGameCamera.gameObject.SetActive(true);
            if (Xreaction != null) Xreaction.SetActive(true);
            if (normalGameCamera != null) normalGameCamera.gameObject.SetActive(false);
            
            ToggleInput(false);
        }
    }

    public void UpdatePannel(float r_temp, float r_pressure, float t_temp, float t_pressure, float waterFLow, float energyProduced, float radiation)
    {
        if(txtTurbinePressure != null) txtTurbinePressure.text = $"{t_pressure:F0} bar";
        infoImages[0].color = hudGradient.Evaluate(t_pressure / Reaction.turbineCriticalPressure);
        if (txtTurbineTemp != null) txtTurbineTemp.text = $"{t_temp:F0} K";
        infoImages[1].color = hudGradient.Evaluate((t_temp - Reaction.ambientTemperature)/ Reaction.explosionTurbineTemperature);
        if (txtReactorPressure != null) txtReactorPressure.text = $"{r_pressure:F0} bar";
        infoImages[2].color = hudGradient.Evaluate(r_pressure / Reaction.reactorCriticalPressure);
        if (txtReactorTemp != null) txtReactorTemp.text = $"{r_temp:F0} K";
        infoImages[3].color = hudGradient.Evaluate((r_temp - Reaction.ambientTemperature) / Reaction.explosionReactorTemperature);
        if (txtEnergyProduced != null) txtEnergyProduced.text = $"{energyProduced:F0}";
    }

    public void CrashPannel(float r_temp, float r_pressure, float t_temp, float t_pressure, float waterFLow, float energyProduced, float radiation)
    {
        if (txtReactorTemp != null) txtReactorTemp.text = $"---- K";
        if (txtReactorPressure != null) txtReactorPressure.text = $"---- bar";
        if (txtTurbineTemp != null) txtTurbineTemp.text = $"---- K";
        if (txtTurbinePressure != null) txtTurbinePressure.text = $"---- bar";
        if (txtEnergyProduced != null) txtEnergyProduced.text = $"----";
        for (int i = 0; i < infoImages.Length; i++)
        {
            if (infoImages[i] != null) infoImages[i].color = Color.white;
        }
    }

    public void TooggleReloadAlert()
    {
        if (tutorialGameObject != null) tutorialGameObject.SetActive(true);
        ToggleInput(false);
        Timer.timerIsRunning = false;
        tutorialIndex = 0;
        
        reloadAlert = true; // Marca que estamos no modo de alerta
        imagesPresented = reloadAlertImages; // Define as imagens de alerta diretamente aqui
        
        UpdateStoryboardUI();
    }

    public void ToggleTutorial(bool isOn)
    {
        if (tutorialGameObject != null) tutorialGameObject.SetActive(true); // Mantém consistente
        
        ToggleInput(!isOn);
        Timer.timerIsRunning = !isOn;

        if (isOn)
        {
            tutorialIndex = 0;
            
            // Se NÃO for um alerta de reload, decide entre o tutorial principal ou o de reload normal
            if (!reloadAlert)
            {
                switch (tutorialController)
                {
                    case 0:
                        imagesPresented = mainTutorialImages;
                        break;
                    case 1:
                        if(reloadTutorialImages != null)
                            imagesPresented = reloadTutorialImages;
                        break;
                }
            }
            // Se for reloadAlert, 'imagesPresented' já foi definido corretamente em TooggleReloadAlert()

            UpdateStoryboardUI();
        }
        else
        {
            if (tutorialGameObject != null) tutorialGameObject.SetActive(false); // Fecha o objeto visualmente

            if (reloadAlert)
            {
                reloadAlert = false; // Reseta a flag de alerta ao fechar
            }
            else
            {
                // Lógica original para incrementar o controller do tutorial normal/reload
                if (tutorialController > 1)
                {
                    tutorialController = 1;
                }
                else
                {
                    tutorialController++;
                }
            }
        }
    }

    public void FowardTutorial()
    {
        if (imagesPresented == null || imagesPresented.Length == 0) return;

        if (tutorialIndex < imagesPresented.Length - 1)
        {
            tutorialIndex++;
            UpdateStoryboardUI();
        }
    }

    public void BackwardTutorial()
    {
        if (imagesPresented == null || imagesPresented.Length == 0) return;

        if (tutorialIndex > 0)
        {
            tutorialIndex--;
            UpdateStoryboardUI();
        }
    }

    private void UpdateStoryboardUI()
    {
        if (imagesPresented == null || imagesPresented.Length == 0) return;

        tutorialIndex = Mathf.Clamp(tutorialIndex, 0, imagesPresented.Length - 1);

        if (presentedImageComponent != null)
        {
            Texture2D newTexture = imagesPresented[tutorialIndex];
        
            if (presentedImageComponent.texture != newTexture)
            {
                presentedImageComponent.texture = newTexture;
                presentedImageComponent.SetAllDirty(); 
            }
        }

        if (arrows != null && arrows.Length >= 2)
        {
            if (arrows[0] != null) arrows[0].SetActive(tutorialIndex > 0);                        // Seta Voltar
            if (arrows[1] != null) arrows[1].SetActive(tutorialIndex < imagesPresented.Length - 1); // Seta Avançar
        }

        if (leaveBotton != null)
        {
            leaveBotton.SetActive(tutorialIndex == imagesPresented.Length - 1);
        }
    }

    private void ToggleInput(bool enablePlayerInput)
    {
        if (isPlayerInputActive == enablePlayerInput) return;
        isPlayerInputActive = enablePlayerInput;

        var playerMap = InputSystem.actions?.FindActionMap("Player");
        var uiMap = InputSystem.actions?.FindActionMap("UI");

        if (enablePlayerInput)
        {
            playerMap?.Enable();
            uiMap?.Disable();
            ToggleAndroidInput(true);
        }
        else
        {
            playerMap?.Disable();
            uiMap?.Enable();
            ToggleAndroidInput(false);
        }
    }

    private void ToggleAndroidInput(bool enableButtons)
    {
        if(Application.platform != RuntimePlatform.Android) return;

        androidButtons.SetActive(enableButtons);
    }
}