using UnityEngine;
using UnityEngine.UI;

public class ItemFrame : MonoBehaviour
{
    [SerializeField] private PlayerInteraction player;
    [SerializeField] private Sprite[] itemSprites;
    private enum ItemNames
    {
        Wrench,
        FireExtinguisher,
        Tape
    };

    private Image itemSpriteRenderer;

    private void Start()
    {
        itemSpriteRenderer = transform.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        UpdateSprite();
    }

    private void UpdateSprite() {
        GameObject item = player.heldItemObject;
        if(item != null) 
        {
            int index = (int)System.Enum.Parse(typeof(ItemNames), item.name);
            itemSpriteRenderer.sprite = itemSprites[index];
            itemSpriteRenderer.enabled = true;
        }
        else
        {
            itemSpriteRenderer.sprite = null;
            itemSpriteRenderer.enabled = false;
        }
    }
}
