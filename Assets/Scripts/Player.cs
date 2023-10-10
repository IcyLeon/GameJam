using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;

    // Timer
    [SerializeField] GameObject sliderReference;
    [SerializeField] Slider timerSlider;
    [SerializeField] float maxTimer = 1.0f;

    private OrderInformation orderToFollow;
    private OrderInformation currentOrder;
    private bool startGathering = false;
    private PlayerState playerState;
    private PlayerState nextState;
    private bool doneGathering;
    private Vector3 targetPos;

    private enum PlayerState
    {
        IDLE,
        COLLECTING,
        GETFLOWER1,
        GETFLOWER2,
        GETFLOWER3,
        GETWRAP,
        MOVETOCOUNTER,
    }    

    private void Start()
    {
        playerState = PlayerState.IDLE;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        sliderReference.SetActive(false);
    } 

    private void Update()
    {
        switch (playerState)
        {
            case PlayerState.IDLE:
                {
                    if (startGathering)
                    {
                        playerState = PlayerState.GETFLOWER1;
                        targetPos = OrderSystem.GetInstance().GetFlowerBoothLocation(orderToFollow.flower1);
                        navMeshAgent.SetDestination(targetPos);
                        startGathering = false;
                    }

                    break;
                }

            case PlayerState.GETFLOWER1:
                {
                    if ((targetPos - transform.position).magnitude < navMeshAgent.stoppingDistance)
                    {
                        GetFlower(orderToFollow.flower1, 1);
                        nextState = PlayerState.GETFLOWER2;
                        playerState = PlayerState.COLLECTING;
                        StartCoroutine(RunTimerTest());
                    }

                    break;
                }

            case PlayerState.GETFLOWER2:
                {
                    if ((targetPos - transform.position).magnitude < navMeshAgent.stoppingDistance)
                    {
                        GetFlower(orderToFollow.flower2, 2);
                        nextState = PlayerState.GETFLOWER3;
                        playerState = PlayerState.COLLECTING;
                        StartCoroutine(RunTimerTest());
                    }
                    break;
                }

            case PlayerState.GETFLOWER3:
                {
                    if ((targetPos - transform.position).magnitude < navMeshAgent.stoppingDistance)
                    {
                        GetFlower(orderToFollow.flower3, 3);
                        nextState = PlayerState.GETWRAP;
                        playerState = PlayerState.COLLECTING;
                        StartCoroutine(RunTimerTest());
                    }
                    break;
                }

            case PlayerState.GETWRAP:
                {
                    if ((targetPos - transform.position).magnitude < navMeshAgent.stoppingDistance)
                    {
                        nextState = PlayerState.MOVETOCOUNTER;
                        playerState = PlayerState.COLLECTING;
                        StartCoroutine(RunTimerTest());
                    }
                    break;
                }

            case PlayerState.MOVETOCOUNTER:
                {
                    if ((targetPos - transform.position).magnitude < navMeshAgent.stoppingDistance)
                    {
                        playerState = PlayerState.IDLE;
                    }
                    break;
                }

            case PlayerState.COLLECTING:
                {
                    if (doneGathering)
                    {
                        doneGathering = false;
                        playerState = nextState;

                        switch (nextState)
                        {
                            case PlayerState.GETFLOWER1:
                                {
                                    targetPos = OrderSystem.GetInstance().GetFlowerBoothLocation(orderToFollow.flower1);
                                    break;
                                }
                            case PlayerState.GETFLOWER2:
                                {
                                    targetPos = OrderSystem.GetInstance().GetFlowerBoothLocation(orderToFollow.flower2);
                                    break;
                                }
                            case PlayerState.GETFLOWER3:
                                {
                                    targetPos = OrderSystem.GetInstance().GetFlowerBoothLocation(orderToFollow.flower3);
                                    break;
                                }
                            case PlayerState.GETWRAP:
                                {
                                    targetPos = OrderSystem.GetInstance().GetWrapLocation();
                                    break;
                                }
                            case PlayerState.MOVETOCOUNTER:
                                {
                                    targetPos = OrderSystem.GetInstance().GetCounterLocation();
                                    break;
                                }
                        }
                        navMeshAgent.SetDestination(targetPos);
                    }

                    break;
                }
        }
    }

    /// <summary>
    /// Used to tell the player that they can go to the next state. Called from the OrderSystem
    /// </summary>
    public void GoNextState()
    {
        doneGathering = true;
    }

    // give the player an order to do. The player will automatically fetch the required items and give it to the counter
    public void GiveOrder(OrderInformation newOrder)
    {
        // CLear out all the orders and assigned it to OrderToFollow
        orderToFollow = newOrder;
        currentOrder = null;
        currentOrder = new OrderInformation();
        startGathering = true;
    }

    /// <summary>
    /// Update the currentOrder class to match some of the ordertofollow class
    /// </summary>
    void GetFlower(FlowerTypes flowerToGet, int whichFlower)
    {
        switch (whichFlower)
        {
            case 1:
                currentOrder.flower1 = flowerToGet;
                break;
            case 2:
                currentOrder.flower2 = flowerToGet;
                break;
            case 3:
                currentOrder.flower3 = flowerToGet;
                break;
        }
    }

    /// <summary>
    /// Let a timer run according to the max value set. A timer UI will be rendered and updated according to the timer left.
    /// </summary>
    IEnumerator RunTimerTest()
    {
        sliderReference.SetActive(true);
        float dt = 0;

        while(dt <= maxTimer)
        {
            timerSlider.value = dt;
            dt += Time.deltaTime;
            yield return null;
        }
        sliderReference.SetActive(false);
        timerSlider.value = 0;
        doneGathering = true;
    }
}
