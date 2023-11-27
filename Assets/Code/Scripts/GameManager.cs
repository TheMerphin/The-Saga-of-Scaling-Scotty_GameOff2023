using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuContainer;
    bool gameHasEnded;
    AudioManager audioManager;

    public string[] levelSceneNames;
    private int levelIndex = 0;
    public GameObject gameOverScreen;
    public GameObject finishedScreen;

    void Awake()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    void Start()
    {
        StartCoroutine(SwitchLevel(false));
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

    private IEnumerator FadeIn(bool exitGame)
    {
        audioManager.Play("Swoosh");
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        if(exitGame) SceneManager.LoadScene("MenuScene");
    }

    private IEnumerator FadeOut()
    {
        audioManager.Play("Swoosh");
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
    }

    public void GameOver()
    {
        if(!gameHasEnded)
        {
            gameHasEnded = true;
            gameOverScreen.SetActive(true);
            audioManager.Play("LoseLaugh");
            Invoke("OnExitClicked", 8f);
        }
        
    }

    public void OnExitClicked()
    {
        PauseGame(false);
        StartCoroutine(FadeIn(true));
    }

    public void ProgressToNextLevel()
    {
        levelIndex++;

        if (levelIndex >= levelSceneNames.Length)
        {
            finishedScreen.SetActive(true);
            PauseGame(true);
        }
        else
        {
            StartCoroutine(SwitchLevel(true));
        }
    }

    private IEnumerator SwitchLevel(bool fadeIn)
    {
        if(fadeIn) yield return StartCoroutine(FadeIn(false));

        yield return SceneManager.LoadSceneAsync(levelSceneNames[levelIndex], LoadSceneMode.Additive);
        if (levelIndex > 0) yield return SceneManager.UnloadSceneAsync(levelSceneNames[levelIndex - 1]);

        StartCoroutine(FadeOut());
    }
}
