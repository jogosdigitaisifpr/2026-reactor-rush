using UnityEngine;
using UnityEngine.UI;

public class RadiationCounter : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private float[] spritesIntervals;
    private Image spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<Image>();
        spritesIntervals = new float[]{0.5f, 2.5f, 4f, 5f, 6f, 7f, 8f};
    }

    private void Update()
    {
        UpdateSprite();        
    }

    private void UpdateSprite()
    {
        float radiationLevel = 0;
        if(ReloadController.Instance != null)
        {
            radiationLevel = ReloadController.Instance.radiationCounter;
        }

        for(int i = 0; i < sprites.Length; i++)
        {
            if(spritesIntervals[i] > radiationLevel) {
                spriteRenderer.sprite = sprites[i];
                break;
            }
        }
    }
}
