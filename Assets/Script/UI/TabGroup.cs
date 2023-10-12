using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField] TabButton[] tabButtonList;
    public TabButton selectedTab;

    private void Start()
    {
        ResetTabs();
    }

    public void TabClicked(TabButton button)
    {
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
}
