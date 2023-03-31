using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "IconWaypoint", menuName = "ScriptableObjects/IconWaypoint", order = 1)]
public class MapWaypointIconsSO : ScriptableObject
{
    public GameObject prefab;
    public float iconSize = 1;
    public Sprite icon = null;
}