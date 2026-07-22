using TMPro;
using UnityEngine;
using System.Collections;

public class ScorePopup : MonoBehaviour
{
    public TMP_Text scoreText;
    public CanvasGroup canvasGroup;

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float timer = 1f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            transform.position += Vector3.up * 40f * Time.deltaTime;

            canvasGroup.alpha = timer;

            yield return null;
        }

        Destroy(gameObject);
    }
}