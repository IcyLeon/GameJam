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
    private bool bought;

    private int itemCost;

    private void Start()
    {
        InventoryManager.GetInstance().onCurrencyValueChanged += AdjustDisplay;
        AdjustDisplay(0); // 0 is a dummy parameter for now
        purchaseButton.onClick.AddListener(delegate
        {
            Bought();
            }
        );
        bought = false;
    }

    /// <summary>
    /// DO NOT USE THIS METHOD OTHER THAN WHEN LOADING IT IN!
    /// </summary>
    public void BuyItem()
    {
        Bought(true);
    }

    public void SetUpgradeItemSO(UpgradeItemSO theUpgradeItemSO)
    {
        upgradeItemSO = theUpgradeItemSO;
        SetIcon(upgradeItemSO.itemSprite);
        SetName(upgradeItemSO.itemName);
        SetDescription(upgradeItemSO.itemDescription);
        SetCost(upgradeItemSO.Cost);
    }

    public UpgradeItemSO GetItemSO()
    {
        return upgradeItemSO;
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

    private void AdjustDisplay(int value)
    {
        if (InventoryManager.GetInstance().GetCoins() < itemCost && !bought)
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

    private void Bought(bool LoadIn = false)
    {
        bought = true;
        purchaseButton.interactable = false;
        costAmtDisplay.color = buyableColor;
        costAmtDisplay.text = "BOUGHT";
        coinIcon.SetActive(false);
        InventoryManager im = InventoryManager.GetInstance();
        im.onCurrencyValueChanged -= AdjustDisplay;

        if (!LoadIn)
            im.SubtractCoins(itemCost);

        im.AddUpgradeSO(upgradeItemSO);
    }
}
