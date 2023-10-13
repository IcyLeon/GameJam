using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedStation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NameTxt, RarityTxt, IncomeTxt;
    [SerializeField] RectTransform RT;
    [SerializeField] GameObject Content;
    private AssetManager assetManager;

    private void Start()
    {
        assetManager = AssetManager.GetInstance();
        assetManager.onSelectedClick += OnStationSelect;
    }

    public void Update()
    {

    }
    public void OnStationSelect()
    {
        Vector3 ScreenPos = Camera.main.WorldToScreenPoint(assetManager.GetSelectedStation().transform.position + Vector3.up * 2f);
        RT.position = ScreenPos;
        Content.SetActive(true);

        if (NameTxt)
        {
            if (assetManager.GetSelectedStation().GetItemsSO() != InventoryManager.GetInstance().GetWrappersSOList()[0])
                NameTxt.text = assetManager.GetSelectedStation().GetItemsSO().ItemName;
            else
                NameTxt.text = InventoryManager.GetInstance().GetMostMultiplerWrapper().ItemName;

        }

        if (RarityTxt)
        {
            if (assetManager.GetSelectedStation().GetItemsSO() != InventoryManager.GetInstance().GetWrappersSOList()[0])
            {
                RarityTxt.text = assetManager.GetSelectedStation().GetItemsSO().GetRarityTxt();
                RarityTxt.color = assetManager.GetRarityColor(assetManager.GetSelectedStation().GetItemsSO().Rarity);
            }
            else
            {
                RarityTxt.text = InventoryManager.GetInstance().GetMostMultiplerWrapper().GetRarityTxt();
                RarityTxt.color = assetManager.GetRarityColor(InventoryManager.GetInstance().GetMostMultiplerWrapper().Rarity);
            }
        }

        if (IncomeTxt)
        {
            if (assetManager.GetSelectedStation().GetItemsSO() != InventoryManager.GetInstance().GetWrappersSOList()[0])
                IncomeTxt.text = FinalPriceCalculation.GetInstance().CalculateModifierApplied(assetManager.GetSelectedStation().GetItemsSO()).ToString();
            else
                IncomeTxt.text = "X" + InventoryManager.GetInstance().GetMostMultiplerWrapper().multipler.ToString();
        }

    }
}
