using System.Collections;
using UnityEngine;
using TMPro;

public class BacktoMenu : MonoBehaviour
{
    public AudioSource sfxSelect;

    public GameObject MainMenu;
    public GameObject Credits;

    public PointerScript Pointer;

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return)) || (Input.GetKeyDown(KeyCode.Space)))
        {
            print("Back to menu.");
            sfxSelect.Play();

            Pointer.choice = 1;

            Credits.SetActive(false);
            MainMenu.SetActive(true);
        }
    }
}
