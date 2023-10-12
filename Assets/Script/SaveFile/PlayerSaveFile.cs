using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/PlayerSave")]
public class PlayerSaveFile : ScriptableObject
{
    public int currencyAmt;
    public List<ItemsSO> UnlockItemsListSO;
    public List<UpgradeItemSO> UnlockUpgradeListSO;
}
