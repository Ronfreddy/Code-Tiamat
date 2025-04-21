using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public GameObject descriptionImage;

    [Header("Modular Part Slot")]
    public ModularPartSlot[] slots;
    public string weaponType;
    public float baseDamage;

    public virtual List<string> GetStatDescriptions()
    {
        return new List<string>();
    }

    public virtual void OnAttackKeyDown()
    {
        Debug.Log("Input: Attack Key Down");
    }

    public virtual void OnAttackKeyUp(Vector2 aimDirection)
    {
        Debug.Log("Input: Attack Key Up");
    }

    private void OnEnable()
    {
        UpdateStat();
    }

    public virtual bool EquipPart(ModularPart part)
    {
        foreach (ModularPartSlot slot in slots)
        {
            if (slot.compatibleType == part.partType)
            {
                if (slot.part != null)
                {
                    InventoryManager.Instance.AddPart(slot.part);
                    UnequipPart(slot.compatibleType);
                }
                slot.part = part;
                Debug.Log("Equipped part: " + part.partName);
                UpdateStat();
                SaveEquippedParts();
                return true;
            }
        }
        Debug.Log("Part not compatible with weapon");
        return false;
    }

    public virtual void UnequipPart(string partType)
    {
        foreach (ModularPartSlot slot in slots)
        {
            if (slot.compatibleType == partType)
            {
                slot.part = null;
                Debug.Log("Unequipped part: " + partType);
                UpdateStat();
                SaveEquippedParts();
                return;
            }
        }
        Debug.Log("Part not found");
    }

    public virtual void LoadPart(ModularPart part)
    {
        // Totally replace the part from the save data
        foreach (ModularPartSlot slot in slots)
        {
            if (slot.compatibleType == part.partType)
            {
                slot.part = part;
                Debug.Log("Loaded part: " + part.partName);
                UpdateStat();
                return;
            }
        }
    }

    public virtual void UpdateStat()
    {

    }

    public string FormatStat(string label, float baseValue, float currentValue, string suffix = "")
    {
        if (Mathf.Approximately(baseValue, currentValue))
            return $"{label}: {currentValue:0.##}{suffix}";

        string color = "";

        if(suffix == "s")
        {
            color = currentValue > baseValue ? "red" : "green";
        }
        else
        {
            color = currentValue > baseValue ? "green" : "red";
        }
        return $"{label}: <color={color}>{currentValue:0.##}{suffix}</color>";
    }

    public void SaveEquippedParts()
    {
        ModularPartSaveData saveData = new ModularPartSaveData();
        saveData.weaponName = this.name;
        saveData.parts = new List<string>();
        foreach (ModularPartSlot slot in this.slots)
        {
            if (slot.part != null)
            {
                saveData.parts.Add(slot.part.name);
            }
        }
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(this.name, json);
        PlayerPrefs.Save();
    }

    public void LoadEquippedParts()
    {
        string json = PlayerPrefs.GetString(this.name);
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log("No equipped parts found for this weapon");
            return;
        }
        ModularPartSaveData saveData = JsonUtility.FromJson<ModularPartSaveData>(json);
        foreach (string partName in saveData.parts)
        {
            ModularPart part = PartDataDictionary.Instance.GetPartByName(partName);
            if (part != null)
            {
                LoadPart(part);
            }
            else
            {
                Debug.LogWarning("Part not found: " + partName);
            }
        }
    }
}

[System.Serializable]
public class ModularPartSlot
{
    public string compatibleType;
    public ModularPart part;
}


[System.Serializable]
public class ModularPartSaveData
{
    public string weaponName;
    public List<string> parts;
}
