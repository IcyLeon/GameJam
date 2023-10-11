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
        public Transform pos;
    }
    [SerializeField] BoothInfo[] BoothInfoList;
    [SerializeField] ItemsSO[] ItemSOList; 

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

    public Transform GetBoothPos(ItemsSO itemsSO)
    {
        for (int i = 0; i < BoothInfoList.Length; i++)
        {
            if (BoothInfoList[i].itemsSO == itemsSO)
                return BoothInfoList[i].pos;
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

    public ItemsSO[] GetItemsSOList()
    {
        return ItemSOList;
    }

    public ItemsSO GetItemsSOByFlowerTypes(FlowerTypes flowerType)
    {
        for(int i = 0; i < GetItemsSOList().Length; i++)
        {
            if (flowerType == GetItemsSOList()[i].flowerType)
                return GetItemsSOList()[i];
        }
        return null;
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
