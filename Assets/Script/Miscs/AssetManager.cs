using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    COMMON,
    UNCOMMON,
    RARE,
}

public class AssetManager : MonoBehaviour
{
    [Serializable]
    public class BoothInfo
    {
        public Sprite BoothSprite;
        public ItemsSO itemsSO;
        public Station station;
    }
    [SerializeField] GameObject TimerPrefab;
    [SerializeField] BoothInfo[] BoothInfoList;
    [SerializeField] UpgradeItemSO[] UpgradeItemSOList;
    [SerializeField] ItemsSO[] FlowerItemSOList;
    [SerializeField] ItemsSO[] WrapperItemSOList;

    public event Action onSelectedClick;
    private Station SelectedStation;

    private static AssetManager instance;
    public GameObject StationPrefab;
    public GameObject CanvasWorldText;
    public ParticleSystem ParticlesEffectBurst;
    [SerializeField] Color32 CommonColor, UncommonColor, RareColor;

    private void Awake()
    {
        instance = this;
        
        if (JsonSaveFile.GetInstance() != null)
        {
            JsonSaveFile.GetInstance().LoadPlayerData();
        }
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
                return UncommonColor;
            case Rarity.RARE:
                return RareColor;
        }
        return CommonColor;
    }

    public GameObject GetTimerPrefab()
    {
        return TimerPrefab;
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

    public Station GetStation(ItemsSO itemsSO)
    {
        for (int i = 0; i < BoothInfoList.Length; i++)
        {
            if (BoothInfoList[i].itemsSO == itemsSO)
                return BoothInfoList[i].station;
        }
        return null;
    }

    public BoothInfo[] GetBoothInfo()
    {
        return BoothInfoList;
    }

    public string AdjustCurrencyDisplay(int currencyAmt)
    {
        // If more than Billion
        if (currencyAmt >= 1000000000)
        {
            float displayValue = currencyAmt / 1000000000.0f;
            return displayValue.ToString("F2") + "b";
        }
        // If more than Million
        else if (currencyAmt >= 1000000)
        {
            float displayValue = currencyAmt / 1000000.0f;
            return displayValue.ToString("F2") + "m";
        }
        // If more than Thousand
        else if (currencyAmt >= 1000)
        {
            float displayValue = currencyAmt / 1000.0f;
            return displayValue.ToString("F2") + "k";
        }

        else
        {
            return currencyAmt.ToString();
        }
    }

    public ItemsSO GetFlowerItemsSO(ItemsSO itemsSO)
    {
        for(int i = 0; i < FlowerItemSOList.Length; i++)
        {
            if (itemsSO == FlowerItemSOList[i])
                return FlowerItemSOList[i];
        }
        return null;
    }
    public ItemsSO GetWrapperItemsSO(ItemsSO itemsSO)
    {
        for (int i = 0; i < WrapperItemSOList.Length; i++)
        {
            if (itemsSO == WrapperItemSOList[i])
                return WrapperItemSOList[i];
        }
        return null;
    }

    public ItemsSO[] GetFlowerItemSOList()
    {
        return FlowerItemSOList;
    }

    public ItemsSO[] GetWrapperItemSOList()
    {
        return WrapperItemSOList;
    }

    public UpgradeItemSO[] GetUpgradeItemsSOList()
    {
        return UpgradeItemSOList;
    }
    //public ItemsSO GetItemsSOByFlowerTypes(WrapTypes wrapTypes)
    //{
    //    for (int i = 0; i < GetItemsSOList().Length; i++)
    //    {
    //        if (wrapTypes == GetItemsSOList()[i].)
    //            return GetItemsSOList()[i];
    //    }
    //    return null;
    //}

}
