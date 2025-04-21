using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Utilities.Utility;

public class PartUIController : MonoBehaviour
{
    public GameObject partSlotPrefab;         // UI slot prefab for parts
    public Transform inventoryGrid;           // Parent for inventory grid

    [Header("Weapon Reference & UI")]
    public WeaponBase selectedWeapon;       // Reference to player's weapon
    public GameObject weaponUIHolder;       // UI holder for weapons
    public GameObject swordUIPrefab;
    public GameObject bowUIPrefab;
    public GameObject bombUIPrefab;

    [Header("Selected Part Reference")]
    public ModularPart selectedPart;        // Currently selected part
    public ModularPart weaponPart;

    [Header("Interaction Buttons")]
    public Button equipButton;              // Button to equip selected part
    public Button unequipButton;            // Button to unequip selected part
    public Button throwButton;

    [Header("Description")]
    public Image objectImage;
    public GameObject weaponImageParent;
    public TextMeshProUGUI objectRarityText;
    public TextMeshProUGUI objectNameText;
    public TextMeshProUGUI objectWeaponText;
    public Transform descriptionTextParent;
    public GameObject effectTextPrefab;
    private List<GameObject> spawnableTexts = new List<GameObject>();

    void OnEnable()
    {
        Time.timeScale = 0;
        PopulateInventoryUI();
        ClearDescription();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (selectedPart != null)
        {
            equipButton.interactable = true;
            equipButton.gameObject.SetActive(true);
        }
        else
        {
            equipButton.interactable = false;
            equipButton.gameObject.SetActive(false);
        }

        if(weaponPart != null)
        {
            unequipButton.interactable = true;
            unequipButton.gameObject.SetActive(true);
        }
        else
        {
            unequipButton.interactable = false;
            unequipButton.gameObject.SetActive(false);
        }

        if (weaponPart != null || selectedPart != null)
        {
            throwButton.interactable = true;
        }
        else
        {
            throwButton.interactable = false;
        }
    }

    #region Inventory Side
    // Populate the inventory UI with available parts
    public void PopulateInventoryUI()
    {
        ClearInventoryUI();  // Clear old entries

        foreach (InventoryPart part in InventoryManager.Instance.partInventory.equippedParts)
        {
            GameObject slot = Instantiate(partSlotPrefab, inventoryGrid);
            PartButton partButton = slot.GetComponent<PartButton>();
            partButton.RecordPart(part);  // Record the part in the button

            Button equipButton = slot.GetComponent<Button>();
            equipButton.onClick.AddListener(() =>
            {
                SelectPart(part.partType);
            });
        }
        ChangeSelectedWeapon(selectedWeapon);
    }

    // Equip a selected part
    public void EquipPart()
    {
        if (selectedPart == null) return;
        if (selectedWeapon.EquipPart(selectedPart))
        {
            InventoryManager.Instance.RemovePart(selectedPart);  // Remove from inventory
            selectedPart = null;

            // Refresh Inventory UI
            PopulateInventoryUI();
            ClearDescription();
        }
    }

    // Unequip a selected part
    public void UnequipPart()
    {
        if (weaponPart == null) return;
        selectedWeapon.UnequipPart(weaponPart.partType);
        InventoryManager.Instance.AddPart(weaponPart);
        weaponPart = null;


        // Refresh Inventory UI
        PopulateInventoryUI();
        ClearDescription();
    }

    // Throw a selected part
    public void ThrowPart()
    {
        if (selectedPart == null && weaponPart == null) return;
        if (selectedPart != null)
        {
            InventoryManager.Instance.RemovePart(selectedPart);
            selectedPart = null;
        }
        else if (weaponPart != null)
        {
            selectedWeapon.UnequipPart(weaponPart.partType);
            weaponPart = null;
        }

        //Refresh Inventory UI
        PopulateInventoryUI();
        ClearDescription();
    }

    // Clear Inventory UI
    public void ClearInventoryUI()
    {
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }
    }

    public void ChangeSelectedWeapon(WeaponBase weapon)
    {
        selectedWeapon = weapon;
        foreach (Transform child in weaponUIHolder.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject weaponUI = Instantiate(GetWeaponUIPrefab(weapon), weaponUIHolder.transform);
        WeaponShowUp weaponShowUp = weaponUI.GetComponent<WeaponShowUp>();
        foreach (ModularPartSlot slot in weapon.slots)
        {
            weaponShowUp.DisplayWeaponParts(slot);
        }

        foreach (PartButton button in weaponShowUp.parts)
        {
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectWeaponPart(button.part);
            });
        }

        ClearDescription();
        UpdateDescription(weapon);
    }

    private GameObject GetWeaponUIPrefab(WeaponBase weapon)
    {
        switch (weapon.weaponType)
        {
            case "Sword":
                return swordUIPrefab;
            case "Bow":
                return bowUIPrefab;
            case "Bomb":
                return bombUIPrefab;
            default:
                return null;
        }
    }

    public void SelectPart(ModularPart part)
    {
        selectedPart = part;
        weaponPart = null; // Reset weapon part selection
        ClearDescription();
        UpdateDescription(part);
    }

    public void SelectWeaponPart(ModularPart part)
    {
        weaponPart = part;
        selectedPart = null; // Reset part selection
        ClearDescription();
        UpdateDescription(part);
    }

    #endregion

    #region Description Side
    private void UpdateDescription(ModularPart part)
    {
        if(part == null) return;
        objectImage.sprite = part.partImage;
        objectImage.color = new Color(1, 1, 1, 1);
        objectRarityText.text = part.rarity.ToString();
        objectRarityText.color = GetRarityColor(part.rarity);
        objectNameText.text = part.partName;
        objectNameText.color = GetRarityColor(part.rarity);
        objectWeaponText.text = "Equip on " + part.compatibleWeapon;
        foreach(Modifier modifier in part.modifiers)
        {
            GameObject effectText = Instantiate(effectTextPrefab, descriptionTextParent);
            effectText.GetComponent<EffectText>().UpdateEffectText(modifier);
            spawnableTexts.Add(effectText);
        }
        GameObject descriptionText = Instantiate(effectTextPrefab, descriptionTextParent);
        descriptionText.GetComponent<EffectText>().UpdateText(part.description, Color.white);
        spawnableTexts.Add(descriptionText);
    }

    private void UpdateDescription(WeaponBase weapon)
    {
        GameObject weaponImageObject = Instantiate(weapon.descriptionImage, weaponImageParent.transform);
        objectNameText.text = weapon.weaponType.ToString();
        foreach (string description in weapon.GetStatDescriptions())
        {
            GameObject effectText = Instantiate(effectTextPrefab, descriptionTextParent);
            effectText.GetComponent<EffectText>().UpdateText(description, Color.white);
            spawnableTexts.Add(effectText);
        }
    }

    private void ClearDescription()
    {
        objectImage.sprite = null;
        objectImage.color = new Color(1, 1, 1, 0);
        if(weaponImageParent.transform.childCount > 0)
        {
            Destroy(weaponImageParent.transform.GetChild(0).gameObject);
        }
        objectRarityText.text = "";
        objectRarityText.color = Color.white;
        objectNameText.text = "";
        objectNameText.color = Color.white;
        objectWeaponText.text = "";
        foreach(GameObject text in spawnableTexts)
        {
            Destroy(text);
        }
        spawnableTexts.Clear();
    }

    #endregion
}
