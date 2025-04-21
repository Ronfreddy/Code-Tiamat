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
        float commonWeight = EditorGUILayout.FloatField("Common Weight", 50);
        float rareWeight = EditorGUILayout.FloatField("Rare Weight", 30);
        float epicWeight = EditorGUILayout.FloatField("Epic Weight", 10);
        float legendaryWeight = EditorGUILayout.FloatField("Legendary Weight", 3);

        if (GUILayout.Button("Auto-Fill Weights"))
        {
            foreach (ModularPart item in lootItems.parts)
            {
                string name = item.name;
                if (name.EndsWith("_I"))
                {
                    lootTable.weightedItems.Add(new WeightedItem<ModularPart>(item, commonWeight));
                }
                else if (name.EndsWith("_II"))
                {
                    lootTable.weightedItems.Add(new WeightedItem<ModularPart>(item, rareWeight));
                }
                else if (name.EndsWith("_III"))
                {
                    lootTable.weightedItems.Add(new WeightedItem<ModularPart>(item, epicWeight));
                }
                else if (name.EndsWith("_IV"))
                {
                    lootTable.weightedItems.Add(new WeightedItem<ModularPart>(item, legendaryWeight));
                }
            }
            EditorUtility.SetDirty(lootTable);
            Debug.Log("Weights filled based on rarity suffix.");
        }
    }
}
#endif