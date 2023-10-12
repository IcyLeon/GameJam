using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonSaveFile : MonoBehaviour
{
    private static JsonSaveFile instance;
    private string filePath;
    private LoadedData data;

    //[SerializeField] ItemsSO[] shopItemList;
    //[SerializeField] UpgradeItemSO[] upgradeItemShop;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/saveFile.txt";
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        data = new LoadedData();
        LoadPlayerData();
    }

    public static JsonSaveFile GetInstance()
    {
        return instance;
    }


    public int GetAmt()
    {
        return data.currencyAmt;
    }

    public List<ItemsSO> GetItemSOList()
    {
        return data.itemSOList;
    }

    public List<UpgradeItemSO> GetUpgradeItemSOList()
    {
        return data.upgradeItemSoList;
    }

    public void SavePlayerData()
    {
        SaveData newSaveData = new SaveData();

        InventoryManager im = InventoryManager.GetInstance();
        newSaveData.currencyAmt = im.GetCoins();
        List<ItemsSO> unlockedShopItems = im.GetItemsSOList();
        List<UpgradeItemSO> unlockedUpgradeItems = im.GetUpgradeItemsSOList();

        ItemsSO[] shopItemList = AssetManager.GetInstance().GetItemsSOList();
        UpgradeItemSO[] upgradeItemList = AssetManager.GetInstance().GetUpgradeItemsSOList();


        for (int i = 0; i < unlockedShopItems.Count; i++)
        {
            for (int x = 0; x < shopItemList.Length; x++)
            {
                if (unlockedShopItems[i] == shopItemList[x])
                {
                    newSaveData.unlockedShopItemSOindexList.Add(x);
                    break;
                }
            }
        }

        for (int i = 0; i < unlockedUpgradeItems.Count; i++)
        {
            for (int x = 0; x < upgradeItemList.Length; x++)
            {
                if (unlockedUpgradeItems[i] == upgradeItemList[x])
                {
                    newSaveData.unlockedUpgradeItemSOindexList.Add(x);
                    break;
                }
            }
        }

        // Create a new text file if the text file do not yet exist
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }

        string json = JsonUtility.ToJson(newSaveData);
        File.WriteAllText(filePath, json);

    }

    public void LoadPlayerData()
    {
        if (filePath == null)
            return;


        string json = File.ReadAllText(filePath);
        SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

        data.currencyAmt = loadedData.currencyAmt;
        ItemsSO[] shopItemList = AssetManager.GetInstance().GetItemsSOList();
        UpgradeItemSO[] upgradeItemShop = AssetManager.GetInstance().GetUpgradeItemsSOList();

        List<ItemsSO> unlockedShopItemList = new List<ItemsSO>();
        List<UpgradeItemSO> unlockedUpgradeItemList = new List<UpgradeItemSO>();

        for (int i = 0; i < loadedData.unlockedShopItemSOindexList.Count; i++)
        {
            data.itemSOList.Add(shopItemList[loadedData.unlockedShopItemSOindexList[i]]);
        }

        for (int i = 0; i < loadedData.unlockedUpgradeItemSOindexList.Count; i++)
        {
            data.upgradeItemSoList.Add(upgradeItemShop[loadedData.unlockedUpgradeItemSOindexList[i]]);
        }
    }

    [System.Serializable]
    private class SaveData
    {
        public int currencyAmt;
        public List<int> unlockedShopItemSOindexList = new List<int>();
        public List<int> unlockedUpgradeItemSOindexList = new List<int>();
    }


    private class LoadedData
    {
        public int currencyAmt;
        public List<ItemsSO> itemSOList = new List<ItemsSO>();
        public List<UpgradeItemSO> upgradeItemSoList = new List<UpgradeItemSO>();
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }
}