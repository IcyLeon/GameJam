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
    Coroutine togglePanelDelayCoroutine;
    private float WaitingTime = 0;

    public enum NPCState
    {
        MOVE_TO_QUEUE,
        LEAVING,
    }
    private NPCState state;

    protected override void Start()
    {
        base.Start();
        togglePanel(false);
        state = NPCState.MOVE_TO_QUEUE;
        RandomGenerateItem();
        SetupItemImages();

    }

    private void RandomGenerateItem()
    {
        // Get the list of unlocked ItemSO
        List<ItemsSO> unlockedItemList = InventoryManager.GetInstance().GetItemsSOList();
        List<ItemsSO> generatedItemList = new();

        if (unlockedItemList.Count == 0)
            return;

        // Generate a total of 3 different flower type acoording to what player has unlocked
        for (int i = 0; i < MaxAmount; i++)
        {
            int num = Random.Range(0, unlockedItemList.Count);
            generatedItemList.Add(unlockedItemList[num]);
        }

        // Create a dictonary to get any similar occurence
        Dictionary<ItemsSO, int> amtOfFlowerTypes = new Dictionary<ItemsSO, int>();
        for (int i = 0; i < generatedItemList.Count; i++)
        {
            if (!amtOfFlowerTypes.ContainsKey(generatedItemList[i]))
            {
                amtOfFlowerTypes.Add(generatedItemList[i], 1);
            }
            else
            {
                amtOfFlowerTypes[generatedItemList[i]] += 1;
            }
        }

        // for every Key in the dictionary, create a new "Item" and add it into the itemList
        foreach (var itemsSO in amtOfFlowerTypes.Keys)
        {
            Item item = new Item(AssetManager.GetInstance().GetItemsSO(itemsSO), amtOfFlowerTypes[itemsSO]);
            ItemsList.Add(item);
        }
    }

    protected override void Update()
    {
        UpdateState();
        base.Update();
        
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

    protected override void UpdateState()
    {
        switch(state)
        {
            case NPCState.MOVE_TO_QUEUE:
                if (QueueSystem.GetInstance().isFirstInQueue(this)) {
                    if (togglePanelDelayCoroutine == null)
                        togglePanelDelayCoroutine = StartCoroutine(togglePanelDelay(true));

                    if (canvasUI.gameObject.activeSelf)
                        UpdateWaitingTime();

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

    public void UpdateWaitingTime()
    {
        if (state != NPCState.LEAVING)
            WaitingTime += Time.deltaTime;
    }

    public void Served()
    {
        OrderSystem.GetInstance().DeleteOrder(OrderSystem.GetInstance().GetOrder());
        SwitchState(NPCState.LEAVING);
    }

    private void SetState(NPCState state)
    {
        this.state = state;
    }


    public void SwitchState(NPCState state)
    {
        if (state == NPCState.LEAVING)
        {
            togglePanel(false);
        }
        SetState(state);
    }

    public float GetWaitingTime()
    {
        if (!gameObject.activeSelf)
            WaitingTime = 0;

        return WaitingTime;
    }

    private IEnumerator togglePanelDelay(bool active)
    {
        yield return new WaitForSeconds(0.5f);
        OrderSystem.GetInstance().GenerateNewOrder(ItemsList);
        togglePanel(active);
    }

    private void togglePanel(bool active)
    {
        canvasUI.gameObject.SetActive(active);
    }
}
