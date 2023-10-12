using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private int Coins;
    private List<ItemsSO> UnlockItemsListSO;
    private List<UpgradeItemSO> UnlockUpgradeListSO;

    public int GetCoins()
    {
        return Coins;
    }

    public void SetCoins(int amt)
    {
        Coins = amt;
        if (JsonSaveFile.GetInstance() != null)
            JsonSaveFile.GetInstance().UpdateCurrency(amt);
    }

    public Inventory(int Coins = 0)
    {
        this.Coins = Coins;
        UnlockItemsListSO = new();
        UnlockUpgradeListSO = new();
    }

    public void AddItemsSO(ItemsSO ItemsSO)
    {
        UnlockItemsListSO.Add(ItemsSO);
        if (JsonSaveFile.GetInstance() != null)
            JsonSaveFile.GetInstance().UpdateShopItemList(UnlockItemsListSO);
    }

    public List<ItemsSO> GetUnlockItemsListSO()
    {
        return UnlockItemsListSO;
    }

    public void AddUpgradeItemSO(UpgradeItemSO upgradeItemSO)
    {
        UnlockUpgradeListSO.Add(upgradeItemSO);
        if (JsonSaveFile.GetInstance() != null)
            JsonSaveFile.GetInstance().UpdateUpgradeItemList(UnlockUpgradeListSO);
    }

    public List<UpgradeItemSO> GetUnlockedUpgradesListSO()
    {
        return UnlockUpgradeListSO;
    }


}
