using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private int Coins;
    private List<ItemsSO> UnlockItemsListSO;

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
    }

    public void AddItemsSO(ItemsSO ItemsSO)
    {
        UnlockItemsListSO.Add(ItemsSO);
    }

    public List<ItemsSO> GetUnlockItemsListSO()
    {
        return UnlockItemsListSO;
    }

}
