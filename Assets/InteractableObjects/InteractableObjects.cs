using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjects : MonoBehaviour
{
    [SerializeField] Button InteractButton;
    protected ItemsSO itemsSO;

    protected virtual void Awake()
    {
        InteractButton.onClick.AddListener(SelectedInfo);
    }

    public void SetitemsSO(ItemsSO itemsSO)
    {
        this.itemsSO = itemsSO;
    }
    protected virtual void SelectedInfo()
    {
    }
}
