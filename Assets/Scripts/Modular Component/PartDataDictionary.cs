using System;
using System.Collections.Generic;
using UnityEngine;

public class PartDataDictionary : MonoBehaviour
{
    public static PartDataDictionary Instance { get; private set; }

    public List<ModularPart> parts = new List<ModularPart>();
    private Dictionary<string, ModularPart> partDictionary = new Dictionary<string, ModularPart>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePartDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePartDictionary()
    {
        foreach (ModularPart part in parts)
        {
            if (!partDictionary.ContainsKey(part.name))
            {
                partDictionary.Add(part.name, part);
            }
            else
            {
                Debug.LogWarning($"Duplicate part name found: {part.name}. Please ensure all part names are unique.");
            }
        }
    }

    public ModularPart GetPartByName(string partName)
    {
        if (partDictionary.TryGetValue(partName, out ModularPart part))
        {
            return part;
        }
        else
        {
            Debug.LogWarning($"Part with name {partName} not found.");
            return null;
        }
    }
}
