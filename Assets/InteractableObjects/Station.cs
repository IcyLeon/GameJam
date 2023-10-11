using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : InteractableObjects
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Transform WaypointContainer;
    private Transform[] PossibleWaypointTransform; 
    private int Income;

    private void Start()
    {
        Income = GetStartingIncome();
        PossibleWaypointTransform = WaypointContainer.GetComponentsInChildren<Transform>();
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

    public Transform[] GetAllWaypoints()
    {
        return PossibleWaypointTransform;
    }
    public int GetIncome()
    {
        return Income;
    }

    public override void SetitemsSO(ItemsSO itemsSO)
    {
        base.SetitemsSO(itemsSO);
        spriteRenderer.sprite = AssetManager.GetInstance().GetBoothSprite(GetItemsSO());
    }

}
