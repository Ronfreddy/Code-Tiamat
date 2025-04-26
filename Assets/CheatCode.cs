using UnityEngine;

public class CheatCode : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            FillUpEverything();
        }
    }

    private void FillUpEverything()
    {
        foreach (ModularPart part in PartDataDictionary.Instance.parts)
        {
            InventoryManager.Instance.AddPart(part);
        }
    }
}
