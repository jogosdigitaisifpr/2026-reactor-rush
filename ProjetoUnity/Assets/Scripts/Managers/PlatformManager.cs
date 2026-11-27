using UnityEngine;

public class PlatformManager : MonoBehaviour
{

    [SerializeField] private GameObject androidInput;
    public bool isAndroid;

    void Awake()
    {
        if(Application.platform != RuntimePlatform.Android)
        {
            androidInput.SetActive(false);
            isAndroid = false;
        }
        else
        {
            androidInput.SetActive(true);
            isAndroid = true;
        }
    }
}
