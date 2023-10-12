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
    private UpgradeItemSO upgradeItemSO;

    private int itemCost;

    private void Start()
    {
        InventoryManager.GetInstance().onCurrencyValueChanged += AdjustDisplay;
        AdjustDisplay();
        purchaseButton.onClick.AddListener(Bought);
    }

    public void SetUpgradeItemSO(UpgradeItemSO theUpgradeItemSO)
    {
        upgradeItemSO = theUpgradeItemSO;
        SetIcon(upgradeItemSO.itemSprite);
        SetName(upgradeItemSO.itemName);
        SetDescription(upgradeItemSO.itemDescription);
        SetCost(upgradeItemSO.Cost);
    }

    private void SetIcon(Sprite icon)
    {
        upgradeIcon.sprite = icon;
    }

    private void SetName(string name)
    {
        upgradeName.text = name;
    }

    private void SetDescription(string description)
    {
        upgradeDescription.text = description;
    }

    private void SetCost(int cost)
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
        im.AddUpgradeSO(upgradeItemSO);
    }
}
