using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject creditsMenu;
    public GameObject settingsMenu;

    AudioManager audioManager;

    void Awake()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        settingsMenu.SetActive(false);

        audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void Start()
    {
        audioManager.Play("MenuTheme");
    }

    public void OnStartPressed()
    {
        audioManager.Stop("MenuTheme", true, 0f, 1f);
        StartCoroutine(FadeIn());
    }

    public void OnBackPressed()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void OnSettingsPressed()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnCreditsPressed()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        audioManager.Play("Swoosh");
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainScene");
    }
}
