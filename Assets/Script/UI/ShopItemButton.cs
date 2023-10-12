using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : ItemButton
{
    [SerializeField] CanvasGroup canvasGroup;
    private GameObject StationPrefab;
    private int Cost;
    private bool PurchaseStatus = false;
    [SerializeField] TextMeshProUGUI CostTxt;
    [SerializeField] Image ItemImage;

    void Start()
    {
        if (CostTxt)
            CostTxt.text = AssetManager.GetInstance().AdjustCurrencyDisplay(GetOriginalCost());
        if (ItemImage)
            ItemImage.sprite = GetItemsSO().ItemSprite;
    }

    public void Purchased()
    {
        PurchaseStatus = true;
        canvasGroup.alpha = 1.0f;
    }

    public bool isPurchased()
    {
        return PurchaseStatus;
    }

    public int GetOriginalCost()
    {
        return Cost;
    }

    public void SetCost(int amt)
    {
        Cost = amt;
    }

    public void SetStationPrefab(GameObject StationPrefab)
    {
        this.StationPrefab = StationPrefab;
    }

    public GameObject GetStationPrefab()
    {
        return StationPrefab;
    }
}
