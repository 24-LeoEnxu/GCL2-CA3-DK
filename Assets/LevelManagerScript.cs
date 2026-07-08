using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour
{
    public GameObject barrel;
    public GameObject donkeyKong;
    public GameObject mario;
    private MarioMovement marioMovement;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barrel = GetComponent<GameObject>();
        mario = GetComponent<GameObject>();
        donkeyKong = GetComponent<GameObject>();
        marioMovement = mario.GetComponent<MarioMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void marioDie()
    {
        mario.gameObject.SetActive(false);
        gameOver();
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
