using UnityEngine;

namespace Utilities
{
    public static class Utility
    {
        // I want to save my brain whenever I need to pray to RNGesus
        public static bool RandomChance(float chance)
        {
            return Random.Range(0f, 100f) < chance;
        }

        // I want to save my life whenever I have to find rarity color
        public static Color GetRarityColor(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => Color.white,
                Rarity.Rare => Color.blue,
                Rarity.Epic => new Color(0.5f, 0f, 0.5f),
                Rarity.Legendary => Color.yellow,
                _ => Color.white
            };
        }
    }
}
