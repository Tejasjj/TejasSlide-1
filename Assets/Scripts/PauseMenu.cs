using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuPanel;
    public Button pauseButton;
    public Button HomeButton;
    public Button RestartButton;
    public Button OptionsButton;
   // public Button QuitButton;

    public GameObject OptionsPanel;
    public Button OptionMusicButton;
    public Button OptionSFXButton;
    public Button OptionBackButton;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        PauseMenuPanel.SetActive(false);
        OptionsPanel.SetActive(false);

        pauseButton.onClick.AddListener(TogglePause);

        HomeButton.onClick.AddListener(GoHome);
        RestartButton.onClick.AddListener(RestartGame);
        OptionsButton.onClick.AddListener(OpenOptions);
//        QuitButton.onClick.AddListener(QuitGame);

        OptionMusicButton.onClick.AddListener(MusicToggle);
        OptionSFXButton.onClick.AddListener(SFXToggle);
        OptionBackButton.onClick.AddListener(OptionsBack);
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if(isPaused)
        {
            Time.timeScale = 0f;
            PauseMenuPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            PauseMenuPanel.SetActive(false);
        }
    }

    void GoHome()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OpenOptions()
    {
        PauseMenuPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    void MusicToggle()
    {

    }

    void SFXToggle()
    {

    }

    void OptionsBack()
    {
        OptionsPanel.SetActive(false);
        PauseMenuPanel.SetActive(true);
    }

    //void QuitGame()
    //{
    //    Application.Quit();
    //}

    // Update is called once per frame
    //void Update()
    //{

    //}
}
