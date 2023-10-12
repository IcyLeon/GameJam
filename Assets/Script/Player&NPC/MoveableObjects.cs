using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveableObjects : MonoBehaviour
{
    [SerializeField] protected Animator AnimatorController;
    [SerializeField] protected MoveableObjectsSO moveableObjectsSO;
    [SerializeField] float offsetHeight = 0.0f;
    private float Speed;
    protected Coroutine MovingMoveableObjects;
    protected MapManager mapManager;

    public MoveableObjectsSO GetMoveableObjectsSO()
    {
        return moveableObjectsSO;
    }

    public float GetScriptableObjectSpeed()
    {
        if (GetMoveableObjectsSO() == null)
            return 0;

        return GetMoveableObjectsSO().Speed;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        mapManager = MapManager.GetInstance();
        Speed = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        AnimatorController.SetFloat("Velocity", GetSpeed());
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
            Speed = GetScriptableObjectSpeed();
            transform.position = Vector3.MoveTowards(transform.position, EndPos, Time.deltaTime * Speed);
            yield return null;
        }
        Speed = 0;
        Vector3 PosOffset = transform.position;
        PosOffset.y = transform.position.y + offsetHeight;
        transform.position = PosOffset;
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
            targetPosition.y = targetPosition.y + offsetHeight;
            float distance = Vector3.Distance(transform.position, targetPosition);
            Speed = GetScriptableObjectSpeed();
            if (distance > distanceThreshold)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * Speed);
            }
            else
            {
                path.RemoveAt(0);
            }
            yield return null;
        }
        Speed = 0;
        MovingMoveableObjects = null;
    }

    public float GetSpeed()
    {
        return Speed;
    }
}
