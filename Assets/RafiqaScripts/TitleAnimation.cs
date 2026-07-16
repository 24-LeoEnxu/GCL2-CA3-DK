using System.Collections;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    // Donkey Kong Title
    public GameObject DK_TitleGroup;

    // Rendition text
    public GameObject Rendition;

    // Parent of Credits UI
    public GameObject CreditsParent;

    // Credit roll Parent
    public GameObject CreditsRoll;

    // Back to main menu Parent
    public GameObject BackToMainMenu;

    public AudioSource sfx;
    public CanvasGroup canvasGroup;

    RectTransform rect;
    bool animationCheck;

    void Start()
    {
        // get Rect Transform component
        rect = GetComponent<RectTransform>();

        // calling position variable of Rect Transform of game object
        Vector2 pos = rect.anchoredPosition;
        pos.y = -175;
        rect.anchoredPosition = pos;

        // prep hiding text & credit roll
        Rendition.SetActive(false);
        CreditsRoll.SetActive(false);
        BackToMainMenu.SetActive(false);

        // force transparency of game object to be visible
        canvasGroup.alpha = 1;
    }

    void Update()
    {
        if (CreditsParent.activeSelf)
        {
            if (rect.anchoredPosition.y < 0)
            {
                Vector2 pos = rect.anchoredPosition;
                pos.y = Mathf.MoveTowards(pos.y, 15f, 150f * Time.deltaTime);
                rect.anchoredPosition = pos;
            }
            else if (!animationCheck)
            {
                animationCheck = true;
                StartCoroutine(RenditionAppear());
            }
        }

        IEnumerator RenditionAppear()
        {
            yield return new WaitForSeconds(1f);

            Rendition.SetActive(true);
            sfx.Play();

            yield return new WaitForSeconds(0.5f);
            Rendition.SetActive(false);

            yield return new WaitForSeconds(0.5f);
            Rendition.SetActive(true);

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(FadeOut());
        }

        IEnumerator FadeOut()
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 0;
            gameObject.SetActive(false);

            CreditsRoll.SetActive(true);
            BackToMainMenu.SetActive(true);
        }
    }
}
