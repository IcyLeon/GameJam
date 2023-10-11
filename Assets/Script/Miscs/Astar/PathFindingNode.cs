using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode : MonoBehaviour
{
    Vector2Int Position;
    PathFindingNode Previous;
    bool Walkable;
    int GCost;
    int HCost;

    // Start is called before the first frame update
    void Awake()
    {
        Walkable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2Int GetPosition()
    {
        return Position;
    }
    public void SetPosition(Vector2Int pos)
    {
        Position = pos;
    }

    public int CalculateFCost()
    {
        return GCost + HCost;
    }

    public void SetGCost(int value)
    {
        GCost = value;
    }

    public void SetHCost(int value)
    {
        HCost = value;
    }

    public PathFindingNode GetPrevious()
    {
        return Previous;
    }
    public void SetPrevious(PathFindingNode value)
    {
        Previous = value;
    }

    public bool isWalkable()
    {
        return Walkable;
    }

    public void SetWalkable(bool value)
    {
        Walkable = value;
    }
}
