using UnityEngine;

public class Tutorial_EnemyEasterEgg : MonoBehaviour
{
    public EnemyBase enemyRef;
    public GameObject enemyEasterEgg;


    private void Update()
    {
        if (enemyRef != null)
        {
            if (enemyRef.currentHealth <= 1999800)
            {
                enemyEasterEgg.SetActive(true);
            }
        }
    }
}
