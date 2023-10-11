using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItem : MonoBehaviour
{
    [SerializeField] Image upgradeIcon;
    [SerializeField] TextMeshProUGUI upgradeName;
    [SerializeField] TextMeshProUGUI upgradeDescription;
    [SerializeField] Button purchaseButton;
    [SerializeField] TextMeshProUGUI costAmtDisplay;
    [SerializeField] GameObject coinIcon;

    [Header("TextColor")]
    [SerializeField] Color32 buyableColor;
    [SerializeField] Color32 notbuyableColor;

    private int itemCost;

    private void Start()
    {
        InventoryManager.GetInstance().onCurrencyValueChanged += AdjustDisplay;
        AdjustDisplay();
        purchaseButton.onClick.AddListener(Bought);
    }

    public void SetIcon(Sprite icon)
    {
        upgradeIcon.sprite = icon;
    }

    public void SetName(string name)
    {
        upgradeName.text = name;
    }

    public void SetDescription(string description)
    {
        upgradeDescription.text = description;
    }

    public void SetCost(int cost)
    {
        itemCost = cost;
        costAmtDisplay.text = AssetManager.GetInstance().AdjustCurrencyDisplay(itemCost);
    }

    private void AdjustDisplay()
    {
        if (InventoryManager.GetInstance().GetCoins() < itemCost)
            MakeNotBuyable();
        else
            MakeBuyable();
    }


    private void MakeBuyable()
    {
        purchaseButton.interactable = true;
        costAmtDisplay.color = buyableColor;
    }

    private void MakeNotBuyable()
    {
        purchaseButton.interactable = false;
        costAmtDisplay.color = notbuyableColor;
    }

    private void Bought()
    {
        purchaseButton.interactable = false;
        costAmtDisplay.color = buyableColor;
        costAmtDisplay.text = "BOUGHT";
        coinIcon.SetActive(false);
        InventoryManager im = InventoryManager.GetInstance();
        im.onCurrencyValueChanged -= AdjustDisplay;
        im.SubtractCoins(itemCost);
    }
}
