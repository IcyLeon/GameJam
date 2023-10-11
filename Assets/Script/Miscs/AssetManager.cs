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

public enum FlowerTypes
{
    NONE,
    ROSE,
    TULIP,
    DAISY,
    BABY_BREATH,
    LILY_OF_THE_VALLEY,
    ORCHID,
    LAVENDER,
    MOONFLOWER,
    PEONY,
}

public enum WrapTypes
{
    NONE,
    WRAP_1,
}

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
    [SerializeField] Color32 CommonColor, UncommonColor, RareColor;

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
                return UncommonColor;
            case Rarity.RARE:
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
}
