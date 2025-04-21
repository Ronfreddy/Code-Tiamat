using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Utilities.Utility;

public class ItemPopUp : MonoBehaviour
{
    public Image partImage;
    public TextMeshProUGUI partNameText;
    
    public void UpdatePartName(ModularPart part)
    {
        partImage.sprite = part.partImage;
        partNameText.text = part.rarity.ToString() + " " + part.partName;
        partNameText.color = GetRarityColor(part.rarity);
        partNameText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, partNameText.preferredWidth + 20);
    }
}
