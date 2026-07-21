using UnityEngine;

public class BarrelDestroyOnEnd : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Barrel"))
        {
            Destroy(other.gameObject);
        }
    }
}
