using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class QueueRowManager : MonoBehaviour
{
    private List<NPC> NPCQueueList = new List<NPC>();
    [SerializeField] Transform FirstWayPointPosition;
    [SerializeField] int MaxQueueSize;
    private Coroutine Coroutine;
    private MapManager mapManager;
    private List<Vector2Int> QueuePositionList = new();

    private void Start()
    {
        mapManager = MapManager.GetInstance();
        for (int i = 0; i < MaxQueueSize; i++)
        {
            Vector2Int pos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(FirstWayPointPosition.position).x, mapManager.GetMainTileMap().WorldToCell(FirstWayPointPosition.position).y + i);
            QueuePositionList.Add(pos);
        }
    }

    public List<NPC> GetNPCQueueList()
    {
        return NPCQueueList;
    }

    public void AddNPC(NPC npc)
    {
        NPCQueueList.Add(npc);

        Transform RandomEntranceTransform = QueueSystem.GetInstance().GetEntrancePosition();
        Vector2Int pos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(RandomEntranceTransform.position).x, mapManager.GetMainTileMap().WorldToCell(RandomEntranceTransform.position).y);
        npc.transform.position = RandomEntranceTransform.position;
        npc.MoveMoveableObjects_PathFind(pos, QueuePositionList[NPCQueueList.IndexOf(npc)]);
        RelocateNPCQueue();
    }

    public bool CanAddNPC()
    {
        return GetTotalQueueRow() < MaxQueueSize;
    }

    public int GetTotalQueueRow()
    {
        return NPCQueueList.Count;
    }

    public NPC GetFirstInQueue()
    {
        if (NPCQueueList.Count == 0)
            return null;
        else
        {
            NPC npc = NPCQueueList[0];
            return npc;
        }
    }

    public void LeaveNPC(NPC npc)
    {
        if (Coroutine == null)
        {
            Coroutine = StartCoroutine(Served(npc));
        }
    }

    IEnumerator Served(NPC npc)
    {
        yield return new WaitForSeconds(0.5f);
        Transform RandomEntranceTransform = QueueSystem.GetInstance().GetEntrancePosition();
        Vector2Int RandomEntranceTransformGrid = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(RandomEntranceTransform.position).x, mapManager.GetMainTileMap().WorldToCell(RandomEntranceTransform.position).y);
        Vector2Int pos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(npc.transform.position).x, mapManager.GetMainTileMap().WorldToCell(npc.transform.position).y);

        if (npc.MoveMoveableObjects_PathFind(pos, RandomEntranceTransformGrid))
        {
            npc.Served();
            NPCQueueList.Remove(npc);
            RelocateNPCQueue();
        }
        Coroutine = null;
        yield return null;

    }

    private void RelocateNPCQueue()
    {
        for (int i = 0; i < NPCQueueList.Count; i++)
        {
            NPC npc = NPCQueueList[i];
            Vector2Int pos = new Vector2Int(mapManager.GetMainTileMap().WorldToCell(npc.transform.position).x, mapManager.GetMainTileMap().WorldToCell(npc.transform.position).y);
            npc.MoveMoveableObjects_PathFind(pos, QueuePositionList[NPCQueueList.IndexOf(npc)], true);
        }
    }
}
