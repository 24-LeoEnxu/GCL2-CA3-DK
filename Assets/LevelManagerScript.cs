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
        barrel = GameObject.FindWithTag("Barrel");
        mario = GameObject.FindWithTag("Player");
        donkeyKong = GameObject.FindWithTag("DonkeyKong");
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
