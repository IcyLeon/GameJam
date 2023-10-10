using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : InteractableObjects
{
    [SerializeField] SpriteRenderer spriteRenderer;
    private int Income;

    private void Start()
    {
        Income = GetStartingIncome();
        spriteRenderer.sprite = AssetManager.GetInstance().GetBoothSprite(GetItemsSO());
    }
    private int GetStartingIncome()
    {
        if (GetItemsSO() == null)
            return 0;

        return GetItemsSO().StartingIncome;
    }
    protected override void SelectedInfo()
    {
        AssetManager.GetInstance().SetSelectedStation(this);
    }

    public int GetIncome()
    {
        return Income;
    }

    public ItemsSO GetItemsSO()
    {
        return itemsSO;
    }
}
