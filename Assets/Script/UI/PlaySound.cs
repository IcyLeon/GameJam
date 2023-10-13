using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioEnum AudioEnum;
    [SerializeField] bool Looped;
    private SoundManager soundManager;

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public void SetAudioEnum(AudioEnum audioEnum)
    {
        AudioEnum = audioEnum;
    }
    private void Start()
    {
        soundManager = SoundManager.GetInstance();
        if (audioSource != null)
        {
            audioSource.loop = Looped;
            soundManager.SubscribeToSoundDictionary(audioSource);
        }
    }

    // Start is called before the first frame update
    public void PlayAudio(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            if (AudioEnum != AudioEnum.NONE)
                audioSource.clip = soundManager.GetSFXCLip(AudioEnum);
            audioSource.Play();
        }
    }

    public void StopAudio(AudioSource audioSource)
    {
        if (audioSource != null)
            audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
