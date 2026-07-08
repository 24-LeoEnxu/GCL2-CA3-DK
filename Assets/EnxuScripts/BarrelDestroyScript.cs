using UnityEngine;

public class BarrelDestroyScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        //Destroys barrels when triggered
        if(other.CompareTag("Barrel"))
        {
            Destroy(other.gameObject);
        }
    }
}
