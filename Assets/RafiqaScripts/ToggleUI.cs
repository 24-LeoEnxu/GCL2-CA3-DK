using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Credits;
    public PointerScript PointerScript;
    public GameObject Pointer;

    public AudioSource sfxSelect;
    public AudioSource bgm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainMenu.SetActive(true);
        Credits.SetActive(false);

        bgm.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if ((PointerScript.choice == 1 && Input.GetKeyDown(KeyCode.Return)) || (PointerScript.choice == 1 && Input.GetKeyDown(KeyCode.Space)))
        {
            print("Start Game trigger.");
            sfxSelect.Play();
        }

        if ((PointerScript.choice == 2 && Input.GetKeyDown(KeyCode.Return)) || (PointerScript.choice == 2 && Input.GetKeyDown(KeyCode.Space)))
        {
            print("Credits trigger.");
            sfxSelect.Play();

            Pointer.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, -50f);

            MainMenu.SetActive(false);
            Credits.SetActive(true);
        }

        if ((PointerScript.choice == 3 && Input.GetKeyDown(KeyCode.Return)) || (PointerScript.choice == 3 && Input.GetKeyDown(KeyCode.Space)))
        {
            print("Quit game trigger.");
            sfxSelect.Play();

            //IF UNITY PLAYER
            UnityEditor.EditorApplication.isPlaying = false;

            //IF BUILD
            Application.Quit();
        }
    }
}
