using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    [Serializable]
    public class BoothInfo
    {
        public Sprite BoothSprite;
        public ItemsSO itemsSO;
    }
    [SerializeField] BoothInfo[] BoothInfoList;
    public event Action onSelectedClick;
    private Station SelectedStation;

    private static AssetManager instance;
    public GameObject StationPrefab;
    [SerializeField] Color32 CommonColor, RareColor;

    private void Awake()
    {
        instance = this;
    }

    public static AssetManager GetInstance()
    {
        return instance;
    }
    public Color32 GetRarityColor(Rarity rarity)
    {
        switch(rarity)
        {
            case Rarity.COMMON:
                return CommonColor;
            case Rarity.UNCOMMON:
                return RareColor;
        }
        return CommonColor;
    }

    public Station GetSelectedStation()
    {
        return SelectedStation;
    }

    public void SetSelectedStation(Station station)
    {
        SelectedStation = station;
        onSelectedClick?.Invoke();
    }

    public Sprite GetBoothSprite(ItemsSO itemsSO)
    {
        for (int i = 0; i < BoothInfoList.Length; i++)
        {
            if (BoothInfoList[i].itemsSO == itemsSO)
                return BoothInfoList[i].BoothSprite;
        }
        return null;
    }
}
