using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    private InputAction pauseAction;

    private bool isPaused = false;

    private void Start()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");

        pauseMenuUI?.SetActive(false);
    }

    private void Update()
    {
        ReadInput();
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenuUI?.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuUI?.SetActive(false);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void ReadInput()
    {
        if(pauseAction.triggered)
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
}
