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

    /// <summary>
    /// The list that contains all the location for the flower booth
    /// </summary>
    private List<Station> flowerBoothLocation = new List<Station>();

    /// <summary>
    /// Define the list of orders made by the customer. The player will automatically do the first one in the list.
    /// </summary>
    [SerializeField] List<OrderInformation> orderList = new List<OrderInformation>();

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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OrderInformation newOrder = new OrderInformation();
            newOrder.flower1 = FlowerTypes.ROSE;
            newOrder.flower2 = FlowerTypes.ROSE;
            newOrder.flower3 = FlowerTypes.DAISY;
            tempPlayer.GiveOrder(newOrder);
        }
    }

    /// <summary>
    /// Return the position at which the flower type is at
    /// </summary>
    public Vector3 GetFlowerBoothLocation(FlowerTypes flowerType)
    {
        for (int i = 0; i < flowerBoothLocation.Count; i++)
        {
            if (flowerBoothLocation[i].GetItemsSO().flowerType == flowerType)
            {
                return flowerBoothLocation[i].transform.position;
            }
        }

        return Vector3.zero;
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
    public FlowerTypes flower1;
    public FlowerTypes flower2;
    public FlowerTypes flower3;
    public WrapTypes wrap;
}