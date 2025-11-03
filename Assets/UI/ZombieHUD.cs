using TMPro;
using UnityEngine;

public class ZombieHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ZombieSpawner spawner;
    [SerializeField] private TMP_Text zombiesText;
    [SerializeField] private TMP_Text timeText;

    private void Awake()
    {
        if (spawner == null)
        {
            spawner = FindObjectOfType<ZombieSpawner>();
        }
    }

    private void Update()
    {
        if (spawner == null) return;

        if (zombiesText != null)
        {
            zombiesText.text = $"Kalan Zombi: {spawner.AliveZombies}";
        }

        if (timeText != null)
        {
            timeText.text = FormatTime(spawner.TimeLeft);
        }
    }

    private string FormatTime(float seconds)
    {
        if (seconds < 0f) seconds = 0f;
        int m = Mathf.FloorToInt(seconds / 60f);
        int s = Mathf.FloorToInt(seconds % 60f);
        return $"Süre: {m:00}:{s:00}";
    }
}
