using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTraveler : MonoBehaviour
{
    [SerializeField] private GameObject WaypointCore = null;
    private WaypointThing WaypointRef = null;
    [SerializeField] [Unity.Collections.ReadOnly] private int currentPoint = 0;
    [SerializeField] [Unity.Collections.ReadOnly] private int traversalDir = 1;

    private enum Traversal { None, Travers, PingPong }
    [SerializeField] private Traversal TraversalType = Traversal.None;

    private void Awake()
    {
        if (WaypointCore.GetComponent<WaypointThing>() != null)
        {
            WaypointRef = WaypointCore.GetComponent<WaypointThing>();
        }
        else { throw new MissingComponentException("No Waypoint core was found " + transform.name); }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, WaypointRef.Waypoints[currentPoint]) > 0.1f) { transform.position = Vector3.Lerp(transform.position, WaypointRef.Waypoints[currentPoint], Time.deltaTime); }
        else if (traversalDir == 1 && currentPoint < WaypointRef.Waypoints.Count - 1) { currentPoint += traversalDir; }
        else if (traversalDir == -1 && currentPoint > 0) { currentPoint += traversalDir; }
        else { CheckEndTraversal(); }
    }

    private void CheckEndTraversal()
    {
        if (WaypointRef.LoopWaypoints) { currentPoint = 0; }
        else if (TraversalType == Traversal.PingPong) { traversalDir *= -1; }
        else { Debug.LogWarning("Unable to travel, no traversal type was selected."); }
    }
}