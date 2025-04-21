using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon_Bow : WeaponBase
{
    public GameObject arrowPrefab;
    private bool isHoldingFire = false;
    private bool isFullReloading = false;

    [Header("Bow Stats")]
    private float intervalTimer = 0.3f;
    public float shootInterval = 0.3f;
    private float cooldownTimer = 0f;
    public float normalReloadTime = 0.2f;
    public float fullReloadPenaltyTime = 2.0f;
    private int ammoCount = 10;
    public int maxAmmoCount = 10;
    public float arrowSpeed = 800.0f;
    public float arrowScatter = 1.0f;
    public int arrowPierce = 0;

    [Header("Stat Multipliers")]
    public float attackAddition = 0.0f;
    public float attackMultiplier = 100f;
    public float cooldownMultiplier = 100f;
    public float intervalMultiplier = 100f;
    public int ammoAddition = 0;
    public float arrowSpeedAddition = 100f;
    public float arrowScatterAddition = 0.0f;
    public int arrowPierceAddition = 0;

    #region Modified Stat
    public float AttackDamage
    {
        get
        {
            return Mathf.Clamp((baseDamage + attackAddition) * Mathf.Clamp(attackMultiplier / 100, 1f, 999999f), 1f, 999999f);
        }
    }

    public float AttackInterval
    {
        get
        {
            return shootInterval / Mathf.Clamp(intervalMultiplier / 100, 0.1f, 10f);
        }
    }

    public float NormalReloadCooldown
    {
        get
        {
            return normalReloadTime / Mathf.Clamp(cooldownMultiplier / 100, 0.1f, 10f);
        }
    }

    public float FullReloadCooldown
    {
        get
        {
            return NormalReloadCooldown * MaxAmmoCount + fullReloadPenaltyTime;
        }
    }

    public int MaxAmmoCount
    {
        get
        {
            return Mathf.Clamp(maxAmmoCount + ammoAddition, 1, 999999);
        }
    }

    public float ArrowSpeed
    {
        get
        {
            return Mathf.Clamp(arrowSpeed + arrowSpeedAddition, 20f, 5000f);
        }
    }

    public float ArrowScatter
    {
        get
        {
            return Mathf.Clamp(arrowScatter + arrowScatterAddition, 0f, 180f);
        }
    }

    public int ArrowPierce
    {
        get
        {
            return Mathf.Clamp(arrowPierce + arrowPierceAddition, 0, 999999);
        }
    }
    #endregion

    private void Update()
    {
        // Update Stamina Bar
        if(isFullReloading)
        {
            UIController.Instance.UpdateStaminaBar(FullReloadCooldown - cooldownTimer, FullReloadCooldown);
        }
        else 
        {
            UIController.Instance.UpdateStaminaBar(ammoCount, MaxAmmoCount);
        }

        if (isFullReloading)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0.0f)
            {
                isFullReloading = false;
                cooldownTimer = 0.0f;
                ammoCount = MaxAmmoCount;
            }
            return;
        }

        if (isHoldingFire)
        {
            intervalTimer -= Time.deltaTime;
            if (intervalTimer <= 0.0f)
            {
                ShootArrow();
                intervalTimer = AttackInterval;
            }
        }
        else
        {
            //Casually reload
            if (ammoCount < MaxAmmoCount)
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0.0f)
                {
                    cooldownTimer = NormalReloadCooldown;
                    ammoCount += 1;
                }
            }
        }
    }

    public override void OnAttackKeyDown()
    {
        Debug.Log("Input: Attack Key Down - Bow");

        // Play the attack animation
        Animator animator = GetComponent<Animator>();
        animator.Play("Bow_Attack");
        animator.SetBool("isShooting", true);

        // Enable shoot arrow
        isHoldingFire = true;
        intervalTimer = AttackInterval;
    }

    public override void OnAttackKeyUp(Vector2 aimDirection)
    {
        Debug.Log("Input: Attack Key Up - Bow");
        DisableShoot();
    }

    //Done
    public override void UpdateStat()
    {
        ResetMultiplier();
        foreach (ModularPartSlot slot in slots)
        {
            if (slot.part != null)
            {
                attackAddition += slot.part.GetModifier("Damage Addition");
                attackMultiplier += slot.part.GetModifier("Damage Multiplier");
                cooldownMultiplier += slot.part.GetModifier("Cooldown Multiplier");
                intervalMultiplier += slot.part.GetModifier("Interval Multiplier");
                ammoAddition += (int)slot.part.GetModifier("Ammo Addition");
                arrowSpeedAddition += slot.part.GetModifier("Arrow Speed Addition");
                arrowScatterAddition += slot.part.GetModifier("Arrow Scatter Addition");
                arrowPierceAddition += (int)slot.part.GetModifier("Arrow Pierce Addition");
            }
        }

        // Clamp ammo amount
        ammoCount = Mathf.Clamp(ammoCount, 0, MaxAmmoCount);
    }

    public override List<string> GetStatDescriptions()
    {
        List<string> result = new List<string>
        {
            FormatStat("Damage", baseDamage, AttackDamage),
            FormatStat("Interval", shootInterval, AttackInterval, "s"),
            FormatStat("Reload Time", normalReloadTime, NormalReloadCooldown, "s"),
            FormatStat("Fully Reload", FullReloadCooldown, FullReloadCooldown, "s"),
            FormatStat("Max Arrow", maxAmmoCount, MaxAmmoCount),
            FormatStat("Arrow Speed", arrowSpeed, ArrowSpeed),
            FormatStat("Arrow Scatter", arrowScatter, ArrowScatter),
            FormatStat("Arrow Pierce", arrowPierce, ArrowPierce)
        };
        return result;
    }

    private void ShootArrow()
    {
        if (ammoCount <= 0)
        {
            DisableShoot();
            return;
        }

        GameObject arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
        arrow.GetComponent<Weapon_Arrow>().Initialize(AttackDamage, ArrowPierce);

        // Rotate the arrow to match the bow's rotation
        float angleOffset = Random.Range(-ArrowScatter, ArrowScatter);
        Vector3 scatterDirection = Quaternion.Euler(0, 0, angleOffset) * transform.right;
        arrow.GetComponent<Rigidbody2D>().AddForce(scatterDirection.normalized * ArrowSpeed);
        float angle = Mathf.Atan2(scatterDirection.y, scatterDirection.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        ammoCount--;

        AudioManager.Instance.PlayBowAttackSfx();
    }

    private void DisableShoot()
    {
        isHoldingFire = false;
        if(isFullReloading)
        {
            return;
        }
        Animator animator = GetComponent<Animator>();
        animator.SetBool("isShooting", false);

        // Disable shoot arrow
        intervalTimer = AttackInterval;

        // If ammo is empty, use longer cooldown
        if (ammoCount <= 0)
        {
            cooldownTimer = FullReloadCooldown;
            isFullReloading = true;
        }
        else
        {
            cooldownTimer = NormalReloadCooldown;
            isFullReloading = false;
        }
    }

    //Done
    private void ResetMultiplier()
    {
        attackAddition = 0.0f;
        attackMultiplier = 100f;
        cooldownMultiplier = 100f;
        intervalMultiplier = 100f;
        ammoAddition = 0;
        arrowSpeedAddition = 0f;
        arrowScatterAddition = 0.0f;
        arrowPierceAddition = 0;
    }
}
