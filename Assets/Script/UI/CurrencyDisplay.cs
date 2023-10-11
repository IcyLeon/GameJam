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
        onCurrencyChanged();
    }

    private void onCurrencyChanged()
    {
        if (CoinsTxt)
            CoinsTxt.text = AssetManager.GetInstance().AdjustCurrencyDisplay(InventoryManager.GetInstance().GetCoins());
    }
}
