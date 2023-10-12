using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPriceCalculation : MonoBehaviour
{
    //private static FinalPriceCalculation instance;
    //private List<UpgradeItemSO> unlockedItemsList;

    //private void Awake()
    //{
    //    instance = this;
    //}

    //public static FinalPriceCalculation GetInstance()
    //{
    //    return instance;
    //}

    //public void CalculateFinalPrice(OrderInformation theOrder)
    //{
    //    unlockedItemsList = InventoryManager.GetInstance().GetUpgradeItemsSOList();
    //    List<FlowerTypes> flowerList = theOrder.flowerTypeList;
    //    float totalPrice = 0;

    //    for (int i = 0; i < flowerList.Count; i++)
    //    {
    //        totalPrice += CalculateModifierApplied(flowerList[i]);
    //    }

    //    Debug.Log(totalPrice);
    //}

    //int CalculateModifierApplied(FlowerTypes flower)
    //{
    //    ItemsSO flowerItem = GetItemSOReference(flower);
    //    float basePrice = flowerItem.StartingIncome;

    //    if (unlockedItemsList == null)
    //        return (int)basePrice;

    //    for (int i = 0; i < unlockedItemsList.Count; i++)
    //    {
    //        if (unlockedItemsList[i].affectedFlowerTypes == flower)
    //        {
    //            basePrice *= unlockedItemsList[i].multiplerIfAny;
    //        }

    //        else if (unlockedItemsList[i].affectedRarity == flowerItem.Rarity)
    //        {
    //            basePrice *= unlockedItemsList[i].multiplerIfAny;
    //        }
    //    }

    //    return (int)basePrice;
    //}

    //ItemsSO GetItemSOReference(FlowerTypes flowerType)
    //{
    //    List<ItemsSO> flowerList = InventoryManager.GetInstance().GetItemsSOList();

    //    for (int i = 0; i < flowerList.Count; i++)
    //    {
    //        if (flowerList[i].flowerType == flowerType)
    //        {
    //            return flowerList[i];
    //        }
    //    }
    //    return null;
    //}
}
