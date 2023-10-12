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
    }

    public List<ItemsSO> GetUnlockItemsListSO()
    {
        return UnlockItemsListSO;
    }

    public void AddUpgradeItemSO(UpgradeItemSO upgradeItemSO)
    {
        UnlockUpgradeListSO.Add(upgradeItemSO);
    }

    public List<UpgradeItemSO> GetUnlockedUpgradesListSO()
    {
        return UnlockUpgradeListSO;
    }

    public void LoadData()
    {
        Coins = JsonSaveFile.GetInstance().GetAmt();
    }
}
