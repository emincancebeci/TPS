using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
	[SerializeField] private int ammoAmount = 30;
	[SerializeField] private bool autoReloadIfEmpty = true;
	[SerializeField] private AudioClip pickupSfx;
    [Header("Respawn")]
    [SerializeField] private bool respawnEnabled = true;
    [SerializeField] private float respawnDelaySeconds = 30f;

    private Collider triggerCollider;
    private Renderer[] renderers;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>(true);
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

	private void OnTriggerEnter(Collider other)
	{
		// Try to find AtesSistemi on the object or its parents
		var shooter = other.GetComponentInParent<AtesSistemi>();
		if (shooter == null) return;

		shooter.AddAmmo(ammoAmount, autoReloadIfEmpty);

		if (pickupSfx != null)
		{
			AudioSource.PlayClipAtPoint(pickupSfx, transform.position, 0.8f);
		}

		if (respawnEnabled)
		{
			Hide();
			StartCoroutine(RespawnAfterDelay());
		}
		else
		{
			Destroy(gameObject);
		}
	}

    private void Hide()
    {
        if (triggerCollider != null) triggerCollider.enabled = false;
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++) renderers[i].enabled = false;
        }
    }

    private void Show()
    {
        if (triggerCollider != null) triggerCollider.enabled = true;
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++) renderers[i].enabled = true;
        }
    }

    private System.Collections.IEnumerator RespawnAfterDelay()
    {
        float t = respawnDelaySeconds;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        Show();
    }
}
