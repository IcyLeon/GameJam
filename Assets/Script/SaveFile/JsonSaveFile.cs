using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonSaveFile : MonoBehaviour
{
    [SerializeField] PlayerSaveFile playerData;

    private static JsonSaveFile instance;
    private string filePath;

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

        LoadPlayerData();
    }

    public static JsonSaveFile GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        SavePlayerData();
    }

    public void UpdateCurrency(int amt)
    {
        playerData.currencyAmt = amt;
    }

    public void UpdateShopItemList(List<ItemsSO> itemList)
    {
        playerData.UnlockItemsListSO = itemList;
    }

    public void UpdateUpgradeItemList(List<UpgradeItemSO> itemList)
    {
        playerData.UnlockUpgradeListSO = itemList;
    }

    public void SavePlayerData()
    {
        SaveData newSaveData = new SaveData();

        newSaveData.currencyAmt = playerData.currencyAmt;

        ItemsSO[] shopItemList = AssetManager.GetInstance().GetItemsSOList();
        UpgradeItemSO[] upgradeItemList = AssetManager.GetInstance().GetUpgradeItemsSOList();

        for (int i = 0; i < playerData.UnlockItemsListSO.Count; i++)
        {
            for (int x = 0; x < shopItemList.Length; x++)
            {
                if (playerData.UnlockItemsListSO[i] == shopItemList[x])
                {
                    newSaveData.unlockedShopItemSOindexList.Add(x);
                    break;
                }
            }
        }

        for (int i = 0; i < playerData.UnlockUpgradeListSO.Count; i++)
        {
            for (int x = 0; x < upgradeItemList.Length; x++)
            {
                if (playerData.UnlockUpgradeListSO[i] == upgradeItemList[x])
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

        ItemsSO[] shopItemList = AssetManager.GetInstance().GetItemsSOList();
        UpgradeItemSO[] upgradeItemShop = AssetManager.GetInstance().GetUpgradeItemsSOList();

        List<ItemsSO> unlockedShopItemList = new List<ItemsSO>();
        List<UpgradeItemSO> unlockedUpgradeItemList = new List<UpgradeItemSO>();

        playerData.currencyAmt = loadedData.currencyAmt;
        for (int i = 0; i < loadedData.unlockedShopItemSOindexList.Count; i++)
        {
            unlockedShopItemList.Add(shopItemList[loadedData.unlockedShopItemSOindexList[i]]);
        }
        playerData.UnlockItemsListSO = unlockedShopItemList;


        for (int i = 0; i < loadedData.unlockedUpgradeItemSOindexList.Count; i++)
        {
            unlockedUpgradeItemList.Add(upgradeItemShop[loadedData.unlockedUpgradeItemSOindexList[i]]);
        }
    }

    [System.Serializable]
    private class SaveData
    {
        public int currencyAmt;
        public List<int> unlockedShopItemSOindexList = new List<int>();
        public List<int> unlockedUpgradeItemSOindexList = new List<int>();
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }
}