using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour
{
    public GameObject barrel;
    public GameObject donkeyKong;
    public GameObject mario;

    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barrel = GetComponent<GameObject>();
        mario = GetComponent<GameObject>();
        donkeyKong = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        /*
        gameOverScreen.SetActive(true);
        gameOverSound.Play();
        */


    }
}
