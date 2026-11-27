using UnityEngine;

public class LiftArea : MonoBehaviour
{
    public bool isPlayerInLiftArea = false;
    private const string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInLiftArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInLiftArea = false;
        }
    }
}
