using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class DifficultyEditor : EditorWindow
{
    public SO_LevelDifficulty difficulty;

    [MenuItem("Tools/Difficulty Editor %#e")]
    public static void OpenWindow()
    {
        GetWindow<DifficultyEditor>("Difficulty Settings");
    }

    private void OnGUI()
    {
        difficulty = (SO_LevelDifficulty)EditorGUILayout.ObjectField("Difficulty Settings", difficulty, typeof(SO_LevelDifficulty), false);

        if (difficulty == null)
        {
            difficulty = Resources.Load<SO_LevelDifficulty>("ScriptableObjects/LevelDifficulty");
        }

        if (difficulty != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Difficulty Settings", EditorStyles.boldLabel);
            difficulty.enemySpawnRate = EditorGUILayout.CurveField("Enemy Spawn Rate", difficulty.enemySpawnRate);
            difficulty.fixedEnemyPerRoom = EditorGUILayout.CurveField("Constant Enemy Per Room", difficulty.fixedEnemyPerRoom);
            difficulty.roomCurve = EditorGUILayout.CurveField("Room Count", difficulty.roomCurve);
            difficulty.lootDropChance = EditorGUILayout.FloatField("Loot Drop Chance", difficulty.lootDropChance);
            difficulty.healDropChance = EditorGUILayout.FloatField("Heal Bag Drop Chance", difficulty.healDropChance);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Melee Enemy Settings", EditorStyles.boldLabel);
            difficulty.meleeDamageCurve = EditorGUILayout.CurveField("Damage", difficulty.meleeDamageCurve);
            difficulty.meleeHPCurve = EditorGUILayout.CurveField("Health", difficulty.meleeHPCurve);
            difficulty.meleeSpeedCurve = EditorGUILayout.CurveField("Speed", difficulty.meleeSpeedCurve);
            difficulty.meleeDetectRange = EditorGUILayout.FloatField("Detection Range", difficulty.meleeDetectRange);
            difficulty.meleeAttackRange = EditorGUILayout.FloatField("Attack Range", difficulty.meleeAttackRange);
            difficulty.meleeDashSpeed = EditorGUILayout.FloatField("Dash Speed", difficulty.meleeDashSpeed);
            difficulty.meleeDashCooldown = EditorGUILayout.FloatField("Dash Cooldown", difficulty.meleeDashCooldown);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Ranged Enemy Settings", EditorStyles.boldLabel);
            difficulty.rangedDamageCurve = EditorGUILayout.CurveField("Damage", difficulty.rangedDamageCurve);
            difficulty.rangedHPCurve = EditorGUILayout.CurveField("Health", difficulty.rangedHPCurve);
            difficulty.rangedSpeedCurve = EditorGUILayout.CurveField("Speed", difficulty.rangedSpeedCurve);
            difficulty.rangedDetectRange = EditorGUILayout.FloatField("Detection Range", difficulty.rangedDetectRange);
            difficulty.rangedAttackRange = EditorGUILayout.FloatField("Attack Range", difficulty.rangedAttackRange);
            difficulty.rangedAttackCooldown = EditorGUILayout.FloatField("Attack Cooldown", difficulty.rangedAttackCooldown);
            difficulty.rangedBulletSpeed = EditorGUILayout.FloatField("Bullet Speed", difficulty.rangedBulletSpeed);
            difficulty.rangedBulletMaxSplitCount = EditorGUILayout.IntField("Bullet Max Split Count", difficulty.rangedBulletMaxSplitCount);
        }
        else
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("No Difficulty Settings found.");
        }
    }
}
#endif