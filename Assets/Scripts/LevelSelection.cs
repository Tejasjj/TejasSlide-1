using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Level
{
    public string LevelName;
    public Sprite LevelImage;
}

public class LevelSelection : MonoBehaviour
{
    public Level[] Levels; // array of levels containing names and images
    public Text LevelNameText; // text UI element to display the level name
    public Image LevelImageView; // image UI element to display the level image
    public Button NextButton;
    public Button PreviousButton;
    public Button StartButton;

    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLevelPreview();
        NextButton.onClick.AddListener(ShowNextLevel);
        PreviousButton.onClick.AddListener(ShowPreviousLevel);
        StartButton.onClick.AddListener(StartLevel);
    }

    void UpdateLevelPreview()
    {
        LevelNameText.text = Levels[currentIndex].LevelName;
        LevelImageView.sprite = Levels[currentIndex].LevelImage;
    }

    public void ShowNextLevel()
    {
        currentIndex = (currentIndex + 1) % Levels.Length;
        UpdateLevelPreview();
    }

    public void ShowPreviousLevel()
    {
        currentIndex = (currentIndex - 1 + Levels.Length) % Levels.Length;
        UpdateLevelPreview();
    }

    public void StartLevel()
    {
        PlayerPrefs.SetString("SelectedLevel", Levels[currentIndex].LevelName);
        SceneManager.LoadScene(Levels[currentIndex].LevelName);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
