using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager instance;

    public UpgradeManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    private UpgradeItemSO[] upgradeItemList;
    [SerializeField] Transform UpgradeContentParent;
    [SerializeField] GameObject upgradeItemButton;

    private void Start()
    {
        upgradeItemList = AssetManager.GetInstance().GetUpgradeItemsSOList();
        for (int i = 0; i < upgradeItemList.Length; i++)
        {
            GameObject go = Instantiate(upgradeItemButton, UpgradeContentParent);
            UpgradeItem ui = go.GetComponent<UpgradeItem>();
            ui.SetUpgradeItemSO(upgradeItemList[i]);
        }

        if (JsonSaveFile.GetInstance() != null)
        {

        }
    }


}
