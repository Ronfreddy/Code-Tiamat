using System.Collections.Generic;
using UnityEngine;

public class WeaponShowUp : MonoBehaviour
{
    public List<PartButton> parts = new List<PartButton>();

    public void DisplayWeaponParts(ModularPartSlot part)
    {
        foreach (PartButton button in parts)
        {
            if (button.partType == part.compatibleType)
            {
                button.RecordPart(new InventoryPart(part.part));
            }
        }
    }
}
