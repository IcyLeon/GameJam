using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : InteractableObjects
{
    [SerializeField] SpriteRenderer spriteRenderer;
    private FlowerTypes flowerType;
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

    /// <summary>
    /// Set The flowerType of the station
    /// </summary>
    public void SetFlowerType(FlowerTypes flowerType)
    {
        this.flowerType = flowerType;
    }

    /// <summary>
    /// return the type of flower that the booth contains
    /// </summary>
    /// <returns></returns>
    public FlowerTypes GetFlowerType()
    {
        return flowerType;
    }
}
