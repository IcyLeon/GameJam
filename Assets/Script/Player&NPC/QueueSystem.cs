using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QueueSystem : MonoBehaviour
{
    private static QueueSystem instance;
    public Transform[] EntrancePosition;
    private QueueRowManager[] RowsQueue;
    private NPCManager npcManager;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
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
