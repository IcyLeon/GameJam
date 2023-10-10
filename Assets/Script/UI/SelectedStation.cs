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
            NameTxt.text = assetManager.GetSelectedStation().GetItemsSO().ItemName;
        if (RarityTxt)
        {
            RarityTxt.text = assetManager.GetSelectedStation().GetItemsSO().GetRarityTxt();
            RarityTxt.color = assetManager.GetRarityColor(assetManager.GetSelectedStation().GetItemsSO().Rarity);
        }
        if (IncomeTxt)
            IncomeTxt.text = assetManager.GetSelectedStation().GetIncome().ToString();
    }
}
