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
    public Rarity Rarity;
    public Sprite ItemSprite;
}
