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
    }

    [SerializeField] UpgradeItemSO[] upgradeItemList;
    [SerializeField] Transform UpgradeContentParent;
    [SerializeField] GameObject upgradeItemButton;

    private void Start()
    {
        for (int i = 0; i < upgradeItemList.Length; i++)
        {
            GameObject go = Instantiate(upgradeItemButton, UpgradeContentParent);
            UpgradeItem ui = go.GetComponent<UpgradeItem>();
            ui.SetIcon(upgradeItemList[i].itemSprite);
            ui.SetName(upgradeItemList[i].itemName);
            ui.SetDescription(upgradeItemList[i].itemDescription);
            ui.SetCost(upgradeItemList[i].Cost);
        }
    }
}
