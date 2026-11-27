using UnityEngine;

public class DesableCollisionOfReloder : MonoBehaviour
{
    [SerializeField] private Interactable boxCollider;
    void Update()
    {
        boxCollider.enabled = ReloadController.Instance.needReloading;
    }
}
