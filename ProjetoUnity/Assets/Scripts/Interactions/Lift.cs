using UnityEngine;
using System.Collections;

public class Lift : MonoBehaviour
{
    [SerializeField] private Transform[] liftPositions;
    [SerializeField] private GameObject liftCage;
    [SerializeField] private LiftArea liftArea;
    [SerializeField] private float liftSpeed = 2f;
    [SerializeField] private GameObject player;

    private float playerHeightOffset = 1.0f;

    private int currentPositionIndex = 0;

    public void MoveLift()
    {
        if(!liftArea.isPlayerInLiftArea)
        {
            Debug.Log("Player is not in the lift area. Cannot move the lift.");
            return;
        }
    
        if (player == null)
        {
            Debug.LogWarning("Player reference is not set in Lift script.");
            return;
        }
        
        playerHeightOffset = player.transform.position.y - transform.position.y;

        int positionIndex = (currentPositionIndex + 1) % liftPositions.Length;
        currentPositionIndex = positionIndex;
        liftCage.SetActive(true);
        StartCoroutine(MoveToPosition(liftPositions[currentPositionIndex].position));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, liftSpeed * Time.deltaTime);
            player.transform.position = new Vector3(player.transform.position.x, transform.position.y + playerHeightOffset, player.transform.position.z);
            yield return null;
        }
        transform.position = targetPosition; // Ensure exact position
        player.transform.position = new Vector3(player.transform.position.x, transform.position.y + playerHeightOffset, player.transform.position.z); // Ensure exact position for player
        liftCage.SetActive(false);
    }
}
