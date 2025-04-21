using UnityEngine;
using static Utilities.Utility;

public class Enemy_Melee : EnemyBase
{
    public float dashSpeed = 8f;
    public float dashCooldown = 0.8f;
    public GameObject dashTrail;

    public override void InitializeStat(int currentLevel)
    {
        maxHealth = LevelManager.instance.difficultySetting.meleeHPCurve.Evaluate(currentLevel);
        currentHealth = maxHealth;
        baseDamage = Mathf.CeilToInt(LevelManager.instance.difficultySetting.meleeDamageCurve.Evaluate(currentLevel));
        chaseSpeed = LevelManager.instance.difficultySetting.meleeSpeedCurve.Evaluate(currentLevel);
        detectionRange = LevelManager.instance.difficultySetting.meleeDetectRange;
        attackRange = LevelManager.instance.difficultySetting.meleeAttackRange;
        dashSpeed = LevelManager.instance.difficultySetting.meleeDashSpeed;
        dashCooldown = LevelManager.instance.difficultySetting.meleeDashCooldown;
        dropLootChance = LevelManager.instance.difficultySetting.lootDropChance;
        lootTable = LevelManager.instance.difficultySetting.meleeLootTable;
        if (RandomChance(dropLootChance))
        {
            lootPart = lootTable.Roll();
        }
    }
    public override void OnAttack()
    {
        Debug.Log("Enemy Melee Attack");
        canAttack = false;
        LookAtPlayer();
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * dashSpeed;
        Invoke("ResetAttack", dashCooldown);

        // Play the dash trail effect
        dashTrail.SetActive(true);
    }

    private void ResetAttack()
    {
        canAttack = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        dashTrail.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Character_HealthSystem>().TakeDamage(baseDamage);
        }
    }
}
