using System.Collections;
using System.Threading;
using UnityEngine;

public class DonkeyKongBarrelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject barrelPrefab; // normal rolling barrel, needs Barrel tag
    [SerializeField] private GameObject explosivePrefab; // normal rolling barrel, needs Barrel tag
    [SerializeField] private Transform spawnPoint; // where barrels spawn from

    [SerializeField] private float baseSpawnInterval = 4.5f; // avg time between spawns, matches arcade roughly
    [SerializeField] private float spawnIntervalVariance = 1.0f; // randomness so its not too predictable

    [Range(0f, 1f)]
    [SerializeField] private float doubleBarrelChance = 0.15f; // chance he throws 2 in a row like the og game
    [SerializeField] private float explodingBarrelChance = 0.1f; //chance he throws an exploding barrel 


    [SerializeField] private float doubleBarrelGap = 0.4f; // time between the double barrel throws

    [SerializeField] private Animator animator; // optional throw anim
    [SerializeField] private string throwTriggerName = "BarrelToss"; // anim trigger name

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

            if (Random.value <= doubleBarrelChance) // sometimes throw a second one right after
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

        // random chance to spawn explosive
        if (explosivePrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("DonkeyKongBarrelSpawner: Missing explosivePrefab or spawnPoint reference."); // forgot to drag something in inspector
            return;
        }
        float randomChance = Random.Range(0f, 1f);
        if (randomChance <= explodingBarrelChance)
        {
            GameObject explosive = Instantiate(explosivePrefab, spawnPoint.position, spawnPoint.rotation); // random chance to spawn explosive 50% of the time

            if (!explosive.CompareTag(BarrelTag))
            {
                explosive.tag = BarrelTag; // just in case the prefab wasnt tagged right, fix it here
            }
        }
        
        //spawn normal barrels
        GameObject barrel = Instantiate(barrelPrefab, spawnPoint.position, spawnPoint.rotation); // actually spawn it, direction now handled by PlatformDirection on contact

        if (!barrel.CompareTag(BarrelTag))
        {
            barrel.tag = BarrelTag; // just in case the prefab wasnt tagged right, fix it here
        }
        

        
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