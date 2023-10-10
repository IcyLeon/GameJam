using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] List<Transform> waypointList = new List<Transform>();
}
