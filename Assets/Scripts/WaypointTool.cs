using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EditorTools;
using UnityEditor;

public class WaypointTool : EditorWindow
{
    [SerializeField] private float waypointSphereRadius = 1f;

    [SerializeField] private Color lineColour = Color.white;
    [SerializeField] private Color waypointColour = Color.white;

    private SerializedObject serializedObject;


    [MenuItem("Tools/Waypoint Tool")]
    public static void ShowWindow() => GetWindow<WaypointTool>("Waypoint Tool");

    [System.Obsolete]
    private void OnEnable()
    {
        //Retrieve data if it exists, or save default values.
        var data = EditorPrefs.GetString("WaypointTool", JsonUtility.ToJson(this, false));

        //Apply them to this window
        JsonUtility.FromJsonOverwrite(data, this);

        Linedrawer.Setup();

        List<GameObject> objectsInScene = new List<GameObject>();

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
            if (go.GetComponent<WaypointThing>() && !Linedrawer.points.Contains(go))
            {
                Debug.Log($"{go.name}");
                Linedrawer.AddPoint(go);
            }
        }

        Linedrawer.points.Sort(Linedrawer.SortByName);


        Selection.selectionChanged += Repaint;
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        //Get the Json data
        var data = JsonUtility.ToJson(this, false);

        //And save them
        EditorPrefs.SetString("WaypointTool", data);

        Selection.selectionChanged -= Repaint;
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    private void DuringSceneGUI(SceneView sceneView)
    {
        UpdateNodes();
    }


    //Actual tool code here!
    private void OnGUI()
    {

        GUILayout.Label("Colours", EditorStyles.boldLabel);
        //Add a Text:
        lineColour = EditorGUILayout.ColorField("Line Colour", lineColour);
        waypointColour = EditorGUILayout.ColorField("Waypoint Colour", waypointColour);

        EditorGUILayout.Space();

        GUILayout.Label("Waypoint Sphere radius", EditorStyles.boldLabel);
        //Add more texties here
        waypointSphereRadius = EditorGUILayout.Slider(waypointSphereRadius, 0.01f, 10f);


        EditorGUILayout.Space();

        GUILayout.Label("Cool buttons", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Node"))
            AddNode();

        using (new EditorGUI.DisabledScope(Selection.gameObjects.Length == 0))
        {
            if (GUILayout.Button("Remove Node"))
                RemoveNode();
        }

        GUILayout.EndHorizontal();
        if (GUILayout.Button("Clear points!"))
            ClearPoints();

    }

    private void AddNode()
    {
        GameObject node = new GameObject($"{Linedrawer.amountOfNodes}");

        if (node != null)
        {
            node.AddComponent<WaypointThing>();
            WaypointThing nodeComp = node.GetComponent<WaypointThing>();
            nodeComp.SetColour(waypointColour);
            nodeComp.SetSphereRadius(waypointSphereRadius);

            Linedrawer.AddPoint(node);
        }

    }

    private void UpdateNodes()
    {
        if (Linedrawer.points.Count > 0 && Linedrawer.points != null)
        {
            foreach (GameObject go in Linedrawer.points)
            {
                WaypointThing nodeComp = go.GetComponent<WaypointThing>();
                nodeComp.SetColour(waypointColour);
                nodeComp.SetSphereRadius(waypointSphereRadius);
            }
        }
    }

    private void RemoveNode()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Linedrawer.RemovePoint(go);
            EditorApplication.delayCall += () => DestroyImmediate(go);
        }


    }

    private void ClearPoints()
    {
        foreach (GameObject go in Linedrawer.points)
        {
            EditorApplication.delayCall += () => DestroyImmediate(go);
        }

        Linedrawer.ResetPointList();

    }

}
