using UnityEngine;

public class Weapon_Arrow : MonoBehaviour
{
    private float lifeTime = 3.0f;

    private float arrowDamage = 0f;
    private int arrowPierce = 0;

    private void OnEnable()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(float damage, int pierce)
    {
        arrowDamage = damage;
        arrowPierce = pierce;
        Debug.Log("Arrow Damage: " + arrowDamage);
        Debug.Log("Arrow Pierce: " + arrowPierce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBase>().TakeDamage(arrowDamage);
            arrowPierce--;
            if (arrowPierce < 0)
            {
                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
