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
    public int baseEnemyCount = 3;
    public float timeBetweenWaves = 5f;
    public float spawnDelay = 0.8f;

    [Header("Escalate")]
    public int enemyIncreasePerWave = 2;
    public float speedIncreasePerWave = 0.4f;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;

    void Start()
    {
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

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            enemiesAlive++;

            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false;
    }

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

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation,
            enemyParent
        );

        EnemyMover mover = enemy.GetComponent<EnemyMover>();

        if (mover != null)
        {
            mover.speed += (currentWave - 1) * speedIncreasePerWave;
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

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Level1")
        {
            SceneManager.LoadScene("Level2");
        }
        else if (currentScene == "Level2")
        {
            Debug.Log("YOU WIN!");
        }
    }
}