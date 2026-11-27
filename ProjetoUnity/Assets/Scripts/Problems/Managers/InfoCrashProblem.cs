using UnityEngine;
using WrightAngle.Waypoint;

public class InfoCrashProblem : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private HUDController hud;
    [SerializeField] private GameObject problemWarning;
    [SerializeField] private WaypointTarget npcWaypoint;
    // private bool visited = false;
    
    private void OnEnable()
    {
        playerStatus.infoCrashProblem = true;
        if(problemWarning != null) problemWarning.SetActive(true);
        npcWaypoint.ActivateWaypoint();
    }

    private void OnDisable()
    {
        playerStatus.infoCrashProblem = false;
        if(problemWarning != null) problemWarning.SetActive(false);
        npcWaypoint.DeactivateWaypoint();
        // visited = false;
    }

}
