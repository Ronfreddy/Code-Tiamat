using UnityEngine;

[CreateAssetMenu(fileName = "SO_LevelDifficulty", menuName = "Scriptable Objects/SO_LevelDifficulty")]
public class SO_LevelDifficulty : ScriptableObject
{
    public AnimationCurve enemySpawnRate;
    public AnimationCurve fixedEnemyPerRoom;
    public AnimationCurve roomCurve;
    public float lootDropChance;
    public float healDropChance;

    [Header("Melee Enemy")]
    public AnimationCurve meleeDamageCurve;
    public AnimationCurve meleeHPCurve;
    public AnimationCurve meleeSpeedCurve;
    public float meleeDetectRange;
    public float meleeAttackRange;
    public float meleeDashSpeed;
    public float meleeDashCooldown;
    public WeightedPartLootTable meleeLootTable;

    [Header("Ranged Enemy")]
    public AnimationCurve rangedDamageCurve;
    public AnimationCurve rangedHPCurve;
    public AnimationCurve rangedSpeedCurve;
    public float rangedDetectRange;
    public float rangedAttackRange;
    public float rangedAttackCooldown;
    public float rangedBulletSpeed;
    public int rangedBulletMaxSplitCount;
    public WeightedPartLootTable rangedLootTable;
}

public enum Difficulty
{
    Easy,
    Normal,
    Hard,
    Lunatic
}
