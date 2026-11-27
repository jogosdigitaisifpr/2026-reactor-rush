using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private GameObject player;

    public void TeleportPlayer()
    {
        player.transform.position = teleportDestination.position;
    }
}
