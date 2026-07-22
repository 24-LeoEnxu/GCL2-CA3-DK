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

    public GameObject WinUI;
    public GameObject GameUI;

    public static int hsEndgame;
    public TMP_Text highscoreEndgame;

    // scoreboard
    public int score = 0;
    public int bonusCountdown = 5000;
    public static int highScore = 0;
    public static int lives = 3;

    public GameObject scorePopupPrefab;

    public TMP_Text highScoreText;
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text bonusText;

    public Canvas scoreCanvas;

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
        WinUI.SetActive(false);
        GameUI.SetActive(true);

        barrel = GameObject.FindWithTag("Barrel");
        mario = GameObject.FindWithTag("Player");
        donkeyKong = GameObject.FindWithTag("DonkeyKong");
        marioMovement = mario.GetComponent<MarioMovement>();

        highScoreText.text = "TOP-" + highScore.ToString("D6");
        livesText.text = "[" + lives + "]";

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
    public void AddScore(ScoreType scoreType, Vector3 worldPosition)
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
                points = 100;
                break;
        }

        score += points;
        Debug.Log("Current score:" + score);

        scoreText.text = "I-" + score.ToString("D6");

        SpawnScorePopup(points, worldPosition);
        UpdateHighScore();
    }

    public void SpawnScorePopup(int points, Vector3 worldPosition)
    {
        // converts screen-overlay canvas position to world position
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

        // spawns text
        GameObject popup = Instantiate(scorePopupPrefab, scoreCanvas.transform);

        popup.GetComponent<RectTransform>().position = screenPos;
        popup.GetComponent<ScorePopup>().SetScore(points);
    }

    void UpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score;

            highScoreText.text = "TOP-" + highScore.ToString("D6");
        }
    }

    public void callForBonus()
    {
        StartCoroutine(BonusCountdown());
    }
    IEnumerator BonusCountdown()
    {
        while (bonusCountdown > 0)
        {
            bonusText.text = "[" + bonusCountdown + "]";

            yield return new WaitForSeconds(1f);

            bonusCountdown -= 100;
        }

        bonusCountdown = 0;
        bonusText.text = "[0000]";

        playerDeath();
    }

    // ----------------------- SCENE MANAGEMENT ----------------------- //
    public void playerDeath()
    {
        Debug.Log("Player has died.");

        stageTheme.Pause();
        hammerTime.Pause();
        marioDeath.Play();

        StartCoroutine(Freeze());
    }

    public void restartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void gameOver()
    {
        SceneManager.LoadScene(0);
    }

    public void winGame()
    {
        stageTheme.Stop();
        hammerTime.Stop();
        stageClear.Play();

        highScore += bonusCountdown;
        hsEndgame = highScore;

        highscoreEndgame.text = "[" + hsEndgame + "]";

        StartCoroutine(End());

    }

    IEnumerator End()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1f;

        GameUI.SetActive(false);
        WinUI.SetActive(true);

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(7f);
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }

    IEnumerator Freeze()
    {
        marioMovement.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        marioMovement.animator.SetBool("MarioDeath", true);

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1f;

        lives--;
        livesText.text = "[" + lives + "]";

        if (lives == 0)
        {
            gameOver();
        }

        else
        {
            restartGame();
        }
    }
}
