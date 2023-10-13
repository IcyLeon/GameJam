using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemsSO", order = 1)]
public class ItemsSO : ScriptableObject
{
    public string ItemName;
    public Rarity Rarity = Rarity.COMMON;
    public Sprite ItemSprite;
    /// <summary>
    /// The original income that the flower will give. Ignore this if the itemSO is a wrapper
    /// </summary>
    public int StartingIncome;
    /// <summary>
    /// The multipler that the wrapper will give. Ignore this if the itemSO is a flower
    /// </summary>
    public float multipler;
    [TextAreaAttribute]
    public string ItemDescription;

    public Sprite wrapperEnvironmentSprite;

    public string GetRarityTxt()
    {
        switch(Rarity)
        {
            case Rarity.COMMON:
                return "Common";
            case Rarity.UNCOMMON:
                return "Uncommon";
            case Rarity.RARE:
                return "Rare";
        }
        return "???";
    }
}
