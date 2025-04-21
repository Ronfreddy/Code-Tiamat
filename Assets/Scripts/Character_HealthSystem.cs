using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class Character_HealthSystem : MonoBehaviour
{
    public float health = 100f;
    public bool isInvincible = false;
    private float invincibilityTimer = 0f;
    public float invincibilityDuration = 0.5f;

    public CircleCollider2D playerCollider;
    public CinemachineImpulseSource impulseSource;

    private void Update()
    {
        if(isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            return;
        }
        health -= damage;
        UIController.Instance.UpdateHealthBar(health);
        AudioManager.Instance.PlayPlayerHitSfx();
        impulseSource.GenerateImpulse();
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
        GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            GetComponent<SpriteRenderer>().DOColor(Color.green, 0.1f);
        });
        if(health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > 100f)
        {
            health = 100f;
        }
        UIController.Instance.UpdateHealthBar(health);
    }

    private void Die()
    {
        playerCollider.enabled = false;
        PlayerInputController.Instance.canInput = false;
        GameManager.instance.OnGameOver();
    }

    public void ResetStatus()
    {
        health = 100f;
        playerCollider.enabled = true;
    }
}
