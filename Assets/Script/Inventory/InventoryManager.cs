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

    public static InventoryManager GetInstance()
    {
        return instance;
    }
    public void AddCoins(int amt)
    {
        inventory.SetCoins(inventory.GetCoins() + amt);
        onCurrencyValueChanged?.Invoke();
    }

    public void SubtractCoins(int amt)
    {
        inventory.SetCoins(inventory.GetCoins() - amt);
        onCurrencyValueChanged?.Invoke();
    }
}
