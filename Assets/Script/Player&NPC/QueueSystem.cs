using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QueueSystem : MonoBehaviour
{
    [Serializable]
    public class QueueWaypoint
    {
        public Transform ServingWaypointTransform;
        public QueueRowManager queueREF;
    }
    private static QueueSystem instance;
    [SerializeField] Transform[] EntrancePosition;
    [SerializeField] QueueWaypoint[] ServingQueueWaypoint;
    private QueueRowManager[] RowsQueue;
    private NPCManager npcManager;

    [SerializeField] float maxTimeIntervalBetweenCustomer = 15.0f;
    [SerializeField] float minTimeIntervalBetweenCustomer = 5.0f;
    private float timer;


    private void Awake()
    {
        instance = this;
        RowsQueue = GetComponentsInChildren<QueueRowManager>();
    }
    public static QueueSystem GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    private void Start()
    {
        npcManager = NPCManager.GetInstance();
        RowsQueue = GetComponentsInChildren<QueueRowManager>();
        timer = minTimeIntervalBetweenCustomer;
    }

    private float RandomizeTime()
    {
        return UnityEngine.Random.Range(minTimeIntervalBetweenCustomer, maxTimeIntervalBetweenCustomer);
    }    

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    QueueRowManager queue = FindShortestQueue();
        //    if (queue != null)
        //    {
        //        if (queue.CanAddNPC())
        //        {
        //            NPC npc = npcManager.SpawnNPC();
        //            queue.AddNPC(npc);
        //        }
        //    }
        //}

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = RandomizeTime();
            QueueRowManager queue = FindShortestQueue();
            if (queue != null)
            {
                if (queue.CanAddNPC())
                {
                    NPC npc = npcManager.SpawnNPC();
                    queue.AddNPC(npc);
                }
            }
        }
    }

    public Transform GetFIFOWaypointTransform(QueueRowManager queue)
    {
        for (int i = 0; i < ServingQueueWaypoint.Length; i++)
        {
            QueueWaypoint queueWaypoint = ServingQueueWaypoint[i];
            if (queueWaypoint.queueREF == queue)
                return queueWaypoint.ServingWaypointTransform;
        }
        return null;
    }
    public NPC GetNPCTobeServed()
    {
        QueueRowManager queue = GetFirstQueueFIFO();
        if (queue == null)
            return null;

        return queue.GetFirstInQueue();
    }

    public QueueRowManager GetFirstQueueFIFO()
    {
        if (RowsQueue.Length == 0)
        {
            return null;
        }

        QueueRowManager queue = RowsQueue[0];

        for (int i = 1; i < RowsQueue.Length; i++) // Start the loop from the second element
        {
            QueueRowManager row = RowsQueue[i];

            if (row.GetFirstInQueue())
            {
                if (!queue.GetFirstInQueue() || row.GetFirstInQueue().GetWaitingTime() > queue.GetFirstInQueue().GetWaitingTime())
                {
                    queue = row;
                }
            }
        }

        return queue;
    }

    public Transform GetEntrancePosition()
    {
        if (EntrancePosition.Length == 0)
            return null;

        return EntrancePosition[UnityEngine.Random.Range(0, EntrancePosition.Length)];
    }

    private QueueRowManager FindShortestQueue()
    {
        if (RowsQueue.Length == 0)
            return null;

        QueueRowManager queue = RowsQueue[0];

        for (int i = 0; i < RowsQueue.Length; i++)
        {
            QueueRowManager row = RowsQueue[i];
            if (row.GetTotalQueueRow() < queue.GetTotalQueueRow())
            {
                queue = row;
            }
        }
        return queue;
    }

    public bool isFirstInQueue(NPC npc)
    {
        QueueRowManager queue = GetQueueAt(npc);
        if (queue != null)
        {
            if (queue.GetFirstInQueue() == npc && !npc.isMoving())
            {
                return true;
            }
        }

        return false;
    }

    public void LeaveNPCFromQueue(NPC npc)
    {
        QueueRowManager queue = GetQueueAt(npc);
        if (queue != null)
            queue.LeaveNPC(npc);
    }

    public QueueRowManager GetQueueAt(NPC npc)
    {
        for (int i = 0; i < RowsQueue.Length; i++)
        {
            QueueRowManager queueRow = RowsQueue[i];
            for (int j = 0; j < queueRow.GetNPCQueueList().Count; j++)
                if (queueRow.GetNPCQueueList()[j] == npc)
                    return queueRow;
        }
        return null;
    }
}
