using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ShopPurchaseSO[] StartUpShopList;
    [SerializeField] Transform ShopContentParent;
    [SerializeField] GameObject ShopItemPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < StartUpShopList.Length; i++)
        {
            GameObject go = Instantiate(ShopItemPrefab, ShopContentParent);
            ItemButton itemButton = go.GetComponent<ItemButton>();
            itemButton.SetItemSO(StartUpShopList[i].itemsSO);
            itemButton.SetCost(StartUpShopList[i].Cost);
        }
    }


}
