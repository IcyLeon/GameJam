using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShopPurchaseSO", order = 1)]
public class ShopPurchaseSO : ScriptableObject
{
    public int Cost;
    public ItemsSO itemsSO;
}
