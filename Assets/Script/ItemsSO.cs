using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemsSO", order = 1)]
public class ItemsSO : ScriptableObject
{
    public string ItemName;
    public Rarity Rarity = Rarity.COMMON;
    public Sprite ItemSprite;
    public int StartingIncome;
    public FlowerTypes flowerType;
    public Vector3 pos;
    [TextAreaAttribute]
    public string ItemDescription;

    public string GetRarityTxt()
    {
        switch(Rarity)
        {
            case Rarity.COMMON:
                return "Common";
            case Rarity.UNCOMMON:
                return "Uncommon";
        }
        return "???";
    }
}
