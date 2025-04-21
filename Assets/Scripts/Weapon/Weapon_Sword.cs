using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword : WeaponBase
{
    private float normalSize = 0.8f;
    private float basicLifeSteal = 0f;
    public float cooldownTime = 1f;
    public float attackTime = 0.1f;
    public float cooldownTimer = 0.0f;
    private bool isAttacking = false;
    public Character_HealthSystem player;

    [Header("Stat Multipliers")]
    public float attackAddition = 0f;
    public float attackMultiplier = 100f;
    public float cooldownMultiplier = 100f;
    public float sizeMultiplier = 100f;
    public float lifeStealAddition = 0f;

    #region Modified Stat
    public float AttackDamage
    {
        get
        {
            return Mathf.Clamp((baseDamage + attackAddition) * Mathf.Clamp(attackMultiplier / 100, 1f, 999999f), 1f, 999999f);
        }
    }

    public float AttackCooldown
    {
        get
        {
            return cooldownTime / Mathf.Clamp(cooldownMultiplier / 100, 0.1f, 10f);
        }
    }

    public float SwordSize
    {
        get
        {
            return Mathf.Clamp(normalSize * Mathf.Clamp(sizeMultiplier / 100, 0f, 999999f), 0.5f, 2f);
        }
    }

    public float LifeSteal
    {
        get
        {
            return Mathf.Clamp(basicLifeSteal + lifeStealAddition, 0f, 10f);
        }
    }
    #endregion

    private void Update()
    {
        // Update Stamina Bar
        if(isAttacking)
        {
            float totalCooldownTime = AttackCooldown + attackTime;
            UIController.Instance.UpdateStaminaBar(totalCooldownTime - cooldownTimer, totalCooldownTime);
        }
        else
        {
            UIController.Instance.UpdateStaminaBar(10, 10);
        }


        if (isAttacking)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0.0f)
            {
                isAttacking = false;
                PlayerInputController.Instance.canRotate = true;
            }
        }
    }

    public override List<string> GetStatDescriptions()
    {
        List<string> result = new List<string>
        {
            FormatStat("Damage", baseDamage, AttackDamage),
            FormatStat("Cooldown", cooldownTime, AttackCooldown, "s"),
            FormatStat("Size", normalSize, SwordSize),
            FormatStat("Lifesteal", basicLifeSteal, LifeSteal)
        };
        return result;
    }

    public override void OnAttackKeyDown()
    {
        if (isAttacking)
        {
            return;
        }
        Debug.Log("Input: Attack Key Down - Sword");

        // Play the attack animation
        Animator animator = GetComponent<Animator>();
        AudioManager.Instance.PlaySwordAttackSfx();
        animator.Play("Sword_Attack");
        animator.SetFloat("RecoverMultiplier", (100 + cooldownMultiplier) / 100);
        
        isAttacking = true;
        PlayerInputController.Instance.canRotate = false;
        cooldownTimer = AttackCooldown + attackTime;
        Debug.Log("Cooldown Timer: " + cooldownTimer);
    }

    public override void OnAttackKeyUp(Vector2 aimDirection)
    {
        Debug.Log("Input: Attack Key Up - Sword");
    }

    public override void UpdateStat()
    {
        ResetMultiplier();
        foreach(ModularPartSlot slot in slots)
        {
            if (slot.part != null)
            {
                attackAddition += slot.part.GetModifier("Damage Addition");
                attackMultiplier += slot.part.GetModifier("Damage Multiplier");
                cooldownMultiplier += slot.part.GetModifier("Cooldown Multiplier");
                sizeMultiplier += slot.part.GetModifier("Size Multiplier");
                lifeStealAddition += slot.part.GetModifier("Lifesteal Addition");
            }
        }
        transform.localScale = new Vector3(SwordSize, SwordSize, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBase>().TakeDamage(AttackDamage);

            // Knockback (got bug dont put first)
            Vector2 knockbackDirection = (collision.transform.position - transform.parent.parent.position).normalized;
            collision.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            collision.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * 4f, ForceMode2D.Impulse);

            player.Heal(LifeSteal);
        }

        if (collision.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void ResetMultiplier()
    {
        attackAddition = 0f;
        attackMultiplier = 100f;
        cooldownMultiplier = 100f;
        sizeMultiplier = 100f;
        lifeStealAddition = 0f;
    }
}
