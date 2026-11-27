using UnityEngine;
using WrightAngle.Waypoint;

public class CoolingPipeProblem : MonoBehaviour
{
    [SerializeField] private GameObject leakPrefab;
    [SerializeField] private Transform[] leakSpawnPoints;
    [SerializeField] private ProblemWarningIcons problemWarning;
    [SerializeField] private WaypointTarget tapeWaypoint;

    private GameObject currentLeak;

    private void OnEnable()
    {
        Debug.Log("Cooling pipe problem activated!");
        tapeWaypoint.ActivateWaypoint();
        SpawnLeak();
    }

    private void SpawnLeak()
    {
        if (leakPrefab != null && leakSpawnPoints != null)
        {
            int spawnIndex = Random.Range(0, leakSpawnPoints.Length);
            currentLeak = Instantiate(leakPrefab, leakSpawnPoints[spawnIndex].position, Quaternion.identity);
            currentLeak.GetComponent<SevereWaterLeak>().coolingPipeProblem = gameObject; // Link the leak to this problem for deactivation when fixed
        }
        else
        {
            Debug.LogWarning("Leak prefab or spawn points not set for CoolingPipeProblem.");
        }
        PlayerStatus._instance.coolingProblem = true;
        PlayerStatus._instance.severeWaterLeak = currentLeak.GetComponent<SevereWaterLeak>();
    }

    private void OnDisable()
    {
        PlayerStatus._instance.coolingProblem = false;
        if(currentLeak != null)
        {
            Destroy(currentLeak);
        }
        problemWarning.RemoveProblem(4);
        tapeWaypoint.DeactivateWaypoint();
    }
}
