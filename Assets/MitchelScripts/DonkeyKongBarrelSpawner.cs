using UnityEngine;
using System.Collections;

public class DonkeyKongBarrelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject barrelPrefab; // normal rolling barrel, needs Barrel tag
    [SerializeField] private Transform spawnPoint; // where barrels spawn from

    [SerializeField] private float baseSpawnInterval = 4.5f; // avg time between spawns, matches arcade roughly
    [SerializeField] private float spawnIntervalVariance = 1.0f; // randomness so its not too predictable

    [Range(0f, 1f)]
    [SerializeField] private float doubleBarrelChance = 0.15f; // chance he throws 2 in a row like the og game

    [SerializeField] private float doubleBarrelGap = 0.4f; // time between the double barrel throws

    [SerializeField] private float launchSpeedX = 2.5f; // pushes barrel right when it spawns
    [SerializeField] private float launchSpeedVariance = 0.3f; // slight random variation so barrels dont look identical
    [SerializeField] private float launchSpeedY = 0f; // usually 0, can tweak for a lil drop arc

    [SerializeField] private Animator animator; // optional throw anim
    [SerializeField] private string throwTriggerName = "Throw"; // anim trigger name

    private const string BarrelTag = "Barrel"; // just so i dont typo the tag string everywhere

    [SerializeField] private bool isSpawning = true; // toggle for turning spawning on/off from inspector or code

    private Coroutine spawnRoutine; // reference so we can stop it

    private void OnEnable()
    {
        if (isSpawning)
            StartSpawning(); // auto starts if the bool is true
    }

    private void OnDisable()
    {
        StopSpawning(); // cleanup when disabled so it doesnt keep running
    }

    public void StartSpawning()
    {
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnLoop()); // only start if not already running
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine); // stop it
            spawnRoutine = null; // reset the ref
        }
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(2f); // small delay before first barrel, like the intro

        while (isSpawning) // keeps going as long as this is true
        {
            SpawnBarrel(); // spawn one

            if (Random.value < doubleBarrelChance) // sometimes throw a second one right after
            {
                yield return new WaitForSeconds(doubleBarrelGap);
                SpawnBarrel();
            }

            float wait = baseSpawnInterval + Random.Range(-spawnIntervalVariance, spawnIntervalVariance); // randomize next wait time
            wait = Mathf.Max(wait, 0.5f); // stop it going too low or negative
            yield return new WaitForSeconds(wait);
        }
    }

    private void SpawnBarrel()
    {
        if (barrelPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("DonkeyKongBarrelSpawner: Missing barrelPrefab or spawnPoint reference."); // forgot to drag something in inspector
            return;
        }

        if (animator != null && !string.IsNullOrEmpty(throwTriggerName))
        {
            animator.SetTrigger(throwTriggerName); // play the throw anim
        }

        GameObject barrel = Instantiate(barrelPrefab, spawnPoint.position, spawnPoint.rotation); // actually spawn it

        if (!barrel.CompareTag(BarrelTag))
        {
            barrel.tag = BarrelTag; // just in case the prefab wasnt tagged right, fix it here
        }

        ApplyLaunchVelocity(barrel); // give it a push to the right
    }

    private void ApplyLaunchVelocity(GameObject barrel)
    {
        Rigidbody2D rb2D = barrel.GetComponent<Rigidbody2D>(); // try get 2d rigidbody first since this is 2d game
        float speedX = launchSpeedX + Random.Range(-launchSpeedVariance, launchSpeedVariance); // randomize speed a lil

        if (rb2D != null)
        {
            rb2D.linearVelocity = new Vector2(speedX, launchSpeedY); // apply velocity
            return;
        }


        Debug.LogWarning("DonkeyKongBarrelSpawner: Barrel prefab has no Rigidbody2D to apply velocity to."); // no rigidbody found
    }

    public void SetSpawningActive(bool active)
    {
        isSpawning = active; // update the flag
        if (active)
            StartSpawning(); // turn it on
        else
            StopSpawning(); // turn it off
    }
}