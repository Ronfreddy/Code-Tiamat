using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartInventory
{
    public int maxInventorySize = 0;
    public List<InventoryPart> equippedParts = new List<InventoryPart>();
    public GameObject inventoryUI; 

    public void InitializeInventorySlot(int inventorySlotCount)
    {
        maxInventorySize = inventorySlotCount;
    }

    public bool AddPart(ModularPart part)
    {
        if (equippedParts.Count < maxInventorySize)
        {
            if (IsPartInInventory(part))
            {
                equippedParts.Find(x => x.partType == part).partCount++;
            }
            else
            {
                InventoryPart inventoryPart = new InventoryPart(part);
                equippedParts.Add(inventoryPart);
            }

            if(inventoryUI.activeSelf) 
                inventoryUI.GetComponent<PartUIController>().PopulateInventoryUI(); // Refresh the inventory UI

            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemovePart(ModularPart part)
    {
        if (IsPartInInventory(part))
        {
            equippedParts.Find(x => x.partType == part).partCount--;
            if (equippedParts.Find(x => x.partType == part).partCount <= 0)
            {
                equippedParts.Remove(equippedParts.Find(x => x.partType == part));
            }
        }
    }

    public bool IsInventoryFull()
    {
        return equippedParts.Count >= maxInventorySize;
    }

    public bool IsPartInInventory(ModularPart part)
    {
        foreach (InventoryPart inventoryPart in equippedParts)
        {
            if (inventoryPart.partType == part)
            {
                return true;
            }
        }
        return false;
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public PartInventory partInventory = new PartInventory();
    public bool isInventoryOpen = false;
    public int inventorySlots = 20;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        partInventory.InitializeInventorySlot(inventorySlots);
    }

    // Toggle inventory visibility
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (isInventoryOpen)
        {
            // Show inventory UI
            LoadInventory();
            Debug.Log("Inventory Opened");
            partInventory.inventoryUI.SetActive(true);
        }
        else
        {
            // Hide inventory UI
            Debug.Log("Inventory Closed");
            partInventory.inventoryUI.SetActive(false);
            Item_Hover.instance.DisableHover();
        }
    }

    // Add part
    public bool AddPart(ModularPart part)
    {
        if (partInventory.AddPart(part))
        {
            SaveInventory();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Remove part
    public void RemovePart(ModularPart part)
    {
        partInventory.RemovePart(part);
        SaveInventory();
    }

    // Save part inventory
    public void SaveInventory()
    {
        InventoryPartJsonPackage inventoryPartJsonPackage = new InventoryPartJsonPackage();
        inventoryPartJsonPackage.inventoryParts = new List<InventoryPartJson>();
        foreach (InventoryPart inventoryPart in partInventory.equippedParts)
        {
            InventoryPartJson inventoryPartJson = new InventoryPartJson
            {
                partType = inventoryPart.partType.name,
                partCount = inventoryPart.partCount
            };
            inventoryPartJsonPackage.inventoryParts.Add(inventoryPartJson);
        }
        string json = JsonUtility.ToJson(inventoryPartJsonPackage);
        PlayerPrefs.SetString("PartInventory", json);
        PlayerPrefs.Save();
    }

    // Load part inventory
    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey("PartInventory"))
        {
            string json = PlayerPrefs.GetString("PartInventory");
            InventoryPartJsonPackage inventoryPartJsonPackage = JsonUtility.FromJson<InventoryPartJsonPackage>(json);
            partInventory.equippedParts.Clear();
            foreach (InventoryPartJson inventoryPartJson in inventoryPartJsonPackage.inventoryParts)
            {
                ModularPart part = PartDataDictionary.Instance.GetPartByName(inventoryPartJson.partType);
                if (part != null)
                {
                    InventoryPart inventoryPart = new InventoryPart(part);
                    inventoryPart.partCount = inventoryPartJson.partCount;
                    partInventory.equippedParts.Add(inventoryPart);
                }
                else
                {
                    Debug.LogWarning("Part not found: " + inventoryPartJson.partType);
                }
            }
        }
    }

    public void ResetInventory()
    {
        partInventory.equippedParts.Clear();
        PlayerPrefs.DeleteKey("Sword");
        PlayerPrefs.DeleteKey("Bow");
        PlayerPrefs.DeleteKey("Bomb");
        SaveInventory();
    }
}

[System.Serializable]
public class InventoryPart
{
    public ModularPart partType;
    public int partCount;

    public InventoryPart(ModularPart part)
    {
        partType = part;
        partCount = 1;
    }
}

[System.Serializable]
public class InventoryPartJsonPackage
{
    public List<InventoryPartJson> inventoryParts;
}

[System.Serializable]
public class InventoryPartJson
{
    public string partType;
    public int partCount;
}
