using UnityEngine;

public class barrel_script : MonoBehaviour
{
    public GameObject levelManager;
    private LevelManagerScript levelManagerScript;
    public GameObject mario;

    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = GetComponent<GameObject>();
        levelManagerScript = levelManager.GetComponent<LevelManagerScript>();
        mario = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getDestroyed()
    {
        Destroy(gameObject);
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            levelManagerScript.marioDie();
        }
    }

}
