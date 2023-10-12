using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Player : MoveableObjects
{
    // Timer
    private Slider timerSlider;
    [SerializeField] float maxTimer = 1.0f;
    private PlayerState playerState;
    private Coroutine timerCoroutine;
    private OrderSystem orderSystem;
    private Item currentItemPreparing;

    private List<Item> CurrentItemPreparingList = new List<Item>();

    private enum PlayerState
    {
        IDLE,
        MOVING,
        COLLECTING,
        MOVETOCOUNTER,
        SERVED,
    }    

    protected override void Start()
    {
        base.Start();
        orderSystem = OrderSystem.GetInstance();
        playerState = PlayerState.IDLE;
    }

    protected override void Update()
    {
        Vector2Int CurrentPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(transform.position).x, mapManager.GetMainTileMap().WorldToCell(transform.position).y);

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            Vector2Int EndPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(pos).x, mapManager.GetMainTileMap().WorldToCell(pos).y);
            MoveMoveableObjects_PathFind(CurrentPos, EndPos);
        }

        base.Update();
    }

    protected override void UpdateState()
    {
        Vector2Int CurrentPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(transform.position).x, mapManager.GetMainTileMap().WorldToCell(transform.position).y);
        if (OrderSystem.GetInstance().GetOrder() == null)
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                Destroy(timerSlider.gameObject);
            }
            playerState = PlayerState.IDLE;
        }

        switch (playerState)
        {
            case PlayerState.IDLE:
                if (orderSystem.GetOrder() != null)
                    playerState = PlayerState.MOVING;

                break;
            case PlayerState.MOVING:
                if (orderSystem.GetOrder() != null)
                {
                    currentItemPreparing = orderSystem.GetFirstOrderItem();
                    Transform ClosestWaypoint = CheckNearestFlowerPos(orderSystem.GetBoothStation(currentItemPreparing));
                    if (ClosestWaypoint != null)
                    {
                        Vector2Int ClosestWaypointPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(ClosestWaypoint.position).x, mapManager.GetMainTileMap().WorldToCell(ClosestWaypoint.position).y);
                        MoveMoveableObjects_PathFind(CurrentPos, ClosestWaypointPos);
                        playerState = PlayerState.COLLECTING;
                    }
                }
                break;
            case PlayerState.COLLECTING:
                if (!isMoving())
                {
                    if (timerCoroutine == null)
                        timerCoroutine = StartCoroutine(RunTimer(PlayerState.MOVETOCOUNTER, currentItemPreparing.GetAmount()));
                }
                break;
            case PlayerState.MOVETOCOUNTER:
                if (orderSystem.GetItemListPreparing(orderSystem.GetOrder()) > 0)
                {
                    playerState = PlayerState.MOVING;
                }
                else
                {
                    if (!isMoving())
                    {
                        QueueSystem queueSystem = QueueSystem.GetInstance();
                        Transform ClosestWaypoint = queueSystem.GetFIFOWaypointTransform(queueSystem.GetFirstQueueFIFO());
                        if (ClosestWaypoint != null)
                        {
                            Vector2Int ClosestWaypointPos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(ClosestWaypoint.position).x, mapManager.GetMainTileMap().WorldToCell(ClosestWaypoint.position).y);
                            MoveMoveableObjects_PathFind(CurrentPos, ClosestWaypointPos);
                            playerState = PlayerState.SERVED;
                        }
                    }
                }
                break;
            case PlayerState.SERVED:
                if (!isMoving())
                {
                    QueueSystem queueSystem = QueueSystem.GetInstance();

                    queueSystem.LeaveNPCFromQueue(queueSystem.GetNPCTobeServed());
                    playerState = PlayerState.IDLE;
                }
                break;
        }
    }

    Transform CheckNearestFlowerPos(Station station)
    {
        if (station == null)
            return null;

        if (station.GetAllWaypoints().Length == 0)
            return null;

        Transform current = station.GetAllWaypoints()[1];
        float closestDistance = Vector3.Distance(transform.position, current.position); // Initialize with the distance to the first waypoint

        for (int i = 1; i < station.GetAllWaypoints().Length; i++)
        {
            float distance = Vector3.Distance(transform.position, station.GetAllWaypoints()[i].position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                current = station.GetAllWaypoints()[i];
            }
        }

        return current;
    }

    /// <summary>
    /// Let a timer run according to the max value set. A timer UI will be rendered and updated according to the timer left.
    /// </summary>
    IEnumerator RunTimer(PlayerState nextState, int count)
    {
        int i = 0;
        float dt = 0;
        TimerDisplay timerSlider = Instantiate(AssetManager.GetInstance().GetTimerPrefab(), transform).GetComponent<TimerDisplay>();
        timerSlider.SetMinandMaxValue(0, maxTimer);
        timerSlider.transform.position = transform.position;

        while (i < count)
        {
            timerSlider.UpdateTime(dt);
            if (dt >= maxTimer)
            {
                dt = 0;
                i++;
            }
            dt += Time.deltaTime;
            yield return null;
        }
        UpdatePreparedItemList();
        playerState = nextState;
        timerCoroutine = null;
        Destroy(timerSlider.gameObject);
    }

    private void UpdatePreparedItemList()
    {
        orderSystem.DeleteOrderItem(currentItemPreparing);
        CurrentItemPreparingList.Add(currentItemPreparing);
    }
}
