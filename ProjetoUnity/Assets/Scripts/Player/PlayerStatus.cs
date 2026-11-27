using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus _instance;

    public bool rippedClothes = false;
    public bool fireProblem = false;
    public bool steamProblem = false;
    public SteamLeak steamLeak;
    public bool waterProblem = false;//TANAKA SEU GOSTOSO
    public WaterLeak waterLeak;
    public bool coolingProblem = false;
    public SevereWaterLeak severeWaterLeak;
    public bool infoCrashProblem = false;//USE THIS TO DEACTIVATE THE INFO;
    
    void OnEnable()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject); 
            return;
        }
        _instance = this;
    }

    public void Die()
    {
        Debug.Log("Player has died.");
        GameStatus._instance.died = true;
        SceneController._instance.ScoreScene();
    }

    public void TimeUp()
    {
        Debug.Log("Time is up.");
        GameStatus._instance.died = false;
        GameStatus._instance.score = Mathf.FloorToInt(Reaction._instance.TotalEnergyAccumulated);
        SceneController._instance.ScoreScene();
    }
}
