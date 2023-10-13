using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CoinsTxt;

    public void Start()
    {
        InventoryManager.GetInstance().onCurrencyValueChanged += onCurrencyChanged;
        onCurrencyChanged(0); // 0 is a dummy value
    }

    private void onCurrencyChanged(int value)
    {
        if (CoinsTxt)
            CoinsTxt.text = AssetManager.GetInstance().AdjustCurrencyDisplay(InventoryManager.GetInstance().GetCoins());
    }
}
