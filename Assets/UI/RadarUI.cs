using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform radarRect;
    [SerializeField] private RectTransform dotsContainer;
    [SerializeField] private Image dotPrefab;
    [SerializeField] private Image playerIcon;

    [Header("V Direction Lines")]
    [SerializeField] private RectTransform lineLeft;
    [SerializeField] private RectTransform lineRight;

    [Header("Settings")]
    [SerializeField] private float radarRange = 60f;
    [SerializeField] private Color zombieDotColor = Color.red;
    [SerializeField] private string zombieTag = "Zombi";
    [SerializeField] private float vAngle = 30f; // çizgiler arasındaki açı
    [SerializeField] private float vLength = 40f; // çizgi uzunluğu

    private readonly List<Image> dotPool = new List<Image>();
    private readonly List<Transform> targets = new List<Transform>();

    private void Awake()
    {
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null) player = found.transform;
        }
    }

    private void Update()
    {
        CollectTargets();
        EnsurePoolSize(targets.Count);
        RenderDots();
        UpdateVDirection();
    }

    private void CollectTargets()
    {
        targets.Clear();
        var zombies = GameObject.FindGameObjectsWithTag(zombieTag);
        for (int i = 0; i < zombies.Length; i++)
        {
            if (zombies[i] != null) targets.Add(zombies[i].transform);
        }
    }

    private void EnsurePoolSize(int count)
    {
        for (int i = dotPool.Count; i < count; i++)
        {
            var dot = Instantiate(dotPrefab, dotsContainer);
            dot.color = zombieDotColor;
            dot.gameObject.SetActive(false);
            dotPool.Add(dot);
        }
        for (int i = 0; i < dotPool.Count; i++)
        {
            dotPool[i].gameObject.SetActive(i < count);
        }
    }

    private void RenderDots()
    {
        if (player == null || radarRect == null) return;

        Vector2 half = radarRect.rect.size * 0.5f;
        float radius = Mathf.Min(half.x, half.y);

        for (int i = 0; i < targets.Count; i++)
        {
            Transform t = targets[i];
            if (t == null) { dotPool[i].gameObject.SetActive(false); continue; }

            Vector3 delta = t.position - player.position;
            Vector2 planar = new Vector2(delta.x, delta.z);
            float dist = planar.magnitude;

            if (dist > radarRange)
            {
                dotPool[i].gameObject.SetActive(false);
                continue;
            }

            Vector2 norm = planar / radarRange;
            Vector2 uiPos = norm * (radius - 6f);

            dotPool[i].rectTransform.anchoredPosition = uiPos;
            dotPool[i].gameObject.SetActive(true);
        }
    }

    private void UpdateVDirection()
    {
        if (player == null) return;

        float yaw = player.eulerAngles.y;

        // Player yönüne göre çizgileri V şeklinde döndür
        SetVLine(lineLeft, -vAngle, yaw);
        SetVLine(lineRight, vAngle, yaw);
    }

    private void SetVLine(RectTransform line, float angleOffset, float yaw)
    {
        if (line == null) return;

        float totalAngle = yaw + angleOffset;
        line.localRotation = Quaternion.Euler(0, 0, -totalAngle);

        // Boyut ve pozisyon
        line.sizeDelta = new Vector2(2f, vLength);
        line.anchoredPosition = Vector2.zero;
    }
}
