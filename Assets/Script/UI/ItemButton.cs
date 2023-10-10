using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour, IPointerClickHandler
{
    public delegate void onButtonClick(ItemButton item, ItemsSO itemsSO);
    public onButtonClick SendItemButtonInfo;
    private ItemsSO itemsSO;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        ButtonClick();
    }
}
