using UnityEngine;

public class HammerScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Barrel") ||  collider.CompareTag("ThrownBarrel"))
        {
            Destroy(collider.gameObject);
        }
    }
}
