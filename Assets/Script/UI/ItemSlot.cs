using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image ItemImage;
    [SerializeField] TextMeshProUGUI AmountTxt;

    // Start is called before the first frame update
    public void UpdateContent(Sprite sprite, string Amt)
    {
        if (ItemImage)
            ItemImage.sprite = sprite;
        if (AmountTxt)
            AmountTxt.text = Amt;
    }
}
