using UnityEngine;
using System.Collections;

/// <summary>
/// Attach this to the Donkey Kong GameObject.
/// Handles spawning barrels from DK's position, mimicking the
/// cadence of the original 1980 arcade game.
/// </summary>
public class DonkeyKongBarrelSpawner : MonoBehaviour
{
    [Header("Barrel Setup")]
    [Tooltip("Barrel prefab to spawn. Must have the 'Barrel' tag on it.")]
    [SerializeField] public GameObject barrelPrefab;

    [Tooltip("Point where barrels spawn from.")]
    [SerializeField] private Transform spawnPoint;

    [Header("Spawn Timing (seconds)")]
    [Tooltip("Base time between barrel spawns.")]
    [SerializeField] private float baseSpawnInterval = 5f;

    [Tooltip("Random variation added/subtracted from base interval.")]
    [SerializeField] private float spawnIntervalVariance = 0.5f;

    [Tooltip("Chance (0-1) that DK throws a 'double barrel' in quick succession.")]
    [Range(0f, 1f)]
    [SerializeField] private float doubleBarrelChance = 0.05f;

    [Tooltip("Delay between the two barrels in a double-barrel throw.")]
    [SerializeField] private float doubleBarrelGap = 0.5f;

    [Header("Launch Velocity")]
    [Tooltip("Initial rightward speed given to spawned barrels.")]
    [SerializeField] private float launchSpeedX = 2.2f;

    [Tooltip("Small random variance added to the launch speed for natural feel.")]
    [SerializeField] private float launchSpeedVariance = 0.25f;

    [Tooltip("Slight downward/upward nudge on spawn.")]
    [SerializeField] private float launchSpeedY = -0.25f;

    [Header("Animation (optional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string throwTriggerName = "Throw";

    [Header("Barrel Tag")]
    private const string BarrelTag = "Barrel";

    [Header("Gameplay Control")]
    [Tooltip("Toggle to start/stop DK's barrel spawning (e.g. when player dies or level ends).")]
    [SerializeField] private bool isSpawning = true;

    private Coroutine spawnRoutine;

    private void OnEnable()
    {
        if (isSpawning)
            StartSpawning();
    }

    private void OnDisable()
    {
        StopSpawning();
    }

    public void StartSpawning()
    {
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        // Small initial delay before the first barrel
        yield return new WaitForSeconds(2f);

        while (isSpawning)
        {
            SpawnBarrel();

            // Occasionally throw a second barrel shortly after (double barrel)
            if (Random.value < doubleBarrelChance)
            {
                yield return new WaitForSeconds(doubleBarrelGap);
                SpawnBarrel();
            }

            float wait = baseSpawnInterval + Random.Range(-spawnIntervalVariance, spawnIntervalVariance);
            wait = Mathf.Max(wait, 0.5f); // safety floor
            yield return new WaitForSeconds(wait);
        }
    }

    private void SpawnBarrel()
    {
        if (barrelPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("DonkeyKongBarrelSpawner: Missing barrelPrefab or spawnPoint reference.");
            return;
        }

        if (animator != null && !string.IsNullOrEmpty(throwTriggerName))
        {
            animator.SetTrigger(throwTriggerName);
        }

        GameObject barrel = Instantiate(barrelPrefab, spawnPoint.position, spawnPoint.rotation);

        if (!barrel.CompareTag(BarrelTag))
        {
            barrel.tag = BarrelTag;
        }

        ApplyLaunchVelocity(barrel);
    }

    private void ApplyLaunchVelocity(GameObject barrel)
    {
        Rigidbody2D rb2D = barrel.GetComponent<Rigidbody2D>();
        float speedX = launchSpeedX + Random.Range(-launchSpeedVariance, launchSpeedVariance);

        if (rb2D != null)
        {
            rb2D.linearVelocity = new Vector2(speedX, launchSpeedY);
            return;
        }

        Debug.LogWarning("DonkeyKongBarrelSpawner: Barrel prefab has no Rigidbody2D or Rigidbody to apply velocity to.");
    }

    // Call from a GameManager to stop DK throwing on Mario death/level restart
    public void SetSpawningActive(bool active)
    {
        isSpawning = active;
        if (active)
            StartSpawning();
        else
            StopSpawning();
    }
}