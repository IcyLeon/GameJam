using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPriceCalculation : MonoBehaviour
{
    private static FinalPriceCalculation instance;
    private List<UpgradeItemSO> unlockedItemsList;

    private void Awake()
    {
        instance = this;
    }

    public static FinalPriceCalculation GetInstance()
    {
        return instance;
    }

    public int CalculateFinalPrice(OrderInformation theOrder)
    {
        unlockedItemsList = InventoryManager.GetInstance().GetUpgradeItemsSOList();
        List<Item> itemList = theOrder.SendItemList;
        List<ItemsSO> itemSOList = new List<ItemsSO>();

        for (int i = 0; i < itemList.Count; i++)
        {
            int amt = itemList[i].GetAmount();
            for (int x = 0; x < amt; x++)
            {
                itemSOList.Add(itemList[i].GetItemsSO());
            }
        }

        float totalPrice = 0;

        for (int i = 0; i < itemSOList.Count; i++)
        {
            totalPrice += CalculateModifierApplied(itemSOList[i]);
        }

        // Get the wrapperList
        totalPrice *= InventoryManager.GetInstance().GetMostMultiplerWrapper().multipler;



        return (int)totalPrice;
    }

    public int CalculateModifierApplied(ItemsSO flower)
    {
        float basePrice = flower.StartingIncome;

        if (unlockedItemsList == null)
            return (int)basePrice;

        for (int i = 0; i < unlockedItemsList.Count; i++)
        {
            for (int x = 0; x < unlockedItemsList[i].affectWhatItems.Count; x++)
            {
                if (unlockedItemsList[i].affectWhatItems[x] == flower)
                {
                    basePrice *= unlockedItemsList[i].multiplerIfAny;
                }
            }
        }

        return (int)basePrice;
    }
}
