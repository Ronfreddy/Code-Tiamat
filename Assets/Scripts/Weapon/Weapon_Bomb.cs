using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon_Bomb : WeaponBase
{
    public GameObject bombPrefab;
    private bool isCharging = false;
    public float chargeTime;

    private bool isOnCooldown = false;

    [Header("Stats")]
    private float cooldownTimer = 0f;
    public float cooldownTime = 1.0f;
    public float explosionRadius = 1.0f;
    public float maxThrowForce = 10f;
    public float maxChargeTime = 1.0f;
    public float triggerTime = 3.0f;
    public int multishotCount = 0;

    [Header("Stat Multiplier")]
    public float attackAddition = 0.0f;
    public float attackMultiplier = 100f;
    public float cooldownMultiplier = 100f;
    public float radiusAddition = 0.0f;
    public float throwForceMultiplier = 100f;
    public float chargeTimeAddition = 0.0f;
    public float triggerTimeAddition = 0.0f;
    public bool isSticky = false;
    public int multishotAddition = 0;

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

    public float ExplosionRadius
    {
        get
        {
            return Mathf.Clamp(explosionRadius + radiusAddition, 1f, 999999f);
        }
    }

    public float MaxThrowForce
    {
        get
        {
            return Mathf.Clamp(maxThrowForce * Mathf.Clamp(throwForceMultiplier / 100, 0.1f, 999999f), 1f, 100f);
        }
    }

    public float MaxChargeTime
    {
        get
        {
            return Mathf.Clamp(maxChargeTime + chargeTimeAddition, 0.1f, 10f);
        }
    }

    public float TriggerTime
    {
        get
        {
            return Mathf.Clamp(triggerTime + triggerTimeAddition, 0.4f, 10f);
        }
    }

    public int MultishotCount
    {
        get
        {
            return Mathf.Clamp(multishotCount + multishotAddition, 0, 10);
        }
    }
    #endregion

    private void Update()
    {
        if (isCharging)
        {
            UIController.Instance.UpdateStaminaBar(Time.time - chargeTime, MaxChargeTime);
        }
        else if (isOnCooldown)
        {
            UIController.Instance.UpdateStaminaBar(cooldownTimer, AttackCooldown);
        }
        else
        {
            UIController.Instance.UpdateStaminaBar(0, 1);
        }

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0.0f)
            {
                isOnCooldown = false;
                cooldownTimer = 0.0f;
            }
        }
    }

    public override void OnAttackKeyDown()
    {
        if (isOnCooldown) return;
        chargeTime = Time.time;
        isCharging = true;
        AudioManager.Instance.PlayBombChargeSfx();
    }

    public override void OnAttackKeyUp(Vector2 aimDirection)
    {
        if (!isCharging) return;
        base.OnAttackKeyUp(aimDirection);

        // Calculate the hold time
        float holdTime = Time.time - chargeTime;
        holdTime = Mathf.Clamp(holdTime / MaxChargeTime, 0f, 1f);
        float throwForce = MaxThrowForce * holdTime;

        // Spawn the bomb prefab
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);

        // Throw the bomb
        bomb.GetComponent<Detonating_Bomb>().InitializeBomb(AttackDamage, ExplosionRadius, isSticky, TriggerTime);
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        rb.AddForce(aimDirection.normalized * throwForce, ForceMode2D.Impulse);

        // If multishot enabled
        for (int i = 0; i < MultishotCount; i++)
        {
            GameObject multishotBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            float angle = Random.Range(-180f, 180f);
            Vector2 rotatedDirection = Quaternion.Euler(0, 0, angle) * transform.right;
            multishotBomb.GetComponent<Detonating_Bomb>().InitializeBomb(AttackDamage, ExplosionRadius, isSticky, TriggerTime);
            Rigidbody2D multishotRb = multishotBomb.GetComponent<Rigidbody2D>();
            multishotRb.AddForce(rotatedDirection.normalized * throwForce, ForceMode2D.Impulse);
        }

        // Reset the charge state
        isCharging = false;
        isOnCooldown = true;
        cooldownTimer = AttackCooldown;
        AudioManager.Instance.StopBombChargeSfx();
    }

    public override void UpdateStat()
    {
        ResetMultiplier();
        float stickyValue = 0.0f;
        foreach (ModularPartSlot slot in slots)
        {
            if (slot.part != null)
            {
                attackAddition += slot.part.GetModifier("Damage Addition");
                attackMultiplier += slot.part.GetModifier("Damage Multiplier");
                cooldownMultiplier += slot.part.GetModifier("Cooldown Multiplier");
                radiusAddition += slot.part.GetModifier("Radius Addition");
                throwForceMultiplier += slot.part.GetModifier("Throw Force Multiplier");
                chargeTimeAddition += slot.part.GetModifier("Charge Time Addition");
                triggerTimeAddition += slot.part.GetModifier("Trigger Time Addition");
                stickyValue += slot.part.GetModifier("IsSticky");
                multishotAddition += (int)slot.part.GetModifier("Multishot Addition");
            }
        }
        isSticky = stickyValue > 0.0f;
    }

    public override List<string> GetStatDescriptions()
    {
        List<string> result = new List<string>
        {
            FormatStat("Damage", baseDamage, AttackDamage),
            FormatStat("Cooldown", cooldownTime, AttackCooldown),
            FormatStat("Charge Time", maxChargeTime, MaxChargeTime, "s"),
            FormatStat("Throw Force", maxThrowForce, MaxThrowForce),
            FormatStat("Explosion Radius", explosionRadius, ExplosionRadius),
            FormatStat("Trigger Time", triggerTime, TriggerTime, "s"),
            FormatStat("Additional Bomb", multishotCount, MultishotCount),
            isSticky ? "<color=green>Sticky Bomb</color>" : "",
        };
        return result;
    }

    private void ResetMultiplier()
    {
        attackAddition = 0.0f;
        attackMultiplier = 100f;
        cooldownMultiplier = 100f;
        radiusAddition = 0.0f;
        throwForceMultiplier = 100f;
        chargeTimeAddition = 0.0f;
        triggerTimeAddition = 0.0f;
        isSticky = false;
        multishotAddition = 0;
    }
}
