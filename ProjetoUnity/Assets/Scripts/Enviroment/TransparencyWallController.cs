using UnityEngine;

public class TransparencyWallController : MonoBehaviour
{
    private static readonly int WidthPropertyID = Shader.PropertyToID("_TransparencyWidth");
    private Material targetMaterial;
    public float transparencyWidth = 0f;
    private float targetTransparency = 5f;
    private float speedChange = 2f;

    public bool isOnCamera = false;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            targetMaterial = renderer.material;
        }
        else
        {
            Debug.Log("Renderer NUll");
        }
    }

    void Update()
    {
        if (targetMaterial != null)
        {
            if(transparencyWidth < targetTransparency && isOnCamera)
            {
                transparencyWidth += Time.deltaTime * speedChange;
                Debug.Log(transparencyWidth);
                Debug.Log("Turning Transparent");
            }
            if(transparencyWidth > 0 && !isOnCamera)
            {
                transparencyWidth -= Time.deltaTime * speedChange;
                Debug.Log(transparencyWidth);
                Debug.Log("Turning Opaque");
            }

            targetMaterial.SetFloat(WidthPropertyID, transparencyWidth);
        }
        else
        {
            Debug.Log("Target Material NUll");
        }
    }
}
