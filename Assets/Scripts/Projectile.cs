using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10.0f;
    public float lifeTime = 3.0f;

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    public void Initialize(int dmg)
    {
        damage = dmg;
        Invoke("DestroyProjectile", lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Character_HealthSystem>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
