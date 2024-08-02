using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelScript : MonoBehaviour
{
    public void GameScene()
    {
        SceneManager.LoadScene("Level 1 Forest");
    }
    public void BackBtn()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
