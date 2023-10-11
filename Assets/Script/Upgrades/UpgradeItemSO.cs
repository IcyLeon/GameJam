using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/UpgradeItemSO")]
public class UpgradeItemSO : ScriptableObject
{
    public string itemName;
    [TextAreaAttribute]
    public string itemDescription;
    public Sprite itemSprite;
    public int Cost;
    public float multiplerIfAny = 1.0f;
}
