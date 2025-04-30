using UnityEngine;

[CreateAssetMenu(fileName = "ModularPart", menuName = "Scriptable Objects/ModularPart")]
public class ModularPart : ScriptableObject
{
    public string partName;
    public string partType;
    public string compatibleWeapon;
    public Sprite partImage;
    public Sprite lootSprite;
    public Rarity rarity;
    public Modifier[] modifiers;
    [TextArea(15,20)]
    public string description = "";

    public float GetModifier(string key)
    {
        foreach (Modifier modifier in modifiers)
        {
            if (modifier.key == key)
            {
                return modifier.value;
            }
        }
        return 0f;
    }
}

[System.Serializable]
public class Modifier
{
    public string key;
    public float value;
    public Color color;
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
