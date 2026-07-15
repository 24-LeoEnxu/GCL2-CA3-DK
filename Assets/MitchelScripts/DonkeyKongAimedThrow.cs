using UnityEngine;
using System.Collections;

public class DonkeyKongAimedThrow : MonoBehaviour
{
    [SerializeField] private GameObject thrownBarrelPrefab; // the barrel he throws straight at mario
    [SerializeField] private Transform throwOrigin; // where it spawns from

    [SerializeField] private float startDelay = 30f; // waits this long before he starts throwing
    [SerializeField] private float throwInterval = 5f; // time between throws
    [SerializeField] private float throwIntervalVariance = 1f; // adds a bit of randomness

    [SerializeField] private float projectileSpeed = 6f; // how fast the barrel flies

    [SerializeField] private string playerTag = "Player"; // tag we search for to find mario

    [SerializeField] private Animator animator; // optional, for the throw anim
    [SerializeField] private string throwTriggerName = "ThrowAimed"; // anim trigger name

    [Header("Projectile Speed")]
    [SerializeField] private float projectileSpeed = 1f;

    [Header("Targeting")]
    [Tooltip("Player tag to search for and aim at.")]
    [SerializeField] private string playerTag = "Player";

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string throwTriggerName = "ThrowAimed";

    private Coroutine throwRoutine; // so we can stop it later if needed

    private void OnEnable()
    {
        throwRoutine = StartCoroutine(AimedThrowLoop()); // starts the loop when dk is enabled
    }

    private void OnDisable()
    {
        if (throwRoutine != null)
        {
            StopCoroutine(throwRoutine); // stop the coroutine so it doesnt run in bg
            throwRoutine = null; // reset just in case
        }
    }

    private IEnumerator AimedThrowLoop()
    {
        yield return new WaitForSeconds(startDelay); // wait the initial 30s before he rages

        while (true) // keeps throwing forever once it starts, might wanna add a stop condition later
        {
            ThrowAtPlayer(); // do the throw

            float wait = throwInterval + Random.Range(-throwIntervalVariance, throwIntervalVariance); // randomize the wait a bit
            wait = Mathf.Max(wait, 0.5f); // just in case the random makes it negative or too small
            yield return new WaitForSeconds(wait); // wait before next throw
        }
    }

    private void ThrowAtPlayer()
    {
        if (thrownBarrelPrefab == null || throwOrigin == null)
        {
            Debug.LogWarning("DonkeyKongAimedThrow: Missing thrownBarrelPrefab or throwOrigin reference."); // if forgot to assign something in inspector
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag); // find mario in the scene
        if (playerObj == null)
        {
            Debug.LogWarning("DonkeyKongAimedThrow: No GameObject found with tag '" + playerTag + "'."); // no player tag found
            return;
        }

        if (animator != null && !string.IsNullOrEmpty(throwTriggerName))
        {
            animator.SetTrigger(throwTriggerName); // play throw anim if we have one
        }

        Vector2 direction = ((Vector2)playerObj.transform.position - (Vector2)throwOrigin.position).normalized; // math to get direction towards mario

        GameObject projectile = Instantiate(thrownBarrelPrefab, throwOrigin.position, Quaternion.identity); // spawn the barrel

        ThrownBarrel tb = projectile.GetComponent<ThrownBarrel>(); // grab the script off it
        if (tb != null)
        {
            tb.Launch(direction, projectileSpeed); // send it flying
        }
        else
        {
            Debug.LogWarning("DonkeyKongAimedThrow: thrownBarrelPrefab has no ThrownBarrel component."); // if forgot to add the script to the prefab
        }
    }
}