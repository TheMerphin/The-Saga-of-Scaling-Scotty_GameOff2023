using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuContainer;

    AudioManager audioManager;

    void Awake()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale > 0f)
        {
            if (menuContainer.gameObject.activeSelf)
            {
                menuContainer.SetActive(false);
                PauseGame(false);
            }
            else
            {
                menuContainer.SetActive(true);
                PauseGame(true);
            }
        }
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            menuContainer.SetActive(false);
        }
    }

    private IEnumerator FadeIn()
    {
        audioManager.Play("Swoosh");
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MenuScene");
    }

    private IEnumerator FadeOut()
    {
        audioManager.Play("Swoosh");
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
    }

    public void OnExitClicked()
    {
        PauseGame(false);
        StartCoroutine(FadeIn());
    }
}
