using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartButton : MonoBehaviour
{
    public string partType;
    private int partQuantity;
    public ModularPart part; // Reference to the part this button represents
    public GameObject partImage; // UI element to display the part image
    public TextMeshProUGUI partCount;

    public void RecordPart(InventoryPart part)
    {
        this.part = part.partType; // Store the part reference
        partQuantity = part.partCount;
        UpdateImage(); // Update the UI image to reflect the part
    }

    public void UpdateImage()
    {
        Image image = partImage.GetComponent<Image>(); // Get the Image component
        if (part != null)
        {
            image.sprite = part.partImage; // Set the image to the part's image
            image.color = new Color(1, 1, 1, 1); // Make it visible
            if(partQuantity > 1)
                partCount.text = partQuantity.ToString();
        }
        else
        {
            image.sprite = null; // Clear the image if no part is assigned
            image.color = new Color(1, 1, 1, 0); // Make it transparent

        }
    }

    public void ShowDescription()
    {
        if(part != null)
        {
            Item_Hover.instance.EnableHover(); // Enable hover effect
            Item_Hover.instance.UpdateDescription(part); // Set item name
        }
        else
        {
            Debug.Log("No part assigned to this button.");
        }
    }

    public void HideDescription()
    {
        Item_Hover.instance.DisableHover(); // Disable hover effect
    }
}
