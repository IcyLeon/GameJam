using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjects : MonoBehaviour
{
    [SerializeField] Button InteractButton;
    private ItemsSO itemsSO;

    protected virtual void Awake()
    {
        InteractButton.onClick.AddListener(SelectedInfo);
    }

    public virtual void SetitemsSO(ItemsSO itemsSO)
    {
        this.itemsSO = itemsSO;
    }
    protected virtual void SelectedInfo()
    {
    }

    public ItemsSO GetItemsSO()
    {
        return itemsSO;
    }
}
