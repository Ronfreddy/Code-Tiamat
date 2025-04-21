using System.Collections.Generic;
using UnityEngine;
using static Utilities.Utility;

public class Enemy_Ranged : EnemyBase
{
    public GameObject projectilePrefab;
    public float attackCooldown = 1f;
    public float projectileSpeed = 5f;
    public int bulletSplitCount = 1;
    public float scatterAngle = 30f;

    public override void InitializeStat(int currentLevel)
    {
        maxHealth = LevelManager.instance.difficultySetting.rangedHPCurve.Evaluate(currentLevel);
        currentHealth = maxHealth;
        baseDamage = Mathf.CeilToInt(LevelManager.instance.difficultySetting.rangedDamageCurve.Evaluate(currentLevel));
        chaseSpeed = LevelManager.instance.difficultySetting.rangedSpeedCurve.Evaluate(currentLevel);
        detectionRange = LevelManager.instance.difficultySetting.rangedDetectRange;
        attackRange = LevelManager.instance.difficultySetting.rangedAttackRange;
        attackCooldown = LevelManager.instance.difficultySetting.rangedAttackCooldown;
        projectileSpeed = LevelManager.instance.difficultySetting.rangedBulletSpeed;
        dropLootChance = LevelManager.instance.difficultySetting.lootDropChance;
        lootTable = LevelManager.instance.difficultySetting.rangedLootTable;
        bulletSplitCount = Random.Range(1, LevelManager.instance.difficultySetting.rangedBulletMaxSplitCount + 1);
        if (RandomChance(dropLootChance))
        {
            lootPart = lootTable.Roll();
        }
    }

    public override void OnAttack()
    {
        Debug.Log("Enemy Ranged Attack");
        LookAtPlayer();

        List<float> angles = new List<float>();
        switch (bulletSplitCount)
        {
            case 1:
                angles.Add(0);
                break;
            case 2:
                angles.Add(-scatterAngle);
                angles.Add(scatterAngle);
                break;
            case 3:
                angles.Add(-scatterAngle);
                angles.Add(0);
                angles.Add(scatterAngle);
                break;
            default:
                Debug.LogError("Call dev to design more multishot pattern");
                break;
        }
        foreach (float angle in angles)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector2 direction = (Quaternion.Euler(0, 0, angle) * (player.position - transform.position)).normalized;
            projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;
            projectile.GetComponent<Projectile>().Initialize(Mathf.CeilToInt(baseDamage / bulletSplitCount));
        }

        AudioManager.Instance.PlayEnemyShootSfx();

        canAttack = false;
        Invoke("ResetAttack", attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}
