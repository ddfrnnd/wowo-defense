using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 30;

    private WaveManager waveManager;
    private bool isDead = false;

    void Start()
    {
        waveManager = FindFirstObjectByType<WaveManager>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        Debug.Log("Bandit kena damage! HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        WaveManager waveManager = FindFirstObjectByType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.EnemyDead();
        }

        // Disable moving script so they stop in their tracks
        EnemyMover mover = GetComponent<EnemyMover>();
        if (mover != null)
        {
            mover.enabled = false;
        }

        // Disable collider so they can't block bullets or the player
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Play falling and knockback animation
        StartCoroutine(PlayDeathAnimation());
    }

    private System.Collections.IEnumerator PlayDeathAnimation()
    {
        float duration = 1.0f;
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        
        // Find direction away from player for knockback direction
        Vector3 knockbackDir = Vector3.back; // default fallback
        
        // Try to find the Player GameObject
        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            // Try to find the active CharacterBehaviour in the scene
            var character = FindFirstObjectByType<InfimaGames.LowPolyShooterPack.CharacterBehaviour>();
            if (character != null) player = character.gameObject;
        }

        if (player != null)
        {
            knockbackDir = (transform.position - player.transform.position).normalized;
            knockbackDir.y = 0; // horizontal knockback
            if (knockbackDir == Vector3.zero) knockbackDir = Vector3.back;
        }

        float knockbackDist = 3.5f; // Distance they fly back
        float jumpHeight = 1.8f;    // Height of the bounce

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            // Parabolic height: h = 4 * height * percent * (1 - percent)
            float height = 4f * jumpHeight * percent * (1f - percent);

            // Horizontal translation
            Vector3 horizontalPos = startPos + knockbackDir * (percent * knockbackDist);

            // Set position (fly up and back)
            transform.position = new Vector3(horizontalPos.x, horizontalPos.y + height, horizontalPos.z);

            // Spin/tumble rotation
            transform.rotation = startRot * Quaternion.Euler(percent * 360f, percent * 180f, 0f);

            yield return null;
        }

        Destroy(gameObject);
    }
}