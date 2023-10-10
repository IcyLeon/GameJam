using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    private NPCState state;

    private enum NPCState
    {
        MOVE_TO_COUNTER,
        WAITING_FOR_ORDER,
        LEAVING,
    }

    private void Start()
    {
        state = NPCState.MOVE_TO_COUNTER;
    }

    private void Update()
    {
        
    }
}
