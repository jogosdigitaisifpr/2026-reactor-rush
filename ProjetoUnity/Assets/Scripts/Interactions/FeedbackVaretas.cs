using UnityEngine;

public class FeedbackVaretas : MonoBehaviour 
{
    [SerializeField] private Transform limit;
    
    private Vector3 initialPosition; 
    private float dist;

    void Start() 
    {

        initialPosition = transform.position; 
    }

    void FixedUpdate() 
    {
    
        dist = Reaction._instance.rodLevel / 100f;
        dist = Mathf.Clamp01(dist);

   
        Vector3 targetPosition = Vector3.Lerp(initialPosition, limit.position, dist);


        transform.position = targetPosition;
    }
}
