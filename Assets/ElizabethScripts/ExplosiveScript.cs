using UnityEngine;

public class ExplosiveScript : MonoBehaviour
{
    public GameObject levelManager;
    private LevelManagerScript levelManagerScript;
    public GameObject mario;

    public GameObject explosion;
    private explosionScript explosionScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = GetComponent<GameObject>();
        levelManagerScript = levelManager.GetComponent<LevelManagerScript>();
        mario = GameObject.FindWithTag("Player");

        explosion = transform.GetChild(0).gameObject;
        explosionScript = explosion.GetComponent<explosionScript>();
        explosionScript.TriggerExplosion(5);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    

}
