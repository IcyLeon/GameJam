using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Button applySettingChangeButton;
    [SerializeField] Button resetSettingButton;
    [SerializeField] Button resetProgressOrSaveAndQuitButton;
    private float defaultsoundSettings;
    private float defaultmusicSettings;
    private float soundSettings;
    private float musicSettings;
    // Start is called before the first frame update
    void Start()
    {
        applySettingChangeButton.onClick.AddListener(ApplySetting);
        resetSettingButton.onClick.AddListener(ResetSettingsToDefault);
        soundSlider.onValueChanged.AddListener(CheckIfDifferent);
        musicSlider.onValueChanged.AddListener(CheckIfDifferent);

        TextMeshProUGUI displayText = resetProgressOrSaveAndQuitButton.GetComponentInChildren<TextMeshProUGUI>();

        // Check to see what type of button it is, and adjust its functionality and display text accordingly
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                displayText.text = "Reset Progress";
                resetProgressOrSaveAndQuitButton.onClick.AddListener(ResetProgress);
                break;
            case "GameScene":
                displayText.text = "Save and Back To Main Menu";
                resetProgressOrSaveAndQuitButton.onClick.AddListener(SaveAndQuit);
                break;
        }


        gameObject.SetActive(false);
    }

    // this script is called when the gameobject is set to active
    private void OnEnable()
    {
        // Get the playerPrefs of the key sound and music. If there is no playerPref for those, returnn the default of 1
        defaultsoundSettings = PlayerPrefs.GetFloat("Sound", 1);
        defaultmusicSettings = PlayerPrefs.GetFloat("Music", 1);
        soundSlider.value = defaultsoundSettings;
        musicSlider.value = defaultmusicSettings;
        soundSettings = defaultsoundSettings;
        musicSettings = defaultmusicSettings;
    }

    void ApplySetting()
    {
        defaultsoundSettings = soundSettings;
        defaultmusicSettings = musicSettings;
        PlayerPrefs.SetFloat("Sound", defaultsoundSettings);
        PlayerPrefs.SetFloat("Music", defaultmusicSettings);
    }

    void CheckIfDifferent(float value)
    {
        soundSettings = soundSlider.value;
        musicSettings = musicSlider.value;

        if (defaultsoundSettings == soundSettings && defaultmusicSettings == musicSettings)
        {
            applySettingChangeButton.interactable = false;
        }

        else
        {
            applySettingChangeButton.interactable = true;
        }
    }

    void ResetSettingsToDefault()
    {
        PlayerPrefs.DeleteKey("Sound");
        PlayerPrefs.DeleteKey("Music");
        defaultsoundSettings = 1;
        defaultmusicSettings = 1;
        soundSlider.value = defaultsoundSettings;
        musicSlider.value = defaultmusicSettings;
        soundSettings = defaultsoundSettings;
        musicSettings = defaultmusicSettings;
    }

    void ResetProgress()
    {
        if (JsonSaveFile.GetInstance() != null)
        {
            JsonSaveFile.GetInstance().ResetProgress();
        }
    }

    void SaveAndQuit()
    {
        if (JsonSaveFile.GetInstance() != null)
        {
            JsonSaveFile.GetInstance().SavePlayerData();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
