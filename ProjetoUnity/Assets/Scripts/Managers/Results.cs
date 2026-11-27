using UnityEngine;
using TMPro;

public class Results : MonoBehaviour
{
    [SerializeField] private GameObject wholePowerPlant;
    [SerializeField] private GameObject[] WinObjects;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private GameObject[] LoseObjects;
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        Invoke("RevealScore", 3f); // Wait for 3 seconds before revealing the score
    }

    private void RevealScore()
    {
        if(GameStatus._instance.died)
        {
            Lose();
        }
        else
        {
            Win();
        }

        Invoke("ShowResultsPanel", 2f); // Wait for 2 seconds before showing the results panel
    }

    private void ShowResultsPanel()
    {
        resultsPanel.SetActive(true);
    }

    private void Win()
    {
        foreach (GameObject obj in WinObjects)
        {
            obj.SetActive(true);
        }

        for(int i = 0; i < stars.Length; i++)
        {
            if(GameStatus._instance.score >= GameStatus._instance.levelScores[GameStatus._instance.currentLevel - 1][i])
            {
                stars[i].SetActive(true);
            }
        }

        scoreText.text = GameStatus._instance.score.ToString() + " KWh";

        if(GameStatus._instance.currentLevel <= GameStatus._instance.unlockedLevels && GameStatus._instance.score >= GameStatus._instance.levelScores[GameStatus._instance.currentLevel - 1][0]) // If the player has completed the level and scored at least the minimum score for that level, unlock the next level
        {
            GameStatus._instance.unlockedLevels = GameStatus._instance.currentLevel + 1;
        }
    }

    private void Lose()
    {
        wholePowerPlant.SetActive(false);
        foreach (GameObject obj in LoseObjects)
        {
            obj.SetActive(true);
        }
    }

    public void RestartGame()
    {
        GameStatus._instance.died = false;
        GameStatus._instance.score = 0;
        SceneController._instance.ChangeScene(0);
    }

}
