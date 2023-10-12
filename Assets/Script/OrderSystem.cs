using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    private static OrderSystem instance;

    public static OrderSystem GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < AssetManager.GetInstance().GetBoothInfo().Length; i++)
        {
            AssetManager.GetInstance().GetBoothInfo()[i].station.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// The list that contains all the location for the flower booth
    /// </summary>
    private List<Station> flowerBoothLocation = new List<Station>();

    /// <summary>
    /// Define the list of orders made by the customer. The player will automatically do the first one in the list.
    /// </summary>
    private List<OrderInformation> orderList = new List<OrderInformation>();

    /// <summary>
    /// A temporary place to store where to get the wrap
    /// </summary>
    [SerializeField] Transform wrapZone;

    /// <summary>
    /// A temporary place to store where to fulfill the order
    /// </summary>
    [SerializeField] Transform counterArea;

    /// <summary>
    /// Temporary get reference to the player
    /// </summary>
    [SerializeField] Player tempPlayer;

    public void GenerateNewOrder(List<FlowerTypes> flowerTypeList)
    {
        OrderInformation oi = new OrderInformation();
        oi.wrap = WrapTypes.WRAP_1;
        for (int i = 0; i < flowerTypeList.Count; i++)
        {
            oi.flowerTypeList.Add(flowerTypeList[i]);
        }

        orderList.Add(oi);
        FinalPriceCalculation.GetInstance().CalculateFinalPrice(oi);
    }


    /// <summary>
    /// Call this function to get the first item in the orderList. returns null if there is none
    /// </summary>
    /// <returns></returns>
    public OrderInformation GetOrder()
    {
        if (orderList.Count > 0)
            return orderList[0];
        else
            return null;
    }

    /// <summary>
    /// Return the position at which the flower type is at
    /// </summary>
    public Station GetFlowerBoothStation(FlowerTypes flowerType)
    {
        for (int i = 0; i < flowerBoothLocation.Count; i++)
        {
            if (flowerBoothLocation[i].GetItemsSO().flowerType == flowerType)
            {
                return flowerBoothLocation[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Get the position of the wrapping zone
    /// </summary>
    /// <returns></returns>
    public Vector3 GetWrapLocation()
    {
        return wrapZone.position;
    }

    /// <summary>
    /// Get the position of the wrapping zone
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCounterLocation()
    {
        return counterArea.position;
    }


    public void AddStationToList(Station station)
    {
        flowerBoothLocation.Add(station);
    }
}

public class OrderInformation
{
    public List<FlowerTypes> flowerTypeList = new List<FlowerTypes>();
    public WrapTypes wrap;
}