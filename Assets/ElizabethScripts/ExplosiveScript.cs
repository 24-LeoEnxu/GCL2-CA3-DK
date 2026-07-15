using UnityEngine;

public class ExplosiveScript : MonoBehaviour
{
    public GameObject levelManager;
    private LevelManagerScript levelManagerScript;
    public GameObject mario;

    private explosionScript explosion;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = GetComponent<GameObject>();
        levelManagerScript = levelManager.GetComponent<LevelManagerScript>();
        mario = GetComponent<GameObject>();

        explosion = GameObject.FindGameObjectWithTag("explosion").GetComponent<explosionScript>();
        explosion.TriggerExplosion(5);


    }

    // Update is called once per frame
    void Update()
    {

    }





}