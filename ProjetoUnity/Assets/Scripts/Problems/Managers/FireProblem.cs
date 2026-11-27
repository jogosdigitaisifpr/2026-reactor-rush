using UnityEngine;
using WrightAngle.Waypoint;

public class FireProblem : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private Transform[] fireSpawnPoints;
    [SerializeField] private ProblemWarningIcons problemWarning;
    [SerializeField] private WaypointTarget extinguisherWaypoint;

    private GameObject currentFire;

    private void OnEnable()
    {
        Debug.Log("Fire problem activated!");
        extinguisherWaypoint.ActivateWaypoint();
        SpawnFire();
    }

    private void SpawnFire()
    {
        if (firePrefab != null && fireSpawnPoints != null)
        {
            int spawnIndex = Random.Range(0, fireSpawnPoints.Length);
            currentFire = Instantiate(firePrefab, fireSpawnPoints[spawnIndex].position, firePrefab.transform.rotation);
            currentFire.GetComponent<Fire>().fireProblem = gameObject; // Link the fire to this problem for deactivation when extinguished
        }
        else
        {
            Debug.LogWarning("Fire prefab or spawn points not set for FireProblem.");
        }
        PlayerStatus._instance.fireProblem = true;
    }

    private void OnDisable()
    {
        PlayerStatus._instance.fireProblem = false;
        if(currentFire != null)
        {
            Destroy(currentFire);
        }
        problemWarning.RemoveProblem(2);
        extinguisherWaypoint.DeactivateWaypoint();
    }
}