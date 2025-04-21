using TMPro;
using UnityEngine;

public class Tutorial_ChangeWeaponGuide : MonoBehaviour
{
    public string swordGuide;
    public string bowGuide;
    public string bombGuide;
    public TextMeshPro weaponGuideText;

    public PartPickup sampleLoot;
    public ModularPart samplePart;

    private void Start()
    {
        PlayerInputController.Instance.OnWeaponChange += OnWeaponChange;
        sampleLoot.InitializePart(samplePart);
    }

    private void OnDestroy()
    {
        PlayerInputController.Instance.OnWeaponChange -= OnWeaponChange;
    }

    private void OnWeaponChange(int weaponIndex)
    {
        switch (weaponIndex)
        {
            case 0:
                weaponGuideText.text = swordGuide;
                break;
            case 1:
                weaponGuideText.text = bowGuide;
                break;
            case 2:
                weaponGuideText.text = bombGuide;
                break;
        }
    }
}
