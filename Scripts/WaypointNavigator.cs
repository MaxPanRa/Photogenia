 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WaypointNavigator : MonoBehaviour
{
    public GameObject waypointsObject;
    NavMeshAgent navMeshAgent;
    [SerializeField] bool hasQuest = false;
    [SerializeField] private Waypoint prevWaypoint = null;
    public Waypoint currentWaypoint;
    [SerializeField] bool isDebugging = false;
    private bool startedDespawn = false;

    Animator agentAnimator;
    int direction;
    Vector3 nextDest;

    private void Awake()
    {
        
        
    }
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        agentAnimator = GetComponent<Animator>();
        waypointsObject = GameObject.FindGameObjectWithTag("Waypoints");
        currentWaypoint = waypointsObject.transform.GetChild(Random.Range(0, waypointsObject.transform.childCount - 1)).GetComponent<Waypoint>();
        transform.position = currentWaypoint.transform.position;
        CheckCharacterNotLooking();
        nextDest = currentWaypoint.GetPosition();
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
        navMeshAgent.SetDestination(nextDest);
    }
    private void CheckCharacterNotLooking()
    {
        Renderer renderer = this.transform.Find("Geometry").GetComponentInChildren<Renderer>();
        if (renderer.isVisible)
        {
            currentWaypoint = waypointsObject.transform.GetChild(Random.Range(0, waypointsObject.transform.childCount - 1)).GetComponent<Waypoint>();
            transform.position = currentWaypoint.transform.position;
            CheckCharacterNotLooking();
        }
        else
        {
            return;
        }
    }

    void Update()
    {
        if (!hasQuest && PedestrianSpawner.instance != null && !startedDespawn)
        {
            startedDespawn = true;
            Invoke("Despawn", PedestrianSpawner.instance.pedestrianLifetime);
        }
        if (isDebugging) {
            //Debug.Log("Velocidad " + transform.name + " -- " + navMeshAgent.velocity.sqrMagnitude);
            //Debug.Log(Vector3.Distance(transform.position, nextDest));
            //Debug.Log(transform.position);
            //Debug.Log(nextDest+" "+currentWaypoint.name);
            //Debug.Log("Distance: "+Vector3.Distance(transform.position, nextDest));
        }
        if (navMeshAgent.velocity.sqrMagnitude < 0.1f && navMeshAgent.velocity.sqrMagnitude > -0.1f)
        {
            agentAnimator.SetBool("isRunning", false);
            agentAnimator.SetBool("isWalking", false);
        }
        else
        {
            agentAnimator.SetBool("isWalking", true);
        }
        if (currentWaypoint != null && Vector3.Distance(transform.position, nextDest) < 1.4f)
        {
            prevWaypoint = currentWaypoint;
            //Debug.Log(currentWaypoint.name + " --- " + currentWaypoint.ambiDirectonal);
            if (currentWaypoint.ambiDirectonal)
            {
                direction = Mathf.RoundToInt(Random.Range(0f, 1f));
            }
            bool shouldBranch = false;
            if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
            {
                shouldBranch = Random.Range(0f,1f) <= currentWaypoint.branchRatio;
            }
            if (shouldBranch)
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
                if(currentWaypoint.previousWaypoint == null)
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                }
                else
                {
                    currentWaypoint = currentWaypoint.previousWaypoint;
                }
            }
            else
            {
                if (direction == 0)
                {
                    if (currentWaypoint.nextWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.previousWaypoint;
                        direction = 1;
                    }
                }
                else if (direction == 1)
                {
                    if (currentWaypoint.previousWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.previousWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                        direction = 0;
                    }
                }
            }
            if (currentWaypoint)
            {
                nextDest = currentWaypoint.GetPosition();
                navMeshAgent.SetDestination(nextDest);
            }
            else
            {
                currentWaypoint = waypointsObject.transform.GetChild(Random.Range(0, waypointsObject.transform.childCount - 1)).GetComponent<Waypoint>();
                nextDest = currentWaypoint.GetPosition();
                navMeshAgent.SetDestination(nextDest);
            }
        }

        if(currentWaypoint == null)
        {
            currentWaypoint = waypointsObject.transform.GetChild(Random.Range(0, waypointsObject.transform.childCount - 1)).GetComponent<Waypoint>();
            nextDest = currentWaypoint.GetPosition();
            transform.position = currentWaypoint.transform.position;
            navMeshAgent.SetDestination(nextDest);
        }
    }

    public void Despawn(){
        Renderer renderer = this.transform.Find("Geometry").GetComponentInChildren<Renderer>();
        if (!renderer.isVisible)
        {
            if (PedestrianSpawner.instance.count > 0)
            {
                PedestrianSpawner.instance.count--;
            }
            Destroy(this.gameObject);

        }else{
            Invoke("Despawn", 3.0f);
        }
    }

}
