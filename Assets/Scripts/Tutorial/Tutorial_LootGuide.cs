using UnityEngine;

public class Tutorial_LootGuide : MonoBehaviour
{
    public GameObject lootGuideToShow;
    public GameObject inventoryTutorial;

    private void OnDestroy()
    {
        lootGuideToShow.SetActive(true);
        inventoryTutorial.SetActive(true);
        inventoryTutorial.GetComponent<Tutorial_InventoryGuide>().canToggle = true;
    }
}
