using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private int Coins;
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
    }
}
