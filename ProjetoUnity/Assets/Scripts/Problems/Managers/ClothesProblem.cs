using UnityEngine;
using WrightAngle.Waypoint;

public class ClothesProblem : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private ProblemWarningIcons problemWarning;
    [SerializeField] private WaypointTarget wardrobeWaypoint;

    private void OnEnable() {
        playerStatus.rippedClothes = true;
        wardrobeWaypoint.ActivateWaypoint();
    }

    private void OnDisable() {
        playerStatus.rippedClothes = false;
        problemWarning.RemoveProblem(1);
        wardrobeWaypoint.DeactivateWaypoint();
    }
}
