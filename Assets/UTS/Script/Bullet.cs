using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 40;
    public float lifeTime = 3f;
    public GameObject bloodEffect;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Peluru kena: " + collision.gameObject.name);

        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            Debug.Log("Enemy kena damage: " + damage);

            enemy.TakeDamage(damage);

            if (bloodEffect != null)
{
    GameObject blood = Instantiate(
    bloodEffect,
    transform.position + Vector3.up * 0.5f,
    Quaternion.identity
);

Destroy(blood, 2f);

Debug.Log("Darah muncul");
}
else
{
    Debug.LogWarning("BloodEffect belum diisi di Bullet prefab");
}
        }
        else
        {
            Debug.LogWarning("Object yang kena bukan Enemy / belum ada EnemyHealth");
        }

        Destroy(gameObject);
    }
}