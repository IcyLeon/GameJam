using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private bool runOnce;

    // Start is called before the first frame update
    void Start()
    {
        runOnce = false;
        PurchaseBtn.onClick.AddListener(delegate
        {
            PurchaseInfo();
        }
        );
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

        AutoBuyLoadedFlowers();
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

    /// <summary>
    /// Purchase the item if the player can afford it. Ignore the check and subtracting of coins if its loaded in.
    /// </summary>
    void PurchaseInfo(bool loadedIn = false)
    {
        if (isPurchaseable(currentShopItemButtonSelected) || loadedIn)
        {
            currentShopItemButtonSelected.Purchased();

            if (!loadedIn)
                InventoryManager.GetInstance().SubtractCoins(currentShopItemButtonSelected.GetOriginalCost());

            if (GetComponent<TabGroup>().selectedTab.shopType == SHOP_TYPE.FLOWER)
            {
                Station station = AssetManager.GetInstance().GetStation(currentShopItemButtonSelected.GetItemsSO());
                station.SetitemsSO(currentShopItemButtonSelected.GetItemsSO());
                station.gameObject.SetActive(true);
                OrderSystem.GetInstance().AddStationToList(station);
                InventoryManager.GetInstance().AddFlowersSO(currentShopItemButtonSelected.GetItemsSO());

                //OrderSystem.GetInstance().AddStationToList(station);
            }
            else
            {
                if (!runOnce)
                {
                    Station station = AssetManager.GetInstance().GetStation(currentShopItemButtonSelected.GetItemsSO());
                    station.SetitemsSO(currentShopItemButtonSelected.GetItemsSO());
                    OrderSystem.GetInstance().AddStationToList(station);
                    runOnce = true;
                }
                InventoryManager.GetInstance().AddWrappersSO(currentShopItemButtonSelected.GetItemsSO());
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

    /// <summary>
    /// Autobuy rose and 1st wrapper. Also load in flowers that the player unlocked from the save file
    /// </summary>
    void AutoBuyLoadedFlowers()
    {
        ShopItemButton[] shopItemList = GetComponentsInChildren<ShopItemButton>();
        ItemsSO rose = AssetManager.GetInstance().GetFlowerItemSOList()[0];
        ItemsSO wrapper = AssetManager.GetInstance().GetWrapperItemSOList()[0];

        for (int i = 0; i < shopItemList.Length; i++)
        {
            if (shopItemList[i].GetItemsSO() == rose)
            {
                GetComponent<TabGroup>().selectedTab.shopType = SHOP_TYPE.FLOWER;
                currentShopItemButtonSelected = shopItemList[i];
                PurchaseInfo(true);
                currentShopItemButtonSelected.Purchased();
            }

            else if (shopItemList[i].GetItemsSO() == wrapper)
            {
                GetComponent<TabGroup>().selectedTab.shopType = SHOP_TYPE.WRAPPER;
                currentShopItemButtonSelected = shopItemList[i];
                PurchaseInfo(true);
                currentShopItemButtonSelected.Purchased();
            }
        }

        if (JsonSaveFile.GetInstance() != null)
        {
            List<ItemsSO> flowerSOList = JsonSaveFile.GetInstance().GetFlowerItemSoList();
            List<ItemsSO> wrapperSOList = JsonSaveFile.GetInstance().GetWrapperItemSoList();

            GetComponent<TabGroup>().selectedTab.shopType = SHOP_TYPE.FLOWER;
            for (int i = 0; i < flowerSOList.Count; i++)
            {
                for (int x = 0; x < shopItemList.Length; x++)
                {
                    if (flowerSOList[i] == shopItemList[x].GetItemsSO() && flowerSOList[i] != rose)
                    {
                        currentShopItemButtonSelected = shopItemList[x];
                        PurchaseInfo(true);
                        currentShopItemButtonSelected.Purchased();
                        break;
                    }
                }
            }

            GetComponent<TabGroup>().selectedTab.shopType = SHOP_TYPE.WRAPPER;
            for (int i = 0; i < wrapperSOList.Count; i++)
            {
                for (int x = 0; x < shopItemList.Length; x++)
                {
                    if (wrapperSOList[i] == shopItemList[x].GetItemsSO() && wrapperSOList[i] != wrapper)
                    {
                        currentShopItemButtonSelected = shopItemList[x];
                        PurchaseInfo(true);
                        currentShopItemButtonSelected.Purchased();
                        break;
                    }
                }
            }

            currentShopItemButtonSelected = null;
        }
    }
}

public enum SHOP_TYPE
{
    FLOWER,
    WRAPPER,
}