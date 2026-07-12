using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Moves enemy toward the Kuil (temple) using NavMeshAgent for proper pathfinding.
/// Enemies will navigate around walls, buildings, and obstacles instead of clipping through them.
/// Requires: NavMeshAgent component + baked NavMesh in the scene.
/// </summary>
public class EnemyMover : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1.5f;

    [Header("Attack")]
    public int templeDamage = 5;
    public float attackDistance = 5f;

    [Header("Target")]
    public Transform targetTemple;

    private bool hasAttacked = false;
    private NavMeshAgent agent;

    void Start()
    {
        // Find the Kuil/Temple target
        if (targetTemple == null)
        {
            GameObject templeObj = GameObject.FindGameObjectWithTag("Kuil");

            if (templeObj != null)
                targetTemple = templeObj.transform;
            else
                Debug.LogError("Object Kuil belum dikasih Tag 'Kuil'!");
        }

        // Setup NavMeshAgent for proper pathfinding around walls
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = true;
            agent.speed = speed;
            agent.stoppingDistance = attackDistance;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            agent.autoRepath = true;

            // Ensure agent starts on NavMesh by warping to nearest valid position
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 10.0f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }

            if (targetTemple != null)
                agent.SetDestination(targetTemple.position);
        }
    }

    void Update()
    {
        if (targetTemple == null || hasAttacked) return;

        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            // NavMeshAgent handles movement - respects walls and obstacles
            agent.speed = speed;

            // Update destination periodically in case temple moves
            if (Time.frameCount % 60 == 0)
                agent.SetDestination(targetTemple.position);

            // Check if close enough to attack
            if (!agent.pathPending && agent.remainingDistance <= attackDistance)
            {
                AttackTemple();
            }

            // Face movement direction
            if (agent.velocity.sqrMagnitude > 0.01f)
            {
                Vector3 faceDir = agent.velocity;
                faceDir.y = 0;
                if (faceDir.sqrMagnitude > 0.01f)
                    transform.rotation = Quaternion.LookRotation(faceDir);
            }
        }
        else
        {
            // Fallback: direct movement if NavMeshAgent is unavailable
            FallbackDirectMovement();
        }
    }

    /// <summary>
    /// Fallback movement when NavMeshAgent is not available.
    /// Uses Rigidbody if present, otherwise direct transform movement.
    /// </summary>
    void FallbackDirectMovement()
    {
        Vector3 targetPos = targetTemple.position;
        Vector3 currentPos = transform.position;

        Vector3 direction = targetPos - currentPos;
        direction.y = 0;

        float distance = direction.magnitude;

        if (distance <= attackDistance)
        {
            AttackTemple();
            return;
        }

        Vector3 movement = direction.normalized * speed * Time.deltaTime;
        Vector3 newPos = currentPos + movement;
        newPos.y = targetPos.y;

        transform.position = newPos;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void AttackTemple()
    {
        hasAttacked = true;

        // Stop NavMeshAgent
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

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