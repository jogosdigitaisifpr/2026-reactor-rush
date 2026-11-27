using UnityEngine;
using System.Collections;
using FMODUnity;

public class CamController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [HideInInspector] public Transform areaCameraPosition, areaPlayerPosition;
    [SerializeField] private float verticalOffset = 8f;

    // private GameObject Obstruction = null;
    
    private Vector3 initialPos, velocity = new Vector3(0, 0, 0);
    private float smoothTime = 1f;

    private StudioEventEmitter woosh;
    
    private float rayDistance;

    void Awake()
    {
        areaCameraPosition = transform;
        initialPos = transform.position;
    }

    private void Start()
    {
        woosh = GetComponent<StudioEventEmitter>();
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, player.position.y + verticalOffset, transform.position.z);
    }

    // void HideObjectInFront()
    // {
    //     RaycastHit hitWall;
    //     if(Physics.Raycast(transform.position, transform.TransformDirection(0, -0.45f, 1f), out hitWall , 5f))
    //     {
    //         if(hitWall.collider.gameObject.CompareTag("Wall"))
    //         {
    //             if(Obstruction == null)
    //             {
    //                 Obstruction = hitWall.collider.gameObject;
    //                 Obstruction.GetComponent<TransparencyWallControler>().isOnCamera = true;    
    //             }
    //             Debug.Log("Is on camera true: " + Obstruction.GetComponent<TransparencyWallControler>().isOnCamera);
    //         }
    //         else
    //         {
    //             if(Obstruction != null)
    //             {
    //                 Debug.Log("SAIU");
    //                 Obstruction.GetComponent<TransparencyWallControler>().isOnCamera = false;
    //                 Debug.Log("Is on camera false: " + Obstruction.gameObject.GetComponent<TransparencyWallControler>().isOnCamera);
    //                 Obstruction = null;
    //             }
    //         }
    //     }
    // }

    public void CumMovement()
    {
        // transform.position = areaCameraPosition.position;
        // transform.rotation = areaCameraPosition.rotation;

        // Smooth and subtle camera movement within a room  

        // Vector3 offsetMove = new Vector3(
        //     Mathf.Clamp(player.position.x - initialPos.x, -offset, offset), 
        //     Mathf.Clamp(player.position.y - initialPos.y, -offset, offset), 
        //     0
        // );

        // Vector3 targetPosition = initialPos + offsetMove;

        StartCoroutine(ChangeArea());

    }

    private IEnumerator ChangeArea() {
        float t = 0;
        //woosh.Play();
        while(t < smoothTime) {
            t += Time.deltaTime;
            float percentage = t / smoothTime;

            Vector3 targetPosition = new Vector3(areaCameraPosition.position.x, player.position.y + verticalOffset, areaCameraPosition.position.z);
            
            transform.position = Vector3.Slerp(transform.position, targetPosition, percentage);
            transform.rotation = Quaternion.Slerp(transform.rotation, areaCameraPosition.rotation, percentage);
            
            

            yield return null;
        }
    }

}
