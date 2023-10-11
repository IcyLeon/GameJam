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
    private List<FlowerTypes> generatedItemList;

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
        // Get the list of unlocked ItemSO
        List<ItemsSO> unlockedItemList = InventoryManager.GetInstance().GetItemsSOList();

        // List to temporary stored the generated flowerTypes
        generatedItemList = new List<FlowerTypes>();

        // Generate a total of 3 different flower type acoording to what player has unlocked
        for (int i = 0; i < MaxAmount; i++)
        {
            int num = Random.Range(0, unlockedItemList.Count);
            FlowerTypes generatedFlowerType = unlockedItemList[num].flowerType;
            generatedItemList.Add(generatedFlowerType);
        }

        // Create a dictonary to get any similar occurence
        Dictionary<FlowerTypes, int> amtOfFlowerTypes = new Dictionary<FlowerTypes, int>();
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
        foreach (var flower in amtOfFlowerTypes.Keys)
        {
            Item item = new Item(AssetManager.GetInstance().GetItemsSOByFlowerTypes(flower), amtOfFlowerTypes[flower]);
            ItemsList.Add(item);
        }
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
        OrderSystem.GetInstance().GenerateNewOrder(generatedItemList);
        togglePanel(active);
    }

    private void togglePanel(bool active)
    {
        canvasUI.gameObject.SetActive(active);
    }
}
