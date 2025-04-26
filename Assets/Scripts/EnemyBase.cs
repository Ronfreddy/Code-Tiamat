using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using static Utilities.Utility;

public abstract class EnemyBase : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth = 100.0f;

    public EnemyState currentState = EnemyState.Idle;


    public float chaseSpeed = 4f;
    protected Transform player;
    protected Rigidbody2D rb;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;

    public int baseDamage = 10;
    public bool canAttack = true;
    public event Action onDeathEvent;

    public GameObject lootPrefab;
    public WeightedPartLootTable lootTable;
    public ModularPart lootPart;
    public GameObject healBag;

    public GameObject damagePopTextPrefab;
    public GameObject canvas;
    public GameObject healthBar;
    public Image healthBarFill;

    private int frameCounter = 0;
    protected float dropLootChance = 0.5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void InitializeStat(int currentLevel)
    {
        lootPart = lootTable.Roll();
    }

    void Update()
    {
        frameCounter++;
        if(frameCounter % 15 == 0)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    Patrol();
                    break;
                case EnemyState.Chase:
                    ChasePlayer();
                    break;
                case EnemyState.Attack:
                    if (canAttack) AttackPlayer();
                    break;
            }

            DetectPlayer();
        }
        canvas.transform.rotation = Quaternion.Euler(0, 0, 0); // Reset rotation to avoid canvas rotation issues
    }

    public virtual void Patrol()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public virtual void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= detectionRange && Vector2.Distance(transform.position, player.position) > attackRange)
        {
            currentState = EnemyState.Chase;
        }
        else if (Vector2.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = EnemyState.Idle;
        }
    }

    public virtual void ChasePlayer()
    {
        LookAtPlayer();
        if (Vector2.Distance(transform.position, player.position) <= attackRange && canAttack)
        {
            Debug.Log("Player in attack range!");
            currentState = EnemyState.Attack;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            rb.linearVelocity = new Vector2((player.position.x - transform.position.x), (player.position.y - transform.position.y)).normalized * chaseSpeed;
            //rb.MovePosition(Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime));
            //transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
    }

    public virtual void AttackPlayer()
    {
        rb.linearVelocity = Vector2.zero; // Reset velocity to avoid sliding
        OnAttack();
        //if (Time.time - lastAttackTime >= attackCooldown)
        //{
        //    Debug.Log("Enemy attacks the player!");
        //    // Apply damage logic here
        //    OnAttack();
        //    lastAttackTime = Time.time;
        //}

        //if (Vector2.Distance(transform.position, player.position) > attackRange)
        //{
        //    currentState = EnemyState.Chase;
        //}
    }

    public void LookAtPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.up = direction;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        PopDamage(damage);
        UpdateHealthBar();
        AudioManager.Instance.PlayEnemyHitSfx();
        if (currentHealth <= 0.0f)
        {
            OnDeath();
        }
        GetComponent<SpriteRenderer>().DOColor(Color.white, 0.06f).OnComplete(() =>
        {
            GetComponent<SpriteRenderer>().DOColor(Color.red, 0.06f);
        });
    }

    private void OnDeath()
    {
        onDeathEvent?.Invoke();
        DropLoot();
        AudioManager.Instance.PlayEnemyDeathSfx();
        if (RandomChance(LevelManager.instance.difficultySetting.healDropChance))
        {
            Instantiate(healBag, transform.position + new Vector3 (UnityEngine.Random.value, UnityEngine.Random.value), Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public virtual void OnAttack()
    {
        
    }

    private void DropLoot()
    {
        if (lootPart != null)
        {
            GameObject loot = Instantiate(lootPrefab, transform.position, Quaternion.identity);
            loot.GetComponent<PartPickup>().InitializePart(lootPart);
        }
    
    }

    private void PopDamage(float dmg)
    {
        GameObject damagePopText = Instantiate(damagePopTextPrefab, transform.position + new Vector3(0,1,0), Quaternion.identity);
        damagePopText.GetComponent<DamagePopText>().SetDamageText(dmg);
    }

    private void UpdateHealthBar()
    {
        healthBar.GetComponent<Slider>().value = currentHealth / maxHealth;
        Color healthColor = Color.Lerp(Color.red, Color.green, currentHealth / maxHealth);
        healthBarFill.color = healthColor;
    }
}

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Death
}