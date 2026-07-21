using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public enum ScoreType
{
    JumpBarrel,
    HammerBarrel,
    BurnBarrel,
    DodgeFlyingBarrel,
    SavePauline
}
public class LevelManagerScript : MonoBehaviour
{
    public GameObject barrel;
    public GameObject donkeyKong;
    public GameObject mario;
    private MarioMovement marioMovement;

    // scoreboard
    public int score = 0;
    public GameObject scorePopupPrefab;

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
    public AudioSource fireBurn;

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

    // ----------------------- SOUND MANAGEMENT ----------------------- //

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

    public void fire_burnBarrelSFX()
    {
        fireBurn.Play();
    }

    // ----------------------- SCOREBOARD ----------------------- //
    public void AddScore(ScoreType scoreType)
    {
        int points = 0;

        switch (scoreType)
        {
            case ScoreType.JumpBarrel:
                points = 100;
                break;

            case ScoreType.HammerBarrel:
                points = 300;
                break;

            case ScoreType.BurnBarrel:
                points = 500;
                break;

            case ScoreType.DodgeFlyingBarrel:
                points = 200;
                break;

            case ScoreType.SavePauline:
                points = 5000;
                break;
        }

        score += points;

        // spawns a popup text upon scoring
        SpawnScorePopup(points, transform.position);
    }

    public void SpawnScorePopup(int points, Vector3 position)
    {
        GameObject popup = Instantiate(scorePopupPrefab, position, Quaternion.identity);

        popup.GetComponent<ScorePopup>().SetScore(points);
    }

    // ----------------------- SCENE MANAGEMENT ----------------------- //
    public void playerDeath()
    {
        Debug.Log("Player has died.");

        //placeholder
        Time.timeScale = 0f;

        stageTheme.Pause();
        hammerTime.Pause();
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
