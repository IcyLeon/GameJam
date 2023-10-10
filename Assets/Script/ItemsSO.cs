using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    COMMON,
    UNCOMMON
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemsSO", order = 1)]
public class ItemsSO : ScriptableObject
{
    public string ItemName;
    public Rarity Rarity = Rarity.COMMON;
    public Sprite ItemSprite;
    public int StartingIncome;
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
