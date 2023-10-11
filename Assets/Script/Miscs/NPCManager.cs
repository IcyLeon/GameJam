using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private static NPCManager instance;
    [SerializeField] GameObject NPCPrefab;
    private List<NPC> NPCList = new();

    private void Awake()
    {
        instance = this;
    }
    public static NPCManager GetInstance()
    {
        return instance;
    }
    public NPC SpawnNPC()
    {
        NPC npc = Instantiate(NPCPrefab).GetComponent<NPC>();
        NPCList.Add(npc);
        return npc;
    }

    public void RemoveNPC(NPC npc)
    {
        NPCList.Remove(npc);
        Destroy(npc.gameObject);
    }
}
