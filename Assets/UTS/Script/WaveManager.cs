using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public Transform enemyParent;

    public int totalWaves = 3;
    public int baseEnemyCount = 8;
    public float timeBetweenWaves = 5f;
    public float spawnDelay = 0.8f;

    [Header("Escalate")]
    public int enemyIncreasePerWave = 4;
    public float speedIncreasePerWave = 0.4f;

    [Header("UI")]
    public GameUIManager gameUI;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;

    void Start()
    {
        // Find GameUIManager if not assigned
        if (gameUI == null)
            gameUI = FindAnyObjectByType<GameUIManager>();

        // Update initial wave display
        if (gameUI != null)
            gameUI.UpdateWaveText(0, totalWaves);

        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        if (currentWave >= totalWaves)
        {
            LevelClear();
            yield break;
        }

        currentWave++;
        isSpawning = true;

        int enemyCount = baseEnemyCount + ((currentWave - 1) * enemyIncreasePerWave);

        Debug.Log("WAVE " + currentWave + " DIMULAI! Enemy: " + enemyCount);

        // Update wave UI
        if (gameUI != null)
            gameUI.UpdateWaveText(currentWave, totalWaves);

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            enemiesAlive++;

            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false;
    }

    private int lastSpawnIndex = -1;

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab belum diisi!");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn Points belum diisi!");
            return;
        }

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        // Jangan gunakan titik spawn yang sama berturut-turut agar musuh menyebar
        if (spawnIndex == lastSpawnIndex && spawnPoints.Length > 1)
        {
            spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
        }
        lastSpawnIndex = spawnIndex;

        Transform spawnPoint = spawnPoints[spawnIndex];

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation,
            enemyParent
        );

        EnemyMover mover = enemy.GetComponent<EnemyMover>();
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level2")
        {
            // Level 2: Big, slow enemies with high health
            enemy.transform.localScale = UnityEngine.Vector3.one * 2.6f;

            if (mover != null)
            {
                float randomSpeedVariance = UnityEngine.Random.Range(-0.1f, 0.3f);
                mover.speed = 0.5f + ((currentWave - 1) * 0.1f) + randomSpeedVariance;
                mover.templeDamage = 30; // Boss/heavy enemy deals more damage to temple
            }

            if (health != null)
            {
                health.health = 200 + ((currentWave - 1) * 50); // Increased health for Level 2
            }
        }
        else
        {
            // Level 1: Default scale and speed, slightly larger scale
            enemy.transform.localScale = UnityEngine.Vector3.one * 1.5f;

            if (mover != null)
            {
                float randomSpeedVariance = UnityEngine.Random.Range(-0.5f, 1.5f);
                mover.speed += ((currentWave - 1) * speedIncreasePerWave) + randomSpeedVariance;
            }
        }
    }

    public void EnemyDead()
    {
        enemiesAlive--;

        Debug.Log("Enemy sisa: " + enemiesAlive);

        if (enemiesAlive <= 0 && !isSpawning)
        {
            Debug.Log("WAVE CLEAR!");
            StartCoroutine(NextWaveDelay());
        }
    }

    IEnumerator NextWaveDelay()
    {
        Debug.Log("Wave berikutnya dalam " + timeBetweenWaves + " detik...");
        yield return new WaitForSeconds(timeBetweenWaves);

        StartCoroutine(StartNextWave());
    }

    void LevelClear()
    {
        Debug.Log("LEVEL CLEAR!");

        // Show victory panel instead of directly loading next scene
        if (gameUI != null)
        {
            gameUI.ShowVictory();
        }
        else
        {
            // Fallback: direct scene load
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == "Level1")
                SceneManager.LoadScene("Level2");
            else if (currentScene == "Level2")
                Debug.Log("YOU WIN!");
        }
    }
}