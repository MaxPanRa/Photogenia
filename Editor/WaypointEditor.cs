using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class WaypointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
    {
        float sphereSize = 1;
        if((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }
        if (waypoint.branches != null && waypoint.branches.Count > 0)
        {
            Gizmos.color = (gizmoType & GizmoType.Selected) != 0 ? Color.blue : Color.blue * 0.5f;
        }
        if (waypoint.branches != null && waypoint.branches.Count > 1)
        {
            Gizmos.color = (gizmoType & GizmoType.Selected) != 0 ? Color.green : Color.green * 0.5f;
        }
        if (waypoint.branches != null && waypoint.branches.Count > 2)
        {
            Gizmos.color = (gizmoType & GizmoType.Selected) != 0 ? Color.red : Color.red * 0.5f;
        }
        if (waypoint.branches != null && waypoint.branches.Count > 3)
        {
            Gizmos.color = (gizmoType & GizmoType.Selected) != 0 ? Color.black : Color.black * 0.5f;
        }
        sphereSize += waypoint.branchRatio*2;
        Gizmos.DrawSphere(waypoint.transform.position, .1f * sphereSize);
        Gizmos.color = Color.white;
        if (waypoint.ambiDirectonal)
        {
            Gizmos.color = Color.yellow;
        }
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width / 2f),
            waypoint.transform.position - (waypoint.transform.right * waypoint.width / 2f));

        if(waypoint.previousWaypoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.width / 2f;
            Vector3 offsetTo = waypoint.previousWaypoint.transform.right * waypoint.previousWaypoint.width / 2f;
            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.previousWaypoint.transform.position + offsetTo);
        }
        if (waypoint.nextWaypoint != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = waypoint.transform.right * -waypoint.width / 2f;
            Vector3 offsetTo = waypoint.nextWaypoint.transform.right * -waypoint.nextWaypoint.width / 2f;
            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.nextWaypoint.transform.position + offsetTo);
        }

        if(waypoint.branches != null)
        {
            foreach(Waypoint branch in waypoint.branches)
            {
                Gizmos.color = Color.blue;
                if (branch.branches != null && waypoint.branches.Contains(branch) && branch.branches.Contains(waypoint))
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawLine(waypoint.transform.position, branch.transform.position);
            }
        }
    }
}
