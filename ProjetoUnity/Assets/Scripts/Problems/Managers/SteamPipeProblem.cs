using UnityEngine;
using WrightAngle.Waypoint;
using FMODUnity;

public class SteamPipeProblem : MonoBehaviour
{
    [SerializeField] private GameObject leakPrefab;
    [SerializeField] private Transform[] leakSpawnPoints;
    [SerializeField] private ProblemWarningIcons problemWarning;
    [SerializeField] private WaypointTarget wrenchWaypoint;

    private GameObject currentLeak;
    
    private void OnEnable()
    {
        Debug.Log("Steam pipe problem activated!");
        wrenchWaypoint?.ActivateWaypoint();
        SpawnLeak();
    }

    private void SpawnLeak()
    {
        if (leakPrefab != null && leakSpawnPoints != null)
        {
            int spawnIndex = Random.Range(0, leakSpawnPoints.Length);
            currentLeak = Instantiate(leakPrefab, leakSpawnPoints[spawnIndex].position, Quaternion.identity);
            currentLeak.GetComponent<SteamLeak>().steamPipeProblem = gameObject; // Link the leak to this problem for deactivation when fixed
        }
        else
        {
            Debug.LogWarning("Leak prefab or spawn points not set for SteamPipeProblem.");
        }
        PlayerStatus._instance.steamProblem = true;
        PlayerStatus._instance.steamLeak = currentLeak.GetComponent<SteamLeak>();
    }

    private void OnDisable()
    {
        PlayerStatus._instance.steamProblem = false;
        if(currentLeak != null)
        {
            Destroy(currentLeak);
        }
        problemWarning.RemoveProblem(0);
        wrenchWaypoint?.DeactivateWaypoint();
    }
}
