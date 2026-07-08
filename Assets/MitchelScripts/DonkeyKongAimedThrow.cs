using UnityEngine;
using System.Collections;

/// <summary>
/// Attach this to the Donkey Kong GameObject alongside DonkeyKongBarrelSpawner.
/// After a delay, DK starts periodically hurling ThrownBarrel projectiles
/// directly at Mario (Player) in a straight line, ignoring platforms.
/// </summary>
public class DonkeyKongAimedThrow : MonoBehaviour
{
    [Header("Thrown Barrel Setup")]
    [Tooltip("Projectile prefab. Should have ThrownBarrel.cs, a CircleCollider2D (Is Trigger), no Rigidbody2D needed.")]
    [SerializeField] public GameObject thrownBarrelPrefab;

    [Tooltip("Point where thrown barrels originate from (DK's hands/arms).")]
    [SerializeField] private Transform throwOrigin;

    [Header("Timing")]
    [Tooltip("Delay in seconds before DK starts this attack pattern.")]
    [SerializeField] private float startDelay = 30f;

    [Tooltip("Base time between aimed throws once active.")]
    [SerializeField] private float throwInterval = 7f;

    [Tooltip("Random variance added/subtracted from throwInterval.")]
    [SerializeField] private float throwIntervalVariance = 2f;

    [Header("Projectile Speed")]
    [SerializeField] private float projectileSpeed = 1f;

    [Header("Targeting")]
    [Tooltip("Player tag to search for and aim at.")]
    [SerializeField] private string playerTag = "Player";

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string throwTriggerName = "ThrowAimed";

    private Coroutine throwRoutine;

    private void OnEnable()
    {
        throwRoutine = StartCoroutine(AimedThrowLoop());
    }

    private void OnDisable()
    {
        if (throwRoutine != null)
        {
            StopCoroutine(throwRoutine);
            throwRoutine = null;
        }
    }

    private IEnumerator AimedThrowLoop()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            ThrowAtPlayer();

            float wait = throwInterval + Random.Range(-throwIntervalVariance, throwIntervalVariance);
            wait = Mathf.Max(wait, 0.5f);
            yield return new WaitForSeconds(wait);
        }
    }

    private void ThrowAtPlayer()
    {
        if (thrownBarrelPrefab == null || throwOrigin == null)
        {
            Debug.LogWarning("DonkeyKongAimedThrow: Missing thrownBarrelPrefab or throwOrigin reference.");
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj == null)
        {
            Debug.LogWarning("DonkeyKongAimedThrow: No GameObject found with tag '" + playerTag + "'.");
            return;
        }

        if (animator != null && !string.IsNullOrEmpty(throwTriggerName))
        {
            animator.SetTrigger(throwTriggerName);
        }

        Vector2 direction = ((Vector2)playerObj.transform.position - (Vector2)throwOrigin.position).normalized;

        GameObject projectile = Instantiate(thrownBarrelPrefab, throwOrigin.position, Quaternion.identity);

        ThrownBarrel tb = projectile.GetComponent<ThrownBarrel>();
        if (tb != null)
        {
            tb.Launch(direction, projectileSpeed);
        }
        else
        {
            Debug.LogWarning("DonkeyKongAimedThrow: thrownBarrelPrefab has no ThrownBarrel component.");
        }
    }
}