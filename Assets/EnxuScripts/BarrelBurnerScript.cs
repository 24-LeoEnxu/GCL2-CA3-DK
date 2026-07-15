using UnityEngine;

public class BarrelBurnerScript : MonoBehaviour
{
    public BurntBarrelScript bbs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Find BurntBarrelScript
        bbs = FindFirstObjectByType<BurntBarrelScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //If fire powerup triggers with player
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            bbs.burnActive = true;
            Destroy(gameObject);
        }
    }
}
