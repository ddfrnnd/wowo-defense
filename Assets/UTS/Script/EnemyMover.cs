using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1.5f;

    [Header("Attack")]
    public int templeDamage = 10;
    public float attackDistance = 5f;

    [Header("Target")]
    public Transform targetTemple;

    private bool hasAttacked = false;

    void Start()
    {
        if (targetTemple == null)
        {
            GameObject templeObj = GameObject.FindGameObjectWithTag("Kuil");

            if (templeObj != null)
                targetTemple = templeObj.transform;
            else
                Debug.LogError("Object Kuil belum dikasih Tag Temple!");
        }
    }

    void Update()
    {
        if (targetTemple == null || hasAttacked) return;

        Vector3 direction = targetTemple.position - transform.position;
        direction.y = 0;

        float distance = direction.magnitude;

        if (distance <= attackDistance)
        {
            AttackTemple();
            return;
        }

        transform.position += direction.normalized * speed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);

            // Kalau model enemy jalan mundur, aktifkan baris ini:
            // transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0);
        }
    }

    void AttackTemple()
    {
        hasAttacked = true;

        TempleHealth temple = targetTemple.GetComponent<TempleHealth>();

        if (temple != null)
        {
            temple.TakeDamage(templeDamage);
            Debug.Log("Enemy menyerang kuil!");
        }
        else
        {
            Debug.LogError("TempleHealth tidak ditemukan di object Kuil!");
        }

        WaveManager wave = FindFirstObjectByType<WaveManager>();

        if (wave != null)
        {
            wave.EnemyDead();
        }

        Destroy(gameObject);
    }
}