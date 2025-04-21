using UnityEngine;
using static Utilities.Utility;

public class Enemy_Ranged : EnemyBase
{
    public GameObject projectilePrefab;
    public float attackCooldown = 1f;
    public float projectileSpeed = 5f;

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
        if (RandomChance(dropLootChance))
        {
            lootPart = lootTable.Roll();
        }
    }

    public override void OnAttack()
    {
        Debug.Log("Enemy Ranged Attack");
        LookAtPlayer();
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;
        projectile.GetComponent<Projectile>().Initialize(baseDamage);

        AudioManager.Instance.PlayEnemyShootSfx();

        canAttack = false;
        Invoke("ResetAttack", attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}
