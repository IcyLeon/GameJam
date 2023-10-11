using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MoveableObjects
{
    // Timer
    [SerializeField] GameObject sliderReference;
    [SerializeField] Slider timerSlider;
    [SerializeField] float maxTimer = 1.0f;

    //private OrderInformation orderToFollow;
    //private OrderInformation currentOrder;
    private PlayerState playerState;
    private Vector3 targetPos;


    private enum PlayerState
    {
        IDLE,
        COLLECTING,
        GETFLOWER1,
        GETFLOWER2,
        GETFLOWER3,
        GETWRAP,
        MOVETOCOUNTER,
    }    

    private void Start()
    {
        playerState = PlayerState.IDLE;
        sliderReference.SetActive(false);
    } 



    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MapManager map = MapManager.GetInstance();
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            Vector2Int CurrentPos = new Vector2Int(map.GetMainTileMap().WorldToCell(transform.position).x, map.GetMainTileMap().WorldToCell(transform.position).y);
            Vector2Int EndPos = new Vector2Int(map.GetMainTileMap().WorldToCell(pos).x, map.GetMainTileMap().WorldToCell(pos).y);
            StartCoroutine(MovePlayer(CurrentPos, EndPos));
        }
    }

    IEnumerator MovePlayer(Vector2Int Current, Vector2Int EndPos)
    {
        var path = MapManager.GetInstance().AStarPathFinding(Current, EndPos);
        float distanceThreshold = 0.1f; // Adjust this value as needed

        while (path.Count > 0)
        {
            Vector3 targetPosition = path[0].transform.position;
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance > distanceThreshold)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10f);
            }
            else
            {
                path.RemoveAt(0);
            }
            yield return null;
        }
    }


    //// give the player an order to do. The player will automatically fetch the required items and give it to the counter
    //public void GiveOrder(OrderInformation newOrder)
    //{
    //    // CLear out all the orders and assigned it to OrderToFollow
    //    orderToFollow = newOrder;
    //    currentOrder = null;
    //    currentOrder = new OrderInformation();
    //    startGathering = true;
    //}

    /// <summary>
    /// Update the currentOrder class to match some of the ordertofollow class
    /// </summary>
    void GetFlower(FlowerTypes flowerToGet, int whichFlower)
    {
        //switch (whichFlower)
        //{
        //    case 1:
        //        currentOrder.flower1 = flowerToGet;
        //        break;
        //    case 2:
        //        currentOrder.flower2 = flowerToGet;
        //        break;
        //    case 3:
        //        currentOrder.flower3 = flowerToGet;
        //        break;
        //}
    }

    /// <summary>
    /// Let a timer run according to the max value set. A timer UI will be rendered and updated according to the timer left.
    /// </summary>
    IEnumerator RunTimerTest()
    {
        sliderReference.SetActive(true);
        float dt = 0;

        while(dt <= maxTimer)
        {
            timerSlider.value = dt;
            dt += Time.deltaTime;
            yield return null;
        }
        sliderReference.SetActive(false);
        timerSlider.value = 0;
    }
}
