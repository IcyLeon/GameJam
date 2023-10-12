using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager instance;

    private Dictionary<Vector2Int, PathFindingNode> pathFindingNodesDictionary = new();

    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap tilemap_Collider, tilemap_Collider2;
    [SerializeField] GameObject PathFindingNodePrefab;
    [SerializeField] Transform PathFindingNodeContainer;

    private void Awake()
    {
        instance = this;
    }

    public static MapManager GetInstance()
    {
        return instance;
    }

    public Tilemap GetMainTileMap()
    {
        return tilemap;
    }

    public Tilemap GetMainTileMap_Collider()
    {
        return tilemap_Collider;
    }

    public Tilemap GetMainTileMap_Collider2()
    {
        return tilemap_Collider2;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int y = GetMainTileMap().cellBounds.min.y; y < GetMainTileMap().cellBounds.max.y; y++)
        {
            for (int x = GetMainTileMap().cellBounds.min.x; x < GetMainTileMap().cellBounds.max.x; x++)
            {
                var tilelocation = new Vector2Int(x, y);
                var tilelocationXYZ = new Vector3Int(tilelocation.x, tilelocation.y, 0);

                if (GetMainTileMap().HasTile(tilelocationXYZ)) {
                    PathFindingNode pathfindingNode = Instantiate(PathFindingNodePrefab, PathFindingNodeContainer.transform).GetComponent<PathFindingNode>();
                    Vector3 CenterPos = GetMainTileMap().GetCellCenterWorld(tilelocationXYZ);
                    pathfindingNode.transform.position = CenterPos;
                    pathFindingNodesDictionary[tilelocation] = pathfindingNode;
                    pathfindingNode.SetPosition(tilelocation);
                    if (GetMainTileMap_Collider().HasTile(tilelocationXYZ) || GetMainTileMap_Collider2().HasTile(tilelocationXYZ))
                    {
                        pathfindingNode.SetWalkable(false);
                    }
                }
            }
        }
    }

    public PathFindingNode GetPathFindingNode(Vector2Int Pos)
    {
        return pathFindingNodesDictionary[Pos];
    }

    public List<PathFindingNode> AStarPathFinding(Vector2Int startPos, Vector2Int endPos)
    {
        PathFindingNode startNode = GetPathFindingNode(startPos);
        PathFindingNode endNode = GetPathFindingNode(endPos);

        List<PathFindingNode> OpenList = new();
        List<PathFindingNode> ClosedList = new();
        List<PathFindingNode> PathList = new();
        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            PathFindingNode currentNode = GetLowestFCost(OpenList);
            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            if (currentNode == endNode)
            {
                PathList = GetPathList(startNode, endNode);
            }

            foreach(PathFindingNode neighbourNode in GetNeighbourNode(currentNode))
            {
                if (!neighbourNode.isWalkable() || ClosedList.Contains(neighbourNode))
                {
                    continue;
                }
                neighbourNode.SetGCost(GetGCost(startNode, neighbourNode));
                neighbourNode.SetHCost(GetGCost(endNode, neighbourNode));
                neighbourNode.SetPrevious(currentNode);

                if (!OpenList.Contains(neighbourNode))
                    OpenList.Add(neighbourNode);
            }
        }
        return PathList;
    }

    private List<PathFindingNode> GetPathList(PathFindingNode StartNode, PathFindingNode EndNode)
    {
        List<PathFindingNode> List = new();
        PathFindingNode current = EndNode;

        while(current != StartNode)
        {
            List.Add(current);
            current = current.GetPrevious();
        }

        List.Reverse();
        return List;
    }


    private int GetGCost(PathFindingNode startNode, PathFindingNode targetNode)
    {
        return Mathf.Abs(startNode.GetPosition().x - targetNode.GetPosition().x) + Mathf.Abs(startNode.GetPosition().y - targetNode.GetPosition().y); 
    }
    private PathFindingNode GetLowestFCost(List<PathFindingNode> List)
    {
        if (List.Count == 0)
            return null;

        PathFindingNode current = List[0];

        foreach (PathFindingNode node in List)
        {
            float fCost = node.CalculateFCost();
            if (fCost < current.CalculateFCost())
            {
                current = node;
            }
        }
        return current;
    }

    private List<PathFindingNode> GetNeighbourNode(PathFindingNode CurrentNode)
    {
        List<PathFindingNode> List = new List<PathFindingNode>();

        // top
        Vector2Int topPosition = new Vector2Int(CurrentNode.GetPosition().x, CurrentNode.GetPosition().y + 1);
        if (pathFindingNodesDictionary.ContainsKey(topPosition))
        {
            List.Add(pathFindingNodesDictionary[topPosition]);
        }

        // bottom
        Vector2Int bottomPosition = new Vector2Int(CurrentNode.GetPosition().x, CurrentNode.GetPosition().y - 1);
        if (pathFindingNodesDictionary.ContainsKey(bottomPosition))
        {
            List.Add(pathFindingNodesDictionary[bottomPosition]);
        }

        // left
        Vector2Int leftPosition = new Vector2Int(CurrentNode.GetPosition().x - 1, CurrentNode.GetPosition().y);
        if (pathFindingNodesDictionary.ContainsKey(leftPosition))
        {
            List.Add(pathFindingNodesDictionary[leftPosition]);
        }

        // right
        Vector2Int rightPosition = new Vector2Int(CurrentNode.GetPosition().x + 1, CurrentNode.GetPosition().y);
        if (pathFindingNodesDictionary.ContainsKey(rightPosition))
        {
            List.Add(pathFindingNodesDictionary[rightPosition]);
        }

        return List;
    }
}
