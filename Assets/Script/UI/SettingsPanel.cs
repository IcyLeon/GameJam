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

        UpdateSettings();
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
        UpdateSettings();
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
        UpdateSettings();
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

    public void UpdateSettings()
    {
        if (SoundManager.GetInstance() != null)
        {
            List<PlaySound> audioList = SoundManager.GetInstance().GetAudioSource();
            for (int i = 0; i < audioList.Count; i++)
            {
                if (audioList[i] != null)
                {
                    if (audioList[i].GetAudioSource())
                    {
                        switch (SoundManager.GetInstance().GetSoundType(audioList[i].GetAudioEnum()))
                        {
                            case SoundType.SFX:
                                audioList[i].GetAudioSource().volume = defaultsoundSettings;
                                break;
                            case SoundType.BGM:
                                audioList[i].GetAudioSource().volume = defaultmusicSettings;
                                break;
                        }
                        Debug.Log(SoundManager.GetInstance().GetSoundType(audioList[i].GetAudioEnum()));
                    }
                }

            }
        }
    }
}
