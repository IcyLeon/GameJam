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

    private void OnEnable()
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

    public List<ItemsSO> GetFlowerItemSoList()
    {
        return data.floweritemSOList;
    }

    public List<ItemsSO> GetWrapperItemSoList()
    {
        return data.wrapperitemSOList;
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
        List<ItemsSO> unlockedFlowersSO = im.GetFlowersSOList();
        List<ItemsSO> unlockedWrappersSO = im.GetWrappersSOList();
        List<UpgradeItemSO> unlockedUpgradeItems = im.GetUpgradeItemsSOList();

        ItemsSO[] flowerList = AssetManager.GetInstance().GetFlowerItemSOList();
        ItemsSO[] wrapperList = AssetManager.GetInstance().GetWrapperItemSOList();
        UpgradeItemSO[] upgradeItemList = AssetManager.GetInstance().GetUpgradeItemsSOList();


        for (int i = 0; i < unlockedFlowersSO.Count; i++)
        {
            for (int x = 0; x < flowerList.Length; x++)
            {
                if (unlockedFlowersSO[i] == flowerList[x])
                {
                    newSaveData.unlockedFlowerItemSOindexList.Add(x);
                    break;
                }
            }
        }

        for (int i = 0; i < unlockedWrappersSO.Count; i++)
        {
            for (int x = 0; x < wrapperList.Length; x++)
            {
                if (unlockedWrappersSO[i] == wrapperList[x])
                {
                    newSaveData.unlockedWrapperItemSOindexList.Add(x);
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
        ItemsSO[] flowerItemList = AssetManager.GetInstance().GetFlowerItemSOList();
        ItemsSO[] wrapperItemList = AssetManager.GetInstance().GetWrapperItemSOList();
        UpgradeItemSO[] upgradeItemShop = AssetManager.GetInstance().GetUpgradeItemsSOList();

        List<ItemsSO> unlockedFlowerItemList = new List<ItemsSO>();
        List<ItemsSO> unlockedWrapperItemList = new List<ItemsSO>();
        List<UpgradeItemSO> unlockedUpgradeItemList = new List<UpgradeItemSO>();

        for (int i = 0; i < loadedData.unlockedFlowerItemSOindexList.Count; i++)
        {
            data.floweritemSOList.Add(flowerItemList[loadedData.unlockedFlowerItemSOindexList[i]]);
        }

        for (int i = 0; i < loadedData.unlockedWrapperItemSOindexList.Count; i++)
        {
            data.wrapperitemSOList.Add(wrapperItemList[loadedData.unlockedWrapperItemSOindexList[i]]);
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
        public List<int> unlockedFlowerItemSOindexList = new List<int>();
        public List<int> unlockedWrapperItemSOindexList = new List<int>();
        public List<int> unlockedUpgradeItemSOindexList = new List<int>();
    }


    private class LoadedData
    {
        public int currencyAmt;
        public List<ItemsSO> floweritemSOList = new List<ItemsSO>();
        public List<ItemsSO> wrapperitemSOList = new List<ItemsSO>();
        public List<UpgradeItemSO> upgradeItemSoList = new List<UpgradeItemSO>();
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }
}