using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public delegate void onButtonClick(ItemButton item, ItemsSO itemsSO);
    public onButtonClick SendItemButtonInfo;
    [SerializeField] Button SelectButton;
    [SerializeField] TextMeshProUGUI CostTxt;
    private int Cost;
    private ItemsSO itemsSO;

    // Start is called before the first frame update
    private void Start()
    {
        SelectButton.onClick.AddListener(ButtonClick);
        if (CostTxt)
            CostTxt.text = GetOriginalCost().ToString();
    }

    public int GetOriginalCost()
    {
        return Cost;
    }

    public void SetCost(int amt)
    {
        Cost = amt;
    }

    // Update is called once per frame
    private void ButtonClick()
    {
        SendItemButtonInfo?.Invoke(this, GetItemsSO());
    }

    public void SetItemSO(ItemsSO item)
    {
        itemsSO = item;
    }

    public ItemsSO GetItemsSO()
    {
        return itemsSO;
    }
}
