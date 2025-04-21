using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartListHolder", menuName = "Scriptable Objects/PartListHolder")]
public class PartListHolder : ScriptableObject
{
    public List<ModularPart> parts = new List<ModularPart>();
}
