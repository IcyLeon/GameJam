using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private int amount;
    private ItemsSO itemsSO;

    public Item(ItemsSO itemsSO, int amt = 1)
    {
        amount = amt;
        this.itemsSO = itemsSO;
    }

    public void AddAmount(int amt)
    {
        amount += amt;
        ItemOverLoad();
    }

    public void Use(int amt)
    {
        amount -= amt;
        ItemOverLoad();
    }

    public int GetAmount()
    {
        return amount;
    }
    private void ItemOverLoad()
    {
        amount = Mathf.Max(0, amount);
    }

    public ItemsSO GetItemsSO()
    {
        return itemsSO;
    }
}
