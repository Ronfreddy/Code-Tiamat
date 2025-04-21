using TMPro;
using UnityEngine;

public class EffectText : MonoBehaviour
{
    public TextMeshProUGUI effectText;

    public void UpdateEffectText(Modifier modifier)
    {
        effectText.text = ModifierToText.GetText(modifier);
        effectText.color = modifier.color;
    }

    // This can act as a normal text (description) too!
    public void UpdateText(string text, Color color)
    {
        effectText.text = text;
        effectText.color = color;
    }
}

public class ModifierToText
{
    public static string GetText(Modifier modifier)
    {
        string symbol1 = "";    // for symbols like +, -, x, /
        string symbol2 = "";    // for symbols like %, бу, etc.
        string text = "";
        if (modifier.key.Contains("Multiplier"))
        {
            symbol1 = modifier.value > 0 ? "+" : "";
            symbol2 = "%";
        }
        else if (modifier.key.Contains("Addition"))
        {
            symbol1 = modifier.value > 0 ? "+" : "";
        }
        else if (modifier.key == "IsSticky")
        {
            return "Sticks to enemy";
        }

        text = symbol1 + modifier.value.ToString("0.##") + symbol2 + " " + modifier.key.Replace(" Multiplier", "").Replace(" Addition", "");
        return text;
    }
}
