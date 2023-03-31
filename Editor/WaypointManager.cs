using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointManager : EditorWindow
{
    private bool isAttachingBranch;
    private bool isSelectingNext;
    private bool isSelectingPrev;
    private Waypoint currentW = null;

    [MenuItem("Tools/Waypoint Editor")]
    public static void Open()
    {
        GetWindow<WaypointManager>();
    }
    
    public Transform waypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));
        if (waypointRoot == null)
        {
            EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }
        obj.ApplyModifiedProperties();
    }

    void DrawButtons()
    {
        if (isAttachingBranch)
        {
            if (GUILayout.Button("Cancel"))
            {
                isAttachingBranch = false;
            }
            EditorGUILayout.HelpBox("Attaching Branch to Waypoint "+ currentW.name+"... \n Click on a Waypoint to Attach it or Press Cancel", MessageType.Info);
            if(Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>() && Selection.activeGameObject.GetComponent<Waypoint>() != currentW)
            {
                EditorGUILayout.HelpBox("Attach to this Branch? (Waypoint " + Selection.activeGameObject.GetComponent<Waypoint>().name + ")...", MessageType.Info);
                if (GUILayout.Button("Attach To This Branch"))
                {
                    currentW.branches.Add(Selection.activeGameObject.GetComponent<Waypoint>());
                    isAttachingBranch = false; 
                }
            }
        }else if (isSelectingNext)
        {
            if (GUILayout.Button("Cancel"))
            {
                isSelectingNext = false;
            }
            EditorGUILayout.HelpBox("Selecting Next Branch of Waypoint " + currentW.name + "... \n Click on a Waypoint to Select as Next Branch or Press Cancel", MessageType.Info);
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>() && Selection.activeGameObject.GetComponent<Waypoint>() != currentW)
            {
                if (GUILayout.Button("Make This The Next Branch"))
                {
                    currentW.nextWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
                    isSelectingNext = false;
                }
            }
        }
        else if (isSelectingPrev)
        {
            if (GUILayout.Button("Cancel"))
            {
                isSelectingPrev = false;
            }
            EditorGUILayout.HelpBox("Selecting Previous Branch of Waypoint " + currentW.name + "... \n Click on a Waypoint to Select as Previous Branch or Press Cancel", MessageType.Info);
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>() && Selection.activeGameObject.GetComponent<Waypoint>() != currentW)
            {
                if (GUILayout.Button("Make This The Previous Branch"))
                {
                    currentW.previousWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
                    isSelectingPrev = false;
                }
            }
        }
        else
        {
            if (GUILayout.Button("Create Waypoint"))
            {
                CreateWaypoint();
            }
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
            {
                if (GUILayout.Button("Create Waypoint Before"))
                {
                    CreateWaypointBefore();
                }
                if (GUILayout.Button("Create Waypoint After"))
                {
                    CreateWaypointAfter();
                }
                if (GUILayout.Button("Remove Waypoint"))
                {
                    RemoveWaypoint();
                }
                if (GUILayout.Button("Create New Branch"))
                {
                    CreateBranch();
                }
                if (GUILayout.Button("Remove Branches"))
                {
                    RemoveBranchesFromWaypoint();
                }
                if (GUILayout.Button("Attach Branch"))
                {
                    currentW = Selection.activeGameObject.GetComponent<Waypoint>();
                    Selection.activeGameObject = null;
                    isAttachingBranch = true;
                }
                if (GUILayout.Button("Select Next Waypoint"))
                {
                    currentW = Selection.activeGameObject.GetComponent<Waypoint>();
                    Selection.activeGameObject = null;
                    isSelectingNext = true;
                }
                if (GUILayout.Button("Select Previous Waypoint"))
                {
                    currentW = Selection.activeGameObject.GetComponent<Waypoint>();
                    Selection.activeGameObject = null;
                    isSelectingPrev = true;
                }
            }
        }
    }
    void CreateWaypointBefore()
    {
        GameObject waypointObject = new GameObject("Waypoint "+waypointRoot.childCount,typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);
        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;
        newWaypoint.width = selectedWaypoint.width;
        if (selectedWaypoint.previousWaypoint != null)
        {
            newWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            selectedWaypoint.previousWaypoint.nextWaypoint = newWaypoint;
        }
        newWaypoint.nextWaypoint = selectedWaypoint;

        selectedWaypoint.previousWaypoint = newWaypoint;

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
        Selection.activeGameObject = newWaypoint.gameObject;
    }
    void CreateWaypointAfter()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);
        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;
        newWaypoint.previousWaypoint = selectedWaypoint;
        newWaypoint.width = newWaypoint.previousWaypoint.width;
        if (selectedWaypoint.nextWaypoint != null)
        {
            newWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
            selectedWaypoint.nextWaypoint.previousWaypoint = newWaypoint;
            newWaypoint.previousWaypoint = selectedWaypoint;
            selectedWaypoint.nextWaypoint = newWaypoint;
        }
        selectedWaypoint.nextWaypoint = newWaypoint;
        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;
    }
    void RemoveWaypoint()
    {
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
        if(selectedWaypoint.nextWaypoint != null)
        {
            selectedWaypoint.nextWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
        }
        if(selectedWaypoint.previousWaypoint != null)
        {
            selectedWaypoint.previousWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
            Selection.activeGameObject = selectedWaypoint.previousWaypoint.gameObject;
        }
        Waypoint[] waypoints = GameObject.FindObjectsOfType<Waypoint>();
        foreach (Waypoint way in waypoints)
        {
            if (way.branches.Count>0 && way.branches.Contains(selectedWaypoint))
            {
                way.branches.Remove(selectedWaypoint);
            }
        }
        DestroyImmediate(selectedWaypoint.gameObject);
    }

    void CreateWaypoint()
    {
        GameObject waypointsObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointsObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointsObject.GetComponent<Waypoint>();
        if(waypointRoot.childCount > 1)
        {
            waypoint.previousWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>();
            waypoint.previousWaypoint.nextWaypoint = waypoint;
            //Place waypoint at the last position
            waypoint.width = waypoint.previousWaypoint.width;
            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }

        Selection.activeGameObject = waypoint.gameObject;
    }

    void CreateBranch()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        Debug.Log(waypoint.name);
        Waypoint branchedFrom = Selection.activeGameObject.GetComponent<Waypoint>();
        Debug.Log(branchedFrom.name);

        branchedFrom.branches.Add(waypoint); 
        waypoint.transform.position = branchedFrom.transform.position;
        waypoint.transform.forward = branchedFrom.transform.forward;

        Selection.activeGameObject = waypoint.gameObject;
    }

    void RemoveBranchesFromWaypoint()
    {
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
        selectedWaypoint.branches.Clear();
    }

}
