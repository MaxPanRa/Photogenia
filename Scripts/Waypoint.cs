using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{ 
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    [Range(0f, 50f)]
    public float width = 2.8f;

    public List<Waypoint> branches;

    [Range(0f, 1f)]
    public float branchRatio = 0.1f;

    public bool ambiDirectonal = false; 

    public Vector3 GetPosition()
    {
        Vector3 minbound = transform.position + transform.right * width / 2f;
        Vector3 maxbound = transform.position - transform.right * width / 2f;
        return Vector3.Lerp(minbound, maxbound, Random.Range(0f, 1f));
    }

}
