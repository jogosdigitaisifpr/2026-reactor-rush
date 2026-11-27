using UnityEngine;

public class NuclearPellet : MonoBehaviour
{
    private float fallSpeed = 5f, targetY = -3f, timeToDestroy = 2f, timepassed = 0f;

    public void Update()
    {
        Fall();
        DestroyPellet();
    }

    private void Fall()
    {
        float newY = Mathf.MoveTowards(transform.position.y, targetY, fallSpeed * Time.unscaledDeltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void DestroyPellet()
    {
        timepassed+= Time.unscaledDeltaTime;
        if(timepassed>=timeToDestroy)
        {
            ReloadController.Instance.RadiationIntoxication();
            Destroy(gameObject);
        }
    }

}
