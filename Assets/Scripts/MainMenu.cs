using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void CharSelection()
    {
        SceneManager.LoadScene("CharSel");
    }
    public void MultiPlayer()
    {
        SceneManager.LoadScene("Multiplayer");
    }
    public void MainMenuScr()
    {
        SceneManager.LoadScene("MainMenu");
    }
    //public void Quit()
    //{
    //    Application.Quit();
    //}
}
