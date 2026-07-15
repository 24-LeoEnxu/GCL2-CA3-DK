using UnityEngine;

public class BarrelFireScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Parentss burning fire onto rolling barrels
        if (collider.CompareTag("Barrel"))
        {
            gameObject.transform.SetParent(collider.transform);
            Destroy(collider.gameObject, 1);
        }
    }
}
