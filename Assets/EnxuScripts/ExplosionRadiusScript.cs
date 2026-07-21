using UnityEngine;

public class ExplosionRadiusScript : MonoBehaviour
{
    public LevelManagerScript lm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lm = GetComponent<LevelManagerScript>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            LevelManagerScript.Instance.playerDeath();
        }
    }
}
