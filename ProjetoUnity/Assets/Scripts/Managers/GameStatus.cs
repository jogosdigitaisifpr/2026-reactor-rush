using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static GameStatus _instance;

    // general game stats
    public int unlockedLevels = 1;
    
    // last round stats
    public bool died = false;
    public int score = 0;
    public int currentLevel = 1;

    public int[][] levelScores = new int[][]
    {
        new int[] { 10000, 50000, 80000 }, // Level 1
        new int[] { 10000, 50000, 80000 }, // Level 2
        new int[] { 10000, 50000, 80000 }  // Level 3
    };

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
