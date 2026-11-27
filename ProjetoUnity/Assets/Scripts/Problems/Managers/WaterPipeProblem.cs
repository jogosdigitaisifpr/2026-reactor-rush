using UnityEngine;
using WrightAngle.Waypoint;

public class WaterPipeProblem : MonoBehaviour
{
    [SerializeField] private GameObject leakPrefab;
    [SerializeField] private Transform[] leakSpawnPoints;
    [SerializeField] private ProblemWarningIcons problemWarning;
    [SerializeField] private WaypointTarget tapeWaypoint;

    private GameObject currentLeak;

    private void OnEnable()
    {
        Debug.Log("Water pipe problem activated!");
        tapeWaypoint.ActivateWaypoint();
        SpawnLeak();
    }

    private void SpawnLeak()
    {
        if (leakPrefab != null && leakSpawnPoints != null)
        {
            int spawnIndex = Random.Range(0, leakSpawnPoints.Length);
            currentLeak = Instantiate(leakPrefab, leakSpawnPoints[spawnIndex].position, Quaternion.identity);
            currentLeak.GetComponent<WaterLeak>().waterPipeProblem = gameObject; // Link the leak to this problem for deactivation when fixed
        }
        else
        {
            Debug.LogWarning("Leak prefab or spawn points not set for WaterPipeProblem.");
        }
        PlayerStatus._instance.waterProblem = true;
        PlayerStatus._instance.waterLeak = currentLeak.GetComponent<WaterLeak>();
    }

    private void OnDisable()
    {
        PlayerStatus._instance.waterProblem = false;
        if(currentLeak != null)
        {
            Destroy(currentLeak);
        }
        problemWarning.RemoveProblem(3);
        tapeWaypoint.DeactivateWaypoint();
    }
}
