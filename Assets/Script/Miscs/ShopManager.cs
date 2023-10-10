using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ShopPurchaseSO[] StartUpShopList;
    [SerializeField] Button PurchaseBtn;
    [SerializeField] Transform ShopContentParent;
    [SerializeField] GameObject ShopItemPrefab;
    private ShopItemButton currentShopItemButtonSelected;

    [Header("Display Item Infomation")]
    [SerializeField] GameObject DescriptionContent;
    [SerializeField] TextMeshProUGUI TitleTxt, DescTxt;

    // Start is called before the first frame update
    void Awake()
    {
        PurchaseBtn.onClick.AddListener(PurchaseInfo);
        for (int i = 0; i < StartUpShopList.Length; i++)
        {
            GameObject go = Instantiate(ShopItemPrefab, ShopContentParent);
            ShopItemButton shopitemButton = go.GetComponent<ShopItemButton>();
            shopitemButton.SetItemSO(StartUpShopList[i].itemsSO);
            shopitemButton.SetCost(StartUpShopList[i].Cost);
            shopitemButton.SetStationPrefab(AssetManager.GetInstance().StationPrefab);
            shopitemButton.SendItemButtonInfo += onItemButtonSend;
        }
    }

    void onItemButtonSend(ItemButton item, ItemsSO itemsSO)
    {
        ShopItemButton shopItemButton = item as ShopItemButton;
        currentShopItemButtonSelected = shopItemButton;

        if (item != null)
        {
            TitleTxt.text = shopItemButton.GetItemsSO().ItemName;
            DescTxt.text = shopItemButton.GetItemsSO().ItemDescription;
        }
        DescriptionContent.SetActive(item != null);
        PurchaseBtn.gameObject.SetActive(shopItemButton != null && !shopItemButton.isPurchased());
    }

    void PurchaseInfo()
    {
        if (isPurchaseable(currentShopItemButtonSelected))
        {
            currentShopItemButtonSelected.Purchased();
            InventoryManager.GetInstance().SubtractCoins(currentShopItemButtonSelected.GetOriginalCost());
            GameObject go = Instantiate(currentShopItemButtonSelected.GetStationPrefab());
            go.GetComponent<Station>().SetitemsSO(currentShopItemButtonSelected.GetItemsSO());
            go.GetComponent<Station>().SetFlowerType(currentShopItemButtonSelected.GetItemsSO().flowerType);
            go.transform.position = currentShopItemButtonSelected.GetItemsSO().pos;
            OrderSystem.instance.AddStationToList(go);
        }
        PurchaseBtn.gameObject.SetActive(currentShopItemButtonSelected != null && !currentShopItemButtonSelected.isPurchased());
    }

    private bool isPurchaseable(ShopItemButton shopItemButton)
    {
        if (shopItemButton == null)
            return false;

        return InventoryManager.GetInstance().GetCoins() >= shopItemButton.GetOriginalCost();
    }

}
