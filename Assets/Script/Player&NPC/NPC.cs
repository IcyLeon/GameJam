using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MoveableObjects
{
    [SerializeField] Canvas canvasUI;
    [SerializeField] Transform ItemContainer;

    [SerializeField] GameObject ItemSlotPrefab;
    List<ItemSlot> ItemSlotList = new();
    List<Item> ItemsList = new();
    private int MaxAmount = 3;
    Coroutine togglePanelDelayCoroutine, ChangeState;

    public enum NPCState
    {
        MOVE_TO_QUEUE,
        LEAVING,
    }
    private NPCState state;

    protected override void Start()
    {
        togglePanel(false);
        state = NPCState.MOVE_TO_QUEUE;
        RandomGenerateItem();
        SetupItemImages();
    }

    private void RandomGenerateItem()
    {
        Item item = new Item(AssetManager.GetInstance().GetItemsSOByFlowerTypes(FlowerTypes.ROSE), 2);
        ItemsList.Add(item);
    }

    protected override void Update()
    {
        UpdateState();
    }

    private void SetupItemImages()
    {
        for (int i = 0; i < ItemsList.Count; i++)
        {
            ItemSlot itemSlot = Instantiate(ItemSlotPrefab, ItemContainer).GetComponent<ItemSlot>();
            ItemSlotList.Add(itemSlot);
        }

        for (int i = 0; i < ItemSlotList.Count; i++)
        {
            ItemSlotList[i].UpdateContent(ItemsList[i].GetItemsSO().ItemSprite, "X" + ItemsList[i].GetAmount().ToString());
        }
    }

    private void UpdateState()
    {
        switch(state)
        {
            case NPCState.MOVE_TO_QUEUE:
                if (QueueSystem.GetInstance().isFirstInQueue(this)) {
                    if (togglePanelDelayCoroutine == null)
                        togglePanelDelayCoroutine = StartCoroutine(togglePanelDelay(true));
                    if (canvasUI.gameObject.activeSelf && ChangeState == null)
                        ChangeState = StartCoroutine(Test(NPCState.LEAVING));
                }
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
        {
            togglePanel(false);
            QueueSystem.GetInstance().LeaveNPCFromQueue(this);
        }
        SetState(state);
    }

    private IEnumerator togglePanelDelay(bool active)
    {
        yield return new WaitForSeconds(0.5f);
        togglePanel(active);
    }

    private void togglePanel(bool active)
    {
        canvasUI.gameObject.SetActive(active);
    }
}
