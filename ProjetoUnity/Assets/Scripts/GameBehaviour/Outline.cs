using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField] private RenderingLayerMask outlineLayer;

    private Renderer[] renderers;
    private uint originalLayer;
    private bool isOutlineActive;

    private void Start()
    {
        renderers = TryGetComponent<Renderer>(out var meshRenderer)
            ? new[] { meshRenderer }
            : GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            originalLayer = renderers[0].renderingLayerMask;
        }
    }

    public void ShowOutline()
    {
        if (isOutlineActive) return;
        isOutlineActive = true;
        SetOutline(true);
    }

    public void HideOutline()
    {
        if (!isOutlineActive) return;
        isOutlineActive = false;
        SetOutline(false);
    }

    private void SetOutline(bool enable)
    {
        foreach (var rend in renderers)
        {
            if (rend == null) continue;

            rend.renderingLayerMask = enable
                ? originalLayer | outlineLayer
                : originalLayer;
        }
    }

    private void OnDisable()
    {
        if (isOutlineActive)
        {
            HideOutline();
        }
    }
}