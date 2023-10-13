using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;
    [SerializeField] GameObject settingsPage;

    private void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void OpenSettings()
    {
        settingsPage.SetActive(true);
    }

    void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
