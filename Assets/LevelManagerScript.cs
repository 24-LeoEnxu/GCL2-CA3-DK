using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour
{
    public GameObject barrel;
    public GameObject donkeyKong;
    public GameObject mario;
    private MarioMovement marioMovement;

    // audio sources - bgm
    public AudioSource stageStart;
    public AudioSource stageTheme;
    public AudioSource stageClear;
    public AudioSource hammerTime;
    public AudioSource marioDeath;

    // audio sources - sfx
    public AudioSource jumpSFX;
    public AudioSource jump_successSFX;
    public AudioSource marioBonk;
    public AudioSource marioWalk;

    // creates a singleton instead of having to fetch the level manager script every time
    public static LevelManagerScript Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    IEnumerator Start()
    {
        barrel = GameObject.FindWithTag("Barrel");
        mario = GameObject.FindWithTag("Player");
        donkeyKong = GameObject.FindWithTag("DonkeyKong");
        marioMovement = mario.GetComponent<MarioMovement>();

        stageStart.Play();

        yield return new WaitForSeconds(stageStart.clip.length);

        stageTheme.Play();
    }

    // bgm
    public void play_hammerTimeSFX()
    {
        stageTheme.Pause();
        hammerTime.Play();
    }

    public void stop_HammerTimeSFX()
    {
        hammerTime.Stop();
        stageTheme.UnPause();
    }

    public void play_destroyBarrelSFX()
    {
        marioBonk.Play();
    }

    // mario sfx
    public void play_jumpSFX()
    {
        jumpSFX.Play();
    }

    public void play_marioWalkSFX()
    {
        marioWalk.Play();
    }

    public void stop_marioWalkSFX()
    {
        marioWalk.Stop();
    }

    // scene management
    public void playerDeath()
    {
        Debug.Log("Player has died.");

        //placeholder
        Time.timeScale = 0f;

        stageTheme.Pause();
        marioDeath.Play();

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
