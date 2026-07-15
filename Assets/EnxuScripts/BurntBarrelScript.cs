using UnityEngine;

public class BurntBarrelScript : MonoBehaviour
{
    public GameObject barrelBurner;
    public bool burnActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Looks for all rolling barrels in scene
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Barrel");

        //if burnActive is true, spawns fire on rolling barrels
        if (burnActive)
        {
            //looks for every rolling barrel on scene
            foreach (GameObject sp in spawnPoints)
            {
                Instantiate(barrelBurner, sp.transform.position, Quaternion.identity);
                burnActive = false;
            }
        }
    }
}