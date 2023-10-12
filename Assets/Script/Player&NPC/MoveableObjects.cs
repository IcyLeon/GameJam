using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveableObjects : MonoBehaviour
{
    [SerializeField] protected MoveableObjectsSO moveableObjectsSO;
    protected Coroutine MovingMoveableObjects;
    protected MapManager mapManager;

    public MoveableObjectsSO GetMoveableObjectsSO()
    {
        return moveableObjectsSO;
    }

    public float GetSpeed()
    {
        if (GetMoveableObjectsSO() == null)
            return 0;

        return GetMoveableObjectsSO().Speed;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        mapManager = MapManager.GetInstance();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateState();
    }

    public bool MoveMoveableObjects(Vector3 EndPos, bool forceToChange = false)
    {
        if (forceToChange && MovingMoveableObjects != null)
        {
            StopCoroutine(MovingMoveableObjects);
        }

        if (MovingMoveableObjects == null || forceToChange)
        {
            MovingMoveableObjects = StartCoroutine(MoveObject(EndPos));
            return true;
        }

        return false;
    }

    protected virtual void UpdateState()
    {

    }

    public bool MoveMoveableObjects_PathFind(Vector2Int Current, Vector2Int EndPos, bool forceToChange = false)
    {
        if (forceToChange && MovingMoveableObjects != null)
        {
            StopCoroutine(MovingMoveableObjects);
        }

        if (MovingMoveableObjects == null || forceToChange)
        {
            MovingMoveableObjects = StartCoroutine(MoveObject_PathFinder(Current, EndPos));
            return true;
        }
        return false;
    }

    private IEnumerator MoveObject(Vector3 EndPos)
    {
        while (transform.position != EndPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPos, Time.deltaTime * GetSpeed());
            yield return null;
        }
        MovingMoveableObjects = null;
    }

    public bool isMoving()
    {
        return MovingMoveableObjects != null;
    }
    private IEnumerator MoveObject_PathFinder(Vector2Int Current, Vector2Int EndPos)
    {
        if (mapManager == null)
            mapManager = MapManager.GetInstance();

        var path = mapManager.AStarPathFinding(Current, EndPos);
        float distanceThreshold = 0.1f; // Adjust this value

        while (path.Count > 0)
        {
            Vector3 targetPosition = path[0].transform.position;
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance > distanceThreshold)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * GetSpeed());
            }
            else
            {
                path.RemoveAt(0);
            }
            yield return null;
        }
        MovingMoveableObjects = null;
    }
}
