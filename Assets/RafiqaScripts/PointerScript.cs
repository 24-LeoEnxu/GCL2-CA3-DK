using UnityEngine;

public class PointerScript : MonoBehaviour
{
    public int choice = 1;

    public AudioSource sfxHover;

    void Start()
    {
        print("Hovering option: " + choice);
    }

void Update()
    { 
        RectTransform rect = GetComponent<RectTransform>();

        if (choice < 3 && Input.GetKeyDown(KeyCode.S))
        {
            rect.anchoredPosition += new Vector2(0, -15f);
            sfxHover.Play();

            choice++;
            print("Hovering option: " + choice);
        }

        else if (choice > 1 && Input.GetKeyDown(KeyCode.W))
        {
            rect.anchoredPosition += new Vector2(0, 15f);
            sfxHover.Play();

            choice--;
            print("Hovering option: " + choice);
        }
    }
}
