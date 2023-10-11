using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    [Header("Flower Shop Tab")]
    [SerializeField] ShopPurchaseSO[] FlowerShopList;
    [SerializeField] Transform FlowerShopContentParent;
    [SerializeField] Button flowerShopTabButton;

    [Header("Wrapper Shop Tab")]
    [SerializeField] ShopPurchaseSO[] WrapperShopList;
    [SerializeField] Transform WrapperShopContentParent;
    [SerializeField] Button wrapperShopTabButton;

    [Header("Other Shop Related Reference")]
    [SerializeField] Button PurchaseBtn;
    [SerializeField] TextMeshProUGUI shopTypeDisplay;
    [SerializeField] GameObject ShopItemPrefab;
    [SerializeField] GameObject PurchaseFailedPopUp;
    private ShopItemButton currentShopItemButtonSelected;

    [Header("Display Item Infomation")]
    [SerializeField] GameObject DescriptionContent;
    [SerializeField] TextMeshProUGUI TitleTxt, DescTxt;

    // Start is called before the first frame update
    void Awake()
    {
        PurchaseBtn.onClick.AddListener(PurchaseInfo);
        // Create the flower shop item in the flowerShop list
        for (int i = 0; i < FlowerShopList.Length; i++)
        {
            GameObject go = Instantiate(ShopItemPrefab, FlowerShopContentParent);
            ShopItemButton shopitemButton = go.GetComponent<ShopItemButton>();
            shopitemButton.SetItemSO(FlowerShopList[i].itemsSO);
            shopitemButton.SetCost(FlowerShopList[i].Cost);
            shopitemButton.SetStationPrefab(AssetManager.GetInstance().StationPrefab);
            shopitemButton.SendItemButtonInfo += onItemButtonSend;
        }

        // Create the wrapper shop item in the wrapperShop list
        for (int i = 0; i < WrapperShopList.Length; i++)
        {
            GameObject go = Instantiate(ShopItemPrefab, WrapperShopContentParent);
            ShopItemButton shopitemButton = go.GetComponent<ShopItemButton>();
            shopitemButton.SetItemSO(WrapperShopList[i].itemsSO);
            shopitemButton.SetCost(WrapperShopList[i].Cost);
            shopitemButton.SetStationPrefab(AssetManager.GetInstance().StationPrefab);
            shopitemButton.SendItemButtonInfo += onItemButtonSend;
        }

        gameObject.SetActive(false);
    }

    // action
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

    // core
    void PurchaseInfo()
    {
        if (isPurchaseable(currentShopItemButtonSelected))
        {
            currentShopItemButtonSelected.Purchased();
            InventoryManager.GetInstance().SubtractCoins(currentShopItemButtonSelected.GetOriginalCost());

            if (GetComponent<TabGroup>().selectedTab.shopType == SHOP_TYPE.FLOWER)
            {
                GameObject go = Instantiate(currentShopItemButtonSelected.GetStationPrefab());
                Station station = go.GetComponent<Station>();
                station.SetitemsSO(currentShopItemButtonSelected.GetItemsSO());

                //OrderSystem.GetInstance().AddStationToList(station);
            }
        }
        else
        {
            SetPopUpActive(true);
        }
        PurchaseBtn.gameObject.SetActive(currentShopItemButtonSelected != null && !currentShopItemButtonSelected.isPurchased());
    }

    // core
    private bool isPurchaseable(ShopItemButton shopItemButton)
    {
        if (shopItemButton == null)
            return false;

        return InventoryManager.GetInstance().GetCoins() >= shopItemButton.GetOriginalCost();
    }

    /// <summary>
    /// Set the popup active or inactive depending on the boolean value supplied
    /// </summary>
    public void SetPopUpActive(bool active)
    {
        PurchaseFailedPopUp.SetActive(active);
    }
}

public enum SHOP_TYPE
{
    FLOWER,
    WRAPPER,
}