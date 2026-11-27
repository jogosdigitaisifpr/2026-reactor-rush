using UnityEngine;
using FMODUnity;
using System.Net.NetworkInformation;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject levelSelectUI;
    [SerializeField] private GameObject levelSelectCamera; 
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject devsScreen;
    [SerializeField] private GameObject sourcesScreen;
    
    private StudioEventEmitter transitionSFX;

    private void Start()
    {
        transitionSFX = GetComponent<StudioEventEmitter>();
        menuUI.SetActive(true);
        levelSelectUI.SetActive(false);
        levelSelectCamera.SetActive(false);
    }

    public void ToggleLevelSelect(bool isActive)
    {
        transitionSFX.Play();
        menuUI.SetActive(!isActive);
        levelSelectUI.SetActive(isActive);
        levelSelectCamera.SetActive(isActive);
    }

    public void TogglePanel(int panel) // 0 - none; 1 - options; 2 - credits
    {
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        switch(panel)
        {
        case 1:
            optionsPanel.SetActive(true);
            break;
        case 2:
            creditsPanel.SetActive(true);
            break;
        default:
            break;
        }
    }

    public void ToggleCredits(bool devs)
    {
        if(devs)
        {
            devsScreen.SetActive(true);
            sourcesScreen.SetActive(false);
        }
        else
        {
            devsScreen.SetActive(false);
            sourcesScreen.SetActive(true);
        }
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneController._instance.ChangeScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
