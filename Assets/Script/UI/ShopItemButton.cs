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
    private bool PurchaseStatus;
    [SerializeField] TextMeshProUGUI CostTxt;
    [SerializeField] Image ItemImage;

    void Start()
    {
        PurchaseStatus = false;
        if (CostTxt)
            CostTxt.text = GetOriginalCost().ToString();
        if (ItemImage)
            ItemImage.sprite = GetItemsSO().ItemSprite;
    }

    public void Purchased()
    {
        PurchaseStatus = true;
        canvasGroup.alpha = 0.5f;
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
