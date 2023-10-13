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
    public GameObject scrollViewRect;
    public Button itemTabButton;
    [SerializeField] string shopNameDisplay;

    public void ButtonClicked()
    {
        tabgroup.TabClicked(this);
    }

    public void SetInactive()
    {
        scrollViewRect.gameObject.SetActive(false);
        itemTabButton.interactable = true;
    }

    public void SetActive()
    {
        scrollViewRect.gameObject.SetActive(true);
        itemTabButton.interactable = false;
    }

    public string GetNameDisplay()
    {
        return shopNameDisplay;
    }
}
