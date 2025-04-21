using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Utilities.Utility;

public class PartPickup : MonoBehaviour
{
    public ModularPart part;
    public SpriteRenderer spriteRenderer;
    public Light2D rarityGlow;

    public void InitializePart(ModularPart newPart)
    {
        part = newPart;
        spriteRenderer.sprite = part.lootSprite;
        rarityGlow.color = GetRarityColor(part.rarity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (InventoryManager.Instance.AddPart(part))
            {
                UIController.Instance.ShowPartPopUp(part);
                AudioManager.Instance.PlayPickUpSfx();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Cannot pick up. Inventory is full.");
            }
        }
    }
}
