using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    private Inventory inventory;
    public delegate void OnCurrencyValueChanged(int value);
    public OnCurrencyValueChanged onCurrencyValueChanged;

    private void Awake()
    {
        instance = this;
        inventory = new Inventory(0);
        onCurrencyValueChanged?.Invoke(0);
    }

    private void Start()
    {
        if (JsonSaveFile.GetInstance() != null)
        {
            inventory.LoadData();
        }
    }

    public List<ItemsSO> GetAllWrapperList(List<ItemsSO> ItemSOList)
    {
        List<ItemsSO> itemSOList = new();
        for (int i = 0; i < ItemSOList.Count; i++)
        {
            ItemsSO itemSO = AssetManager.GetInstance().GetWrapperItemsSO(ItemSOList[i]);
            if (itemSO != null)
            {
                itemSOList.Add(itemSO);
            }
        }
        return itemSOList;
    }
    public static InventoryManager GetInstance()
    {
        return instance;
    }
    public void AddCoins(int amt)
    {
        if (inventory == null)
            return;
        inventory.SetCoins(inventory.GetCoins() + amt);
        onCurrencyValueChanged?.Invoke(amt);
    }

    public void SubtractCoins(int amt)
    {
        if (inventory == null)
            return;

        inventory.SetCoins(inventory.GetCoins() - amt);
        onCurrencyValueChanged?.Invoke(-amt);
    }

    public int GetCoins()
    {
        if (inventory == null)
            return 0;

        return inventory.GetCoins();
    }

    public void AddFlowersSO(ItemsSO itemSO)
    {
        if (inventory == null)
            return;
        inventory.AddFlowerSO(itemSO);
    }

    public List<ItemsSO> GetFlowersSOList()
    {
        if (inventory == null)
            return null;

        return inventory.GetUnlockFlowerListSO();
    }

    public void AddWrappersSO(ItemsSO itemSO)
    {
        if (inventory == null)
            return;
        inventory.AddWraperSO(itemSO);
    }

    public List<ItemsSO> GetWrappersSOList()
    {
        if (inventory == null)
            return null;

        return inventory.GetUnlockWrapperListSO();
    }

    public void AddUpgradeSO(UpgradeItemSO upgradeItemSO)
    {
        if (inventory == null)
            return;
        inventory.AddUpgradeItemSO(upgradeItemSO);
    }

    public List<UpgradeItemSO> GetUpgradeItemsSOList()
    {
        if (inventory == null)
            return null;

        return inventory.GetUnlockedUpgradesListSO();
    }

    public ItemsSO GetMostMultiplerWrapper()
    {
        List<ItemsSO> wrapperList = inventory.GetUnlockWrapperListSO();
        if (wrapperList != null && wrapperList.Count > 0)
        {
            ItemsSO currentWrapper = wrapperList[0];

            for (int i = 1; i < wrapperList.Count; i++)
            {
                if (wrapperList[i].multipler > currentWrapper.multipler)
                {
                    currentWrapper = wrapperList[i];
                }
            }
            return currentWrapper;
        }
        return null;
    }
}
