using UnityEngine;

public class Weapon_Chains : WeaponBase
{
    public GameObject singleChainPrefab;
    public float chargeTime;

    [Header("Stat Multipliers")]
    public float attackMultiplier = 1.0f;
    public float cooldownMultiplier = 1.0f;

    public override void OnAttackKeyDown()
    {
        base.OnAttackKeyDown();

        Debug.Log("Input: Attack Key Down - Chains");

        // Play the attack animation
        Animator animator = GetComponent<Animator>();
        animator.Play("Chain_Charge");
        animator.SetBool("isCharging", true);

        // Start charging timer
        chargeTime = Time.time;
    }

    public override void OnAttackKeyUp(Vector2 aimDirection)
    {
        base.OnAttackKeyUp(aimDirection);

        Debug.Log("Input: Attack Key Up - Chains");

        // Play the attack animation (not a charging)
        Animator animator = GetComponent<Animator>();
        animator.Play("Chain_Attack");
        animator.SetBool("isCharging", false);

        // Calculate the hold time
        float holdTime = Time.time - chargeTime;
        Debug.Log("Hold Duration: " + holdTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBase>().TakeDamage(10.0f);
        }
    }

    public override void UpdateStat()
    {
        ResetMultiplier();
        foreach(ModularPartSlot slot in slots)
        {
            if (slot.part != null)
            {
                attackMultiplier *= slot.part.GetModifier("attackModifier");
                cooldownMultiplier *= slot.part.GetModifier("cooldownModifier");
            }
        }

        // Clamp the multipliers
        attackMultiplier = Mathf.Clamp(attackMultiplier, 0.1f, 10.0f);
        cooldownMultiplier = Mathf.Clamp(cooldownMultiplier, 0.1f, 10.0f);
    }

    private void ResetMultiplier()
    {
        attackMultiplier = 1.0f;
        cooldownMultiplier = 1.0f;
    }
}
