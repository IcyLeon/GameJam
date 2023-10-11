using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class TabButton : MonoBehaviour
{
    public TabGroup tabgroup;

    public SHOP_TYPE shopType;
    public GameObject itemContent;
    public Button itemTabButton;

    public void ButtonClicked()
    {
        tabgroup.TabClicked(this);
    }

    public void SetInactive()
    {
        itemContent.gameObject.SetActive(false);
        itemTabButton.interactable = true;
    }

    public void SetActive()
    {
        itemContent.gameObject.SetActive(true);
        itemTabButton.interactable = false;
    }
}
