using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 30;

    private WaveManager waveManager;

    void Start()
    {
        waveManager = FindFirstObjectByType<WaveManager>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Bandit kena damage! HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
{
    WaveManager waveManager = FindFirstObjectByType<WaveManager>();

    if (waveManager != null)
    {
        waveManager.EnemyDead();
    }

    Destroy(gameObject);
}
}