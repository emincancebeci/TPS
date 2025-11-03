using UnityEngine;

public class ZombieDeathNotifier : MonoBehaviour
{
    private ZombieSpawner spawner;

    public void SetSpawner(ZombieSpawner s)
    {
        spawner = s;
    }

    // Call this from the death animation (Animation Event at the start of death)
    public void OnDeathAnimationStart()
    {
        if (spawner == null)
        {
            spawner = FindObjectOfType<ZombieSpawner>();
        }
        if (spawner != null)
        {
            spawner.NotifyDeathStart(gameObject);
        }
    }
}
