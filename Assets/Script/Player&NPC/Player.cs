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

    private OrderInformation orderToFollow;
    private OrderInformation currentOrder;
    private PlayerState playerState;
    private Vector3 targetPos;
    private MapManager mapManager;

    private enum PlayerState
    {
        IDLE,
        COLLECTING,
        GETFLOWER,
        GETWRAP,
        STARTWRAPPING,
        MOVETOCOUNTER,
    }    

    protected override void Start()
    {
        mapManager = MapManager.GetInstance();
        base.Start();
        playerState = PlayerState.IDLE;
        sliderReference.SetActive(false);
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            Vector2Int CurrentPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(transform.position).x, mapManager.GetMainTileMap().WorldToCell(transform.position).y);
            Vector2Int EndPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(pos).x, mapManager.GetMainTileMap().WorldToCell(pos).y);
            MoveMoveableObjects_PathFind(CurrentPos, EndPos);
        }

        switch (playerState)
        {
            case PlayerState.IDLE:
                {
                    if (OrderSystem.GetInstance().GetOrder() != null)
                    {
                        StartOrder(OrderSystem.GetInstance().GetOrder());
                        Vector2Int pos = CheckNearestFlowerPos(OrderSystem.GetInstance().GetFlowerBoothLocation(FlowerTypes.ROSE));
                        Vector2Int CurrentPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(transform.position).x, mapManager.GetMainTileMap().WorldToCell(transform.position).y);
                        MoveMoveableObjects_PathFind(CurrentPos, pos);
                        playerState = PlayerState.COLLECTING;
                    }

                    break;
                }

            case PlayerState.COLLECTING:
                {
                    break;
                }
        }
    }

    Vector2Int CheckNearestFlowerPos(Vector3 stationPos)
    {
        Vector2Int startPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(transform.position).x, mapManager.GetMainTileMap().WorldToCell(transform.position).y);
        Vector2Int endPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(stationPos).x, mapManager.GetMainTileMap().WorldToCell(stationPos).y);

        // Create an array to store the paths in all four directions
        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(endPos.x, endPos.y + 3),  // Up
        new Vector2Int(endPos.x - 3, endPos.y),  // Left
        new Vector2Int(endPos.x + 3, endPos.y),  // Right
        new Vector2Int(endPos.x, endPos.y - 3)   // Down
        };

        List<List<PathFindingNode>> pathLengths = new List<List<PathFindingNode>>();  // Nested list for storing path lengths

        // Calculate the path lengths in all four directions
        foreach (Vector2Int direction in directions)
        {
            var path = MapManager.GetInstance().AStarPathFinding(startPos, direction);
            pathLengths.Add(path);  // Add the list of path lengths to the nested list
        }

        return directions[GetLowestPathIdx(pathLengths)];

    }

    private int GetLowestPathIdx(List<List<PathFindingNode>> List)
    {
        int idx = 0;
        if (List.Count == 0)
            return idx;

        int current = List[0].Count;

        for (int i = 0; i < List.Count; i++)
        {
            if (List[i].Count < current && List[i].Count != 0)
            {
                current = List[i].Count;
                idx = i;
            }
        }
        return idx;
    }


    //Vector2Int CheckNearestFlowerPos(Vector3 stationPos)
    //{

    //    Vector2Int startPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(transform.position).x, mapManager.GetMainTileMap().WorldToCell(transform.position).y);
    //    Vector2Int EndPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(stationPos).x, mapManager.GetMainTileMap().WorldToCell(stationPos).y);


    //    var path1 = MapManager.GetInstance().AStarPathFinding(startPos, new Vector2Int(EndPos.x, EndPos.y + 1));
    //    var path2 = MapManager.GetInstance().AStarPathFinding(startPos, new Vector2Int(EndPos.x - 1, EndPos.y));
    //    var path3 = MapManager.GetInstance().AStarPathFinding(startPos, new Vector2Int(EndPos.x + 1, EndPos.y));
    //    var path4 = MapManager.GetInstance().AStarPathFinding(startPos, new Vector2Int(EndPos.x, EndPos.y - 1));

    //    List<int> testPathList = new List<int>();
    //    testPathList.Add(path1.Count);
    //    testPathList.Add(path2.Count);
    //    testPathList.Add(path3.Count);
    //    testPathList.Add(path4.Count);

    //    int pos = 0;
    //    for (int i = 1; i < testPathList.Count; i++)
    //    {
    //        if (testPathList[i] < testPathList[i - 1])
    //        {
    //            pos = i;
    //        }
    //    }

    //    switch (pos)
    //    {
    //        case 0:
    //            return new Vector2Int(EndPos.x, EndPos.y + 1);
    //        case 1:
    //            return new Vector2Int(EndPos.x - 1, EndPos.y);
    //        case 2:
    //            return new Vector2Int(EndPos.x + 1, EndPos.y);
    //        case 3:
    //            return new Vector2Int(EndPos.x, EndPos.y - 1);
    //        default:
    //            return new Vector2Int(0, 0);
    //    }
    //}

    // give the player an order to do. The player will automatically fetch the required items and give it to the counter
    public void StartOrder(OrderInformation newOrder)
    {
        // CLear out all the orders and assigned it to OrderToFollow
        orderToFollow = newOrder;
        currentOrder = null;
        currentOrder = new OrderInformation();
    }

    void SetDestination()
    {

    }

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
