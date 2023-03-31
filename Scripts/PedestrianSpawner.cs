using UnityEngine;
using UnityEngine.AI;

public class PedestrianSpawner : MonoBehaviour
{
    public static PedestrianSpawner instance;
    [SerializeField] private GameObject[] pedestrians;
    [SerializeField] private int pedestriansToSpawn = 20;
    [SerializeField] private bool allowSpawn = true;
    public int count = 0;
    public int pedestrianLifetime = 30;

    void Start()
    {
        instance = this;
    }

    void Update(){
        if(!GameManager.instance.isMovementTutorial && 
            !GameManager.instance.isCameraTutorial){
            allowSpawn = true;
        }else{
            allowSpawn = false;
        }
        if(allowSpawn){
            if(count < pedestriansToSpawn){
                GameObject obj = Instantiate(pedestrians[Random.Range(0, pedestrians.Length )]);
                obj.transform.Find("Geometry").GetChild(Random.Range(0, obj.transform.Find("Geometry").childCount - 1)).gameObject.SetActive(true);
                obj.GetComponent<NavMeshAgent>().speed = Random.Range(0.8f, 2.5f);
                obj.GetComponent<NavMeshAgent>().stoppingDistance = 0;
                //Debug.Log("ChildCount = " + (transform.childCount - 1));
                Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
                //Debug.Log("Child = " + child.name);
                obj.GetComponent<WaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();
                obj.transform.position = child.position;
                count++;
            }
        }
    }
}
