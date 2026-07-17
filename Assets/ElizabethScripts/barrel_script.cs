using UnityEngine;

public class barrel_script : MonoBehaviour
{    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelManagerScript.Instance.playerDeath();
        }
    }

}
