using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TabGroup : MonoBehaviour
{
    [SerializeField] TabButton[] tabButtonList;
    [SerializeField] TextMeshProUGUI shopNameDisplay;
    public TabButton selectedTab;

    private void Start()
    {
        ResetTabs();
        shopNameDisplay.text = tabButtonList[0].GetNameDisplay();
    }

    public void TabClicked(TabButton button)
    {
        shopNameDisplay.text = button.GetNameDisplay();
        selectedTab = button;
        ResetTabs();
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtonList)
        {
            if (button != selectedTab)
                button.SetInactive();
            else
                button.SetActive();
        }
    }

    /// <summary>
    /// Force the tab group to refer to a flower instance. DO NOT USE THIS UNLESS U KNOW WHAT U DOING!
    /// </summary>
    public void ForceToFlower()
    {
        selectedTab = tabButtonList[0];
    }

    /// <summary>
    /// Force the tab group to refer to a wrapper instance. DO NOT USE THIS UNLESS U KNOW WHAT U DOING!
    /// </summary>
    public void ForceToWrapper()
    {
        selectedTab = tabButtonList[1];
    }
}
