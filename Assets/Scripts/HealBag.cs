using UnityEngine;

public class HealBag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Character_HealthSystem>().Heal(10);
            AudioManager.Instance.PlayPickUpSfx();
            Destroy(gameObject);
        }
    }
}
