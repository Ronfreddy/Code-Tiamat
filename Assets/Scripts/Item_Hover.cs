using TMPro;
using UnityEngine;
using static Utilities.Utility;

public class Item_Hover : MonoBehaviour
{
    public static Item_Hover instance;
    public bool isHovering = false; // Flag to check if the item is being hovered over
    public GameObject hoverImage;
    public Vector3 mousePos = Vector3.zero;

    [Header("Item Descriptions")]
    public TextMeshProUGUI itemNameText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isHovering)
        {
            hoverImage.SetActive(false); 
            return;
        }
        else
        {
            hoverImage.SetActive(true); 
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in viewport coordinates
            mousePos.z = 0; // Set z to 0 to keep it in 2D space
            GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        }
    }

    public void EnableHover()
    {
        isHovering = true;
    }

    public void DisableHover()
    {
        isHovering = false;
    }

    public void UpdateDescription(ModularPart part)
    {
        itemNameText.text = part.partName;
        itemNameText.color = GetRarityColor(part.rarity);
    }
}
