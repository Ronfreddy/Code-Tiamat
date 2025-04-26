using UnityEngine;
using UnityEngine.UI;

public class Detonating_Bomb : MonoBehaviour
{
    private bool isSticking = false;
    private float explosionTimer;

    // Inherited Stats
    private float explosionDamage = 50f;
    private float explosionRadius = 5f;
    private bool canStick = false;
    private float explosionDelay = 3f;

    public AudioSource tickSfx;

    [Header("Radius")]
    public GameObject UICanvas;

    [Header("Explosion Timer")]
    public GameObject explosionTimerUI;
    public const float RadiusUIRatio = 0.93f;

    [Header("Vfx")]
    public GameObject explosionVFXPrefab;

    public void InitializeBomb(float damage, float radius, bool isSticky, float triggerTime, bool isFirstBomb)
    {
        explosionDamage = damage;
        explosionRadius = radius;
        canStick = isSticky;
        explosionDelay = triggerTime;
        explosionTimer = explosionDelay;

        if (!isFirstBomb) tickSfx.enabled = false;

        if (explosionDelay <= 0.5f)
        {
            tickSfx.pitch = 1.5f;
            tickSfx.minDistance = explosionRadius * 0.5f;
            tickSfx.maxDistance = explosionRadius * 2;
        }
        else
        {
            tickSfx.pitch = 0.7f;
            tickSfx.minDistance = explosionRadius * 0.5f;
            tickSfx.maxDistance = explosionRadius * 0.7f;
        }

        // Crazy ahh
        UICanvas.transform.localScale = new Vector3 (1 / RadiusUIRatio * explosionRadius, 1 / RadiusUIRatio * explosionRadius, 1 / RadiusUIRatio * explosionRadius);
    }

    private void Update()
    {
        Vector2 currentPosInCanvas = Camera.main.WorldToScreenPoint(transform.position); // Get the mouse position in viewport coordinates
        UICanvas.GetComponent<RectTransform>().anchoredPosition = currentPosInCanvas;

        explosionTimerUI.GetComponent<Slider>().value = (explosionDelay - explosionTimer) / explosionDelay;


        explosionTimer -= Time.deltaTime;
        if (explosionTimer <= 0)
        {
            Explode();
        }

        if (isSticking) 
            transform.localPosition = Vector2.zero;
    }

    private void Explode()
    {
        // Find all enemies within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            // Apply damage to enemies
            if (collider.CompareTag("Enemy"))
            {
                EnemyBase enemy = collider.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(explosionDamage); // Example damage value
                }
            }
        }

        // Create explosion effect
        GameObject explosionVFX = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
        explosionVFX.transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);
        explosionVFX.GetComponent<ParticleSystem>().Play();
        AudioManager.Instance.PlayBombExplodeSfx();

        // Destroy the bomb object after explosion
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && canStick && !isSticking)
        {
            bool hasAnyBombSticked = collision.transform.Find("Detonating Bomb") != null;
            if (!hasAnyBombSticked)
            {
                transform.parent = collision.transform;
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                isSticking = true;
            }
        }
    }
}
