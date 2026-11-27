using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private GameObject[] levelButtons;
    [SerializeField] private GameObject[] clouds;

    void Start()
    {
        // Ensure that the GameStatus instance is initialized
        if (GameStatus._instance == null)
        {
            Debug.LogError("GameStatus instance is not initialized.");
            return;
        }

        LoadUnlockedLevels();
    }

    private void LoadUnlockedLevels()
    {
        int unlockedLevels = GameStatus._instance.unlockedLevels;

        for (int i = 1; i <= levelButtons.Length; i++)
        {
            levelButtons[i - 1].SetActive(i <= unlockedLevels);
            clouds[i - 1].SetActive(i > unlockedLevels);
        }
    }
}
