using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MoveableObjects
{
    [SerializeField] PlaySound playSound;
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
        InventoryManager.GetInstance().onCurrencyValueChanged += SpawnText;
        togglePanel(false);
        state = NPCState.MOVE_TO_QUEUE;
        RandomGenerateItem();
        SetupItemImages();

    }

    private void RandomGenerateItem()
    {
        // Get the list of unlocked ItemSO
        List<ItemsSO> unlockedItemList = InventoryManager.GetInstance().GetFlowersSOList();
        List<ItemsSO> generatedItemList = new();

        if (unlockedItemList.Count == 0)
            return;

        // Generate a total of 3 different flower type acoording to what player has unlocked
        for (int i = 0; i < MaxAmount; i++)
        {
            int nums = Random.Range(0, unlockedItemList.Count);
            generatedItemList.Add(unlockedItemList[nums]);
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
            ItemsSO itemsSOREF = AssetManager.GetInstance().GetFlowerItemsSO(itemsSO);
            if (itemsSOREF != null)
            {
                Item itemREF = new Item(itemsSOREF, amtOfFlowerTypes[itemsSOREF]);
                ItemsList.Add(itemREF);
            }
        }

        ItemsSO WrapperItem = InventoryManager.GetInstance().GetWrappersSOList()[0];
        if (WrapperItem != null)
        {
            Item item = new Item(WrapperItem, 1);
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
        for (int i = 0; i < ItemsList.Count - 1; i++)
        {
            ItemSlot itemSlot = Instantiate(ItemSlotPrefab, ItemContainer).GetComponent<ItemSlot>();
            ItemSlotList.Add(itemSlot);
        }

        for (int i = 0; i < ItemSlotList.Count; i++)
        {
            ItemSlotList[i].UpdateContent(ItemsList[i].GetItemsSO().ItemSprite, "X" + ItemsList[i].GetAmount().ToString());
        }
    }

    public void SpawnText(int value)
    {
        if (QueueSystem.GetInstance().GetNPCTobeServed() == this)
        {
            GameObject go = Instantiate(AssetManager.GetInstance().CanvasWorldText, transform.position, Quaternion.identity);
            WorldText worldText = go.GetComponent<WorldText>();
            worldText.Init(value.ToString(), 1f, 0.65f, Color.yellow, Vector3.zero);
            worldText.MovingText(Vector3.up * 3f);

            ParticleSystem particleSystem = Instantiate(AssetManager.GetInstance().ParticlesEffectBurst, transform).GetComponent<ParticleSystem>();
            Destroy(particleSystem, particleSystem.main.duration);

            if (playSound)
                if (playSound.GetAudioSource())
                    playSound.PlayAudio(playSound.GetAudioSource());
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
