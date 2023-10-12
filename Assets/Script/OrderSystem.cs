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
            if (i + 1 != AssetManager.GetInstance().GetBoothInfo().Length)
                AssetManager.GetInstance().GetBoothInfo()[i].station.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// The list that contains all the location for the flower booth
    /// </summary>
    private List<Station> BoothLocation = new List<Station>();

    /// <summary>
    /// Define the list of orders made by the customer. The player will automatically do the first one in the list.
    /// </summary>
    private List<OrderInformation> orderList = new List<OrderInformation>();

    /// <summary>
    /// A temporary place to store where to get the wrap
    /// </summary>
    [SerializeField] Transform wrapZone;


    private int currentOrderIncome;
    private bool runOnce = false;


    public void GenerateNewOrder(List<Item> itemList)
    {
        OrderInformation oi = new OrderInformation();
        for (int i = 0; i < itemList.Count; i++)
        {
            oi.SendItemList.Add(itemList[i]);
        }

        orderList.Add(oi);
        //FinalPriceCalculation.GetInstance().CalculateFinalPrice(oi);
    }

    /// <summary>
    /// Call this function to get the first item in the orderList. returns null if there is none
    /// </summary>
    /// <returns></returns>
    public OrderInformation GetOrder()
    {
        if (orderList.Count > 0)
        {
            if (!runOnce)
            {
                currentOrderIncome = FinalPriceCalculation.GetInstance().CalculateFinalPrice(orderList[0]);
                runOnce = true;
            }
            return orderList[0];
        }
        else
            return null;
    }

    public void DeleteOrderItem(Item item)
    {
        OrderInformation currentOrder = GetOrder();

        if (currentOrder != null)
            currentOrder.SendItemList.Remove(item);
    }
    public void DeleteOrder(OrderInformation order)
    {
        InventoryManager.GetInstance().AddCoins(currentOrderIncome);
        currentOrderIncome = 0;
        orderList.Remove(order);
        runOnce = false;
    }

    public int GetItemListPreparing(OrderInformation orderInformation)
    {
        if (orderInformation == null)
            return 0;

        return orderInformation.SendItemList.Count;
    }

    public Item GetFirstOrderItem()
    {
        if (GetOrder() == null)
            return null;

        if (GetOrder().SendItemList.Count == 0)
            return null;

        return GetOrder().SendItemList[0];
    }

    /// <summary>
    /// Return the position at which the flower type is at
    /// </summary>
    public Station GetBoothStation(Item item)
    {
        if (item == null)
            return null;

        for (int i = 0; i < BoothLocation.Count; i++)
        {
            if (BoothLocation[i].GetItemsSO() == item.GetItemsSO())
            {
                return BoothLocation[i];
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


    public void AddStationToList(Station station)
    {
        BoothLocation.Add(station);
    }
}

public class OrderInformation
{
    public List<Item> SendItemList = new List<Item>();
}