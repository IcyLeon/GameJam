using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private int Coins;
    private List<ItemsSO> UnlockFlowerListSO;
    private List<ItemsSO> UnlockWrapperListSO;
    private List<UpgradeItemSO> UnlockUpgradeListSO;

    public int GetCoins()
    {
        return Coins;
    }

    public void SetCoins(int amt)
    {
        Coins = amt;
        if (Coins > 2000000000)
        {
            Coins = 2000000000;
        }
    }

    public Inventory(int Coins = 0)
    {
        this.Coins = Coins;
        UnlockFlowerListSO = new();
        UnlockWrapperListSO = new();
        UnlockUpgradeListSO = new();
    }

    public void AddFlowerSO(ItemsSO ItemsSO)
    {
        UnlockFlowerListSO.Add(ItemsSO);
    }

    public List<ItemsSO> GetUnlockFlowerListSO()
    {
        return UnlockFlowerListSO;
    }

    public void AddWraperSO(ItemsSO ItemsSO)
    {
        UnlockWrapperListSO.Add(ItemsSO);
    }

    public List<ItemsSO> GetUnlockWrapperListSO()
    {
        return UnlockWrapperListSO;
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
