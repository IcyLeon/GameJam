using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    SFX,
    BGM
}

public enum AudioEnum
{
    NONE,
    CAFE_AMBIENCE,
    COLLECTING_COINS,
    BGM_MUSIC_SHOP,
    BGM_MUSIC_GAME,
    PRESS_BUTTON,
    PURCHASE_SUCCESS,
    PURCHASE_FAIL,
    MM_BGM
}

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public class AudioReference
    {
        public AudioClip clip;
        public AudioEnum audioEnum;
        public SoundType soundType;
    }
    private static SoundManager instance;
    [SerializeField] AudioReference[] SoundListInfo;

    private List<PlaySound> soundList = new(); // int is a dummy for now

    public void SubscribeToSoundDictionary(PlaySound playSound)
    {
        soundList.Add(playSound);
    }
    public AudioClip GetSFXCLip(AudioEnum audioEnum)
    {
        for (int i = 0; i < SoundListInfo.Length; i++)
        {
            if (SoundListInfo[i].audioEnum == audioEnum)
                return SoundListInfo[i].clip;
        }
        return null;
    }

    public SoundType GetSoundType(AudioEnum audioEnum)
    {
        for (int i = 0; i < SoundListInfo.Length; i++)
        {
            if (SoundListInfo[i].audioEnum == audioEnum)
                return SoundListInfo[i].soundType;
        }
        return default(SoundType);
    }

    private void Awake()
    {
        instance = this;
    }

    public static SoundManager GetInstance()
    {
        return instance;
    }

    public List<PlaySound> GetAudioSource()
    {
        return soundList;
    }
}
