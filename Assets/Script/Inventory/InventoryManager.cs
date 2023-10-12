using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    private Inventory inventory;
    public event Action onCurrencyValueChanged;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inventory = new Inventory(1000);
        if (JsonSaveFile.GetInstance() != null)
        {
            inventory.LoadData();
        }
        onCurrencyValueChanged?.Invoke();
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
        onCurrencyValueChanged?.Invoke();
    }

    public void SubtractCoins(int amt)
    {
        if (inventory == null)
            return;

        inventory.SetCoins(inventory.GetCoins() - amt);
        onCurrencyValueChanged?.Invoke();
    }

    public int GetCoins()
    {
        if (inventory == null)
            return 0;

        return inventory.GetCoins();
    }

    public void AddItemsSO(ItemsSO itemSO)
    {
        if (inventory == null)
            return;
        inventory.AddItemsSO(itemSO);
    }

    public List<ItemsSO> GetItemsSOList()
    {
        if (inventory == null)
            return null;

        return inventory.GetUnlockItemsListSO();
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
}
