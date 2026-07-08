using Unity.VisualScripting;
using UnityEngine;

public class barrel_script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetDestroyed()
    {

        gameObject.SetActive(false);
        print("barrel destroyed");


    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GetDestroyed();

        }
    }

}
