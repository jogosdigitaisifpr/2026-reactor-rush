using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System.Collections;
using FMOD;

public class SceneController : MonoBehaviour
{
    public static SceneController _instance;

    [SerializeField] private GameObject loadingScreen = null;
    private const int level1SceneIndex = 1;
    private const int numberOfLevels = 3;
    private const int scoreSceneIndex = 4;

    private FMOD.Studio.Bus MasterBus;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        if(loadingScreen != null)
        {
            DontDestroyOnLoad(loadingScreen.transform.root.gameObject);
        }
    }

    private void Start()
    {
        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
    }

    public void ChangeScene(int sceneIndex)
    {
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // Stop all currently playing FMOD events with fade out when changing scenes
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        if (sceneIndex >= level1SceneIndex && sceneIndex < level1SceneIndex + numberOfLevels)
        {
            GameStatus._instance.currentLevel = sceneIndex;
        }

        loadingScreen?.SetActive(true);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        loadingScreen?.SetActive(false);
    }

    public void ScoreScene()
    {
        StartCoroutine(CameraFadeOut());
        SceneManager.LoadSceneAsync(scoreSceneIndex);
    }

    private IEnumerator CameraFadeOut()
    {
        if(Camera.main != null)
        {
            Volume volume = Camera.main.GetComponent<Volume>();
            if(volume != null)
            {
                float fadeDuration = 1f; // Duration of the fade effect
                float elapsedTime = 0f;

                while (elapsedTime < fadeDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
                    volume.weight = alpha; // Adjust the weight of the volume for fading
                    yield return null;
                }

                volume.weight = 1f; // Ensure full fade at the end
            }
        }
    }
}
