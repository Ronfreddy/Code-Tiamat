using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeightedPartLootTable", menuName = "Scriptable Objects/WeightedPartLootTable")]
public class WeightedPartLootTable : ScriptableObject
{
    public List<WeightedItem<ModularPart>> weightedItems = new List<WeightedItem<ModularPart>>();

    public ModularPart Roll()
    {
        float totalWeight = 0f;
        foreach (var entry in weightedItems)
            totalWeight += entry.weight;

        float random = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in weightedItems)
        {
            cumulative += entry.weight;
            if (random <= cumulative)
                return entry.item;
        }

        return null; // fallback
    }
}

[System.Serializable]
public class WeightedItem<T>
{
    public T item;
    public float weight;

    public WeightedItem(T item, float weight)
    {
        this.item = item;
        this.weight = weight;
    }
}