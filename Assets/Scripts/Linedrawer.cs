using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Linedrawer : MonoBehaviour
{

    public static List<GameObject> points { get; private set; }
    public static int amountOfNodes { get; private set; }

    private void Awake()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        Setup();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.hideFlags != HideFlags.None)
                continue;
            if (PrefabUtility.GetPrefabType(go) == PrefabType.Prefab || PrefabUtility.GetPrefabType(go) == PrefabType.ModelPrefab)
                continue;

            objectsInScene.Add(go);
        }

        foreach (GameObject go in objectsInScene)
        {
            if (go.GetComponent<WaypointThing>() && !points.Contains(go))
            {
                Linedrawer.AddPoint(go);
            }
        }

        Linedrawer.points.Sort(Linedrawer.SortByName);

    }



    public static void Setup()
    {
        if (points == null) { points = new List<GameObject>(); amountOfNodes = 0; }

    }

    public static void AddPoint(GameObject pointToAdd)
    {
        points.Add(pointToAdd);
        amountOfNodes++;
    }

    public static void RemovePoint(GameObject pointToRemove)
    {
        points.Remove(pointToRemove);
        amountOfNodes--;
        amountOfNodes = Mathf.Clamp(amountOfNodes, 0, int.MaxValue);
    }

    public static int SortByName(GameObject o1, GameObject o2)
    {
        return o1.name.CompareTo(o2.name);
    }

    public static void ResetPointList()
    {
        points = new List<GameObject>();
        amountOfNodes = 0;
    }

    private void OnDrawGizmos()
    {
        if (points.Count > 0)
        {

            Vector3[] lineSegments = new Vector3[points.Count * 2];
            int lastObject = points.Count - 1;
            Vector3 prevPoint;
            if (points[lastObject])
            {
                prevPoint = points[lastObject].transform.position;
            }
            else
            {
                prevPoint = Vector3.zero;
            }

            int pointIndex = 0;
            for (int currObjectIndex = 0; currObjectIndex < points.Count; currObjectIndex++)
            {
                //Find position of our connected object and store it
                Vector3 currentPoint;
                if (points[currObjectIndex]) { currentPoint = points[currObjectIndex].transform.position; }
                else { currentPoint = Vector3.zero; }

                //Store starting point
                lineSegments[pointIndex] = prevPoint;
                pointIndex++;

                //Store the ending point of the line segement
                lineSegments[pointIndex] = currentPoint;
                pointIndex++;

                prevPoint = currentPoint;
            }

            Handles.DrawLines(lineSegments);
        }


    }
}
