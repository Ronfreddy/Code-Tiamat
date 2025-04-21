using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Utilities.Utility;

public class PartButton : MonoBehaviour
{
    public string partType;
    private int partQuantity;
    public ModularPart part; // Reference to the part this button represents
    public GameObject partImage; // UI element to display the part image
    public TextMeshProUGUI partCount;

    public void RecordPart(InventoryPart part)
    {
        this.part = part.partType;
        partQuantity = part.partCount;
        UpdateImage();
    }

    public void UpdateImage()
    {
        Image image = partImage.GetComponent<Image>();
        if (part != null)
        {
            GetComponent<Image>().color = GetRarityColor(part.rarity);
            image.sprite = part.partImage;
            image.color = new Color(1, 1, 1, 1);
            if(partQuantity > 1)
                partCount.text = partQuantity.ToString();
        }
        else
        {
            image.sprite = null;
            image.color = new Color(1, 1, 1, 0); 

        }
    }

    public void ShowDescription()
    {
        if(part != null)
        {
            Item_Hover.instance.EnableHover();
            Item_Hover.instance.UpdateDescription(part);
        }
        else
        {
            Debug.Log("No part assigned to this button.");
        }
    }

    public void HideDescription()
    {
        Item_Hover.instance.DisableHover();
    }
}
