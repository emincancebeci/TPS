using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private int zombieCount = 20;
    [SerializeField] private Vector3 areaCenter;
    [SerializeField] private Vector3 areaSize = new Vector3(100f, 10f, 100f);
    [SerializeField] private bool trySnapToNavMesh = true;
    [SerializeField] private float navMeshMaxSampleDistance = 5f;

    [Header("Round Settings")]
    [SerializeField] private float roundDurationSeconds = 180f; // 3 minutes default

    [Header("Runtime State (Read-Only)")]
    [SerializeField] private int aliveZombies;
    [SerializeField] private float timeLeft;

    private readonly List<GameObject> spawned = new List<GameObject>();
    private readonly HashSet<GameObject> notifiedDeath = new HashSet<GameObject>();
    private bool gameOverTriggered;

    // Public read-only accessors for UI
    public int AliveZombies => aliveZombies;
    public float TimeLeft => timeLeft;

    private void Reset()
    {
        areaCenter = transform.position;
    }

    private void Start()
    {
        // Use spawner transform as default center if none provided
        if (areaCenter == Vector3.zero)
        {
            areaCenter = transform.position;
        }

        timeLeft = roundDurationSeconds;
        SpawnZombiesOnce();
    }

    private void Update()
    {
        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0f) timeLeft = 0f;
        }
        if (!gameOverTriggered && timeLeft == 0f)
        {
            gameOverTriggered = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
            return;
        }

        // Cleanup destroyed references to avoid leaks; aliveZombies is driven by notifications
        for (int i = spawned.Count - 1; i >= 0; i--)
        {
            if (spawned[i] == null)
            {
                spawned.RemoveAt(i);
            }
        }
    }

    private void SpawnZombiesOnce()
    {
        if (zombiePrefab == null)
        {
            Debug.LogWarning("ZombieSpawner: zombiePrefab is not assigned.");
            return;
        }

        for (int i = 0; i < zombieCount; i++)
        {
            Vector3 randomPos = GetRandomPointInArea();

            if (trySnapToNavMesh && NavMeshReady())
            {
                if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, navMeshMaxSampleDistance, NavMesh.AllAreas))
                {
                    randomPos = hit.position;
                }
            }

            GameObject z = Instantiate(zombiePrefab, randomPos, Quaternion.identity);
            // Ensure a notifier exists so animation events can call back into the spawner
            var notifier = z.GetComponent<ZombieDeathNotifier>();
            if (notifier == null)
            {
                notifier = z.AddComponent<ZombieDeathNotifier>();
            }
            notifier.SetSpawner(this);
            spawned.Add(z);
        }

        aliveZombies = spawned.Count;
    }

    // Called by ZombieDeathNotifier when death animation starts
    public void NotifyDeathStart(GameObject zombie)
    {
        if (zombie == null) return;
        if (notifiedDeath.Contains(zombie)) return;

        notifiedDeath.Add(zombie);
        aliveZombies = Mathf.Max(0, aliveZombies - 1);
        if (!gameOverTriggered && aliveZombies == 0)
        {
            gameOverTriggered = true;
            TryTriggerGameOver();
        }
    }

    private void TryTriggerGameOver()
    {
        // Load scene index 0 (Main Menu) as Game Over.
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private Vector3 GetRandomPointInArea()
    {
        Vector3 half = areaSize * 0.5f;
        float x = Random.Range(-half.x, half.x);
        float y = Random.Range(-half.y, half.y);
        float z = Random.Range(-half.z, half.z);
        return areaCenter + new Vector3(x, y, z);
    }

    private bool NavMeshReady()
    {
        // Basic check to avoid errors if NavMesh components are missing
        return NavMeshSurfaceExists() || NavMeshHasData();
    }

    private bool NavMeshHasData()
    {
        // True in baked scenes
        return NavMesh.CalculateTriangulation().vertices != null && NavMesh.CalculateTriangulation().vertices.Length > 0;
    }

    private bool NavMeshSurfaceExists()
    {
        // If using NavMeshComponents package, presence of any surface implies intent to use navmesh
        var surfaces = FindObjectsByType<Component>(FindObjectsSortMode.None);
        for (int i = 0; i < surfaces.Length; i++)
        {
            if (surfaces[i] == null) continue;
            if (surfaces[i].GetType().Name == "NavMeshSurface") return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
        Vector3 center = areaCenter == Vector3.zero ? transform.position : areaCenter;
        Gizmos.DrawCube(center, areaSize);
        Gizmos.color = new Color(0f, 0.7f, 0f, 1f);
        Gizmos.DrawWireCube(center, areaSize);
    }
}
