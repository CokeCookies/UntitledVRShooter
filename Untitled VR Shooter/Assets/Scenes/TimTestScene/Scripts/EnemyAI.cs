using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Basic FSM with states for moving, attacking (i.e. stop and melee) and dead
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent enemyNavMeshAgent;
    private GameObject player;
    [SerializeField] private int maxHealth;
    private int health;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private int headshotMultiplier = 10;
    [SerializeField] Color defaultColour;
    private float colourCooldown = 0.2f;
    [SerializeField] private SphereCollider meleeAttackZone;
    public GameObject currentRail;  //the rail that the enemy agent is currently using
    public GameObject nextRail; //the next rail the agent will teleport to
    public List<GameObject> railNodes;  //the nodes belonging to currentRail; there's no particularly good reason to do this in a list; the AI should just have an int counter, and upon reaching a node they should fetch the next one from the rail
    public GameObject target;   // the node the AI is targeting
    public int targetNumber = 0; //the number (in sequence) of the current target node
    public EnemySpawnController enemySpawnController;   //public for debugging (like everything else!)


    private enum State { Pathfinding, Attacking, Dead };
    private State state;

    IEnumerator PathfindingState()
    {
        //Debug.Log("Enter: pathfinding state");

        while (state == State.Pathfinding)
        {
            //Pathfind to player
            //You really want to be pathfinding not directly to the player, but to a point on the edge of the melee zone (e.g. x distance from player position)
            enemyNavMeshAgent.SetDestination(target.transform.position);
            Debug.DrawLine(transform.position, target.transform.position, Color.green);

            if (ColourChangeCooldown())
                //Debug.Log("cooldown complete");
                GetComponent<MeshRenderer>().material.color = defaultColour;

            yield return 0;
        }

        //Debug.Log("Exit: pathfinding state");
    }

    IEnumerator AttackingState()
    {
        //Debug.Log("Enter: attacking state");
        enemyNavMeshAgent.isStopped = true;   //stop pathfinding
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;    //negate velocity

        while (state == State.Attacking)
        {
            if (ColourChangeCooldown())
                //Debug.Log("cooldown complete");
                GetComponent<MeshRenderer>().material.color = Color.cyan;

            yield return 0;
        }

        //Debug.Log("Exit: attacking state");
    }

    IEnumerator DeadState()
    {
        //Debug.Log("Enter: dead state");

        //Disable colliders (for body and head) and set a timer for self-destruction
        StartCoroutine(DestroySelfTimer());

        //Report back to the SpawnController that you need to be removed from Enemies list

        //Enemy falls and disappears
        //Deleted from scene

        while (state == State.Dead)
        {
            GetComponent<MeshRenderer>().material.color = Color.grey;
            enemyNavMeshAgent.enabled = false;  //disables navmesh agent ... Is this the right approach?
            yield return 0;
        }

        //Debug.Log("Exit: dead state");
        //No need for an exit transition
    }

    void Start()
    {
        //Initializa enemy stats
        health = maxHealth;
    }

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        state = State.Pathfinding;
        StartCoroutine(PathfindingState());
        meleeAttackZone = GetComponent<SphereCollider>();
        enemySpawnController = GameObject.FindWithTag("SpawnController").GetComponent<EnemySpawnController>();   //get the scene's spawn controller

        GetNodePath();
        nextRail = enemySpawnController.GetNextRail(currentRail);
    }

    // Update is called once per frame
    void Update()
    {    
        switch (state)
        {
            case State.Pathfinding:

                if (health <= 0)
                {
                    state = State.Dead;
                    StartCoroutine(DeadState());
                }

                //Check for player proximity; enter attack
                //Can set dead state from outside

                break;

            case State.Attacking:

                if (health <= 0)
                {
                    state = State.Dead;
                    StartCoroutine(DeadState());
                }

                break;

            case State.Dead:

                //No need for any checks
                break;

            default:
                Debug.Log("Default case: you shouldn't be here.");
                break;
        }
    }

    //Public accessors for decrementing enemy health (via raycast hit or something)
    public void TakeDamage()
    {
        health -= baseDamage;
        GetComponent<MeshRenderer>().material.color = Color.red;
        //Debug.Log("take damage");
        colourCooldown = 0.1f;
    }

    public void TakeDamageHeadshot()
    {
        health -= baseDamage * headshotMultiplier;
        GetComponent<MeshRenderer>().material.color = Color.black;
        //Debug.Log("take headshot damage");
        colourCooldown = 0.1f;
    }

    private bool ColourChangeCooldown()
    {
        colourCooldown -= Time.deltaTime;
        return colourCooldown <= 0;
    }
    IEnumerator DestroySelfTimer()
    {
        yield return new WaitForSeconds(3);
        DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    //Upon collision with the melee attack zone trigger, transition to attacking state
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target && other.gameObject != railNodes[railNodes.Count - 1])    //i.e. if the node isn't the lst one in the current nodes
        {
            Debug.Log("I've reched node " + targetNumber + ", my target node.");
            targetNumber++;
            target = railNodes[targetNumber];
        }

        if (other.gameObject == target && other.gameObject == railNodes[railNodes.Count - 1])    //i.e. if the node isn't the lst one in the current nodes
        {
            Debug.Log("I've reached node " + targetNumber + ", the end node. Time to teleport!");
            targetNumber = 0;
            StartCoroutine(TeleportTimer());
            //Teleport to rail
        }


        //Debug.Log("Trigger!");
        //Debug.Log("attack zone!");
        //state = State.Attacking;
        //StartCoroutine(AttackingState());

        //THIS ISN'T OKAY

        /*
        if (other == meleeAttackZone)
        {
            Debug.Log("attack zone!");
            state = State.Attacking;
            StartCoroutine(AttackingState());
        }    */
    }

    void GetNodePath()
    {
        railNodes.Clear();

        for (int i = 0; i < currentRail.transform.childCount; i++)
        {
            GameObject child = currentRail.transform.GetChild(i).gameObject;
            railNodes.Add(child);
        }

        target = railNodes[0];
    }

    IEnumerator TeleportTimer()
    {
        yield return new WaitForSeconds(2);
        TeleportToNextRail();
    }

    private void TeleportToNextRail()
    {

        GameObject teleportNode = nextRail.transform.GetChild(0).gameObject;    //the first node of thetarget rail
        Vector3 targetPosition = new Vector3();
        targetPosition.x = teleportNode.transform.position.x;
        targetPosition.y = teleportNode.transform.position.y;
        targetPosition.z = teleportNode.transform.position.z;
        enemyNavMeshAgent.Warp(targetPosition);

        currentRail = nextRail;
        GetNodePath();
        nextRail = enemySpawnController.GetNextRail(currentRail);
    }
}