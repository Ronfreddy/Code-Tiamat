using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class LootWeightAutoFiller : EditorWindow
{
    public WeightedPartLootTable lootTable;
    public PartListHolder lootItems;

    [MenuItem("Tools/Auto-Fill Loot Weights")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LootWeightAutoFiller));
    }

    void OnGUI()
    {
        lootTable = (WeightedPartLootTable)EditorGUILayout.ObjectField("Loot Table", lootTable, typeof(WeightedPartLootTable), false);
        lootItems = (PartListHolder)EditorGUILayout.ObjectField("Loot Items", lootItems, typeof(PartListHolder), false);

        if (GUILayout.Button("Auto-Fill Weights"))
        {
            foreach (ModularPart item in lootItems.parts)
            {
                string name = item.name;
                if (name.EndsWith("_I"))
                {
                    lootTable.weightedItems.Add(new WeightedItem<ModularPart>(item, 50));
                }
                else if (name.EndsWith("_II"))
                {
                    lootTable.weightedItems.Add(new WeightedItem<ModularPart>(item, 30));
                }
                else if (name.EndsWith("_III"))
                {
                    lootTable.weightedItems.Add(new WeightedItem<ModularPart>(item, 10));
                }
                else if (name.EndsWith("_IV"))
                {
                    lootTable.weightedItems.Add(new WeightedItem<ModularPart>(item, 1));
                }
            }
            EditorUtility.SetDirty(lootTable);
            Debug.Log("Weights filled based on rarity suffix.");
        }
    }
}
#endif