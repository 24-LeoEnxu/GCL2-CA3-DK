using TMPro;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    public TMP_Text text;

    public void SetScore(int score)
    {
        text.text = score.ToString();
    }

    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * 40f;
    }

    void Start()
    {
        Destroy(gameObject, 0.75f);
    }
}