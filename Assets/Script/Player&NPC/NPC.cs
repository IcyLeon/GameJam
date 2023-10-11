using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MoveableObjects
{
    List<ItemsSO> ItemsList = new();
    public enum NPCState
    {
        MOVE_TO_QUEUE,
        LEAVING,
    }
    private NPCState state;

    protected override void Start()
    {
        state = NPCState.MOVE_TO_QUEUE;
    }


    protected override void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        switch(state)
        {
            case NPCState.MOVE_TO_QUEUE:
                if (QueueSystem.GetInstance().isFirstInQueue(this))
                    StartCoroutine(Test(NPCState.LEAVING));

                break;
            case NPCState.LEAVING:
                if (!isMoving())
                {
                    NPCManager.GetInstance().RemoveNPC(this);
                }
                break;
        }
    }

    public void SetState(NPCState state)
    {
        this.state = state;
    }
    public IEnumerator Test(NPCState state)
    {
        yield return new WaitForSeconds(2f);
        if (state == NPCState.LEAVING)
            QueueSystem.GetInstance().LeaveNPCFromQueue(this);
        SetState(state);
    }

}
