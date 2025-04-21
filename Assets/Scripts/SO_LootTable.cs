using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_LootTable", menuName = "Scriptable Objects/SO_LootTable")]
public class SO_LootTable : ScriptableObject
{
    public float dropProbability = 50f; // 100 = 100%
    public List<ModularPart> Parts = new List<ModularPart>();
}
