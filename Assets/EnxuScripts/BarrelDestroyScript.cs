using UnityEngine;

public class BarrelDestroyScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Barrel"))
        {
            Destroy(other.gameObject);
        }
    }
}
