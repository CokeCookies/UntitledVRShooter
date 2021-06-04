using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Basic FSM with states for moving, attacking (i.e. stop and melee) and dead
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent enemyNavMeshAgent;
    [SerializeField] private Transform player;
    [SerializeField] private int maxHealth;
    private int health;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private int headshotMultiplier = 10;
    [SerializeField] Color defaultColour;
    private float colourCooldown = 0.2f;
    [SerializeField] private SphereCollider meleeAttackZone;

    private enum State { Pathfinding, Attacking, Dead };
    private State state;

    IEnumerator PathfindingState()
    {
        Debug.Log("Enter: pathfinding state");

        while (state == State.Pathfinding)
        {
            //Pathfind to player
            //You really want to be pathfinding not directly to the player, but to a point on the edge of the melee zone (e.g. x distance from player position)
            enemyNavMeshAgent.SetDestination(player.position);

            if (ColourChangeCooldown())
                //Debug.Log("cooldown complete");
                GetComponent<MeshRenderer>().material.color = defaultColour;

            yield return 0;
        }

        Debug.Log("Exit: pathfinding state");
    }

    IEnumerator AttackingState()
    {
        Debug.Log("Enter: attacking state");
        enemyNavMeshAgent.isStopped = true;   //stop pathfinding
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;    //negate velocity

        while (state == State.Attacking)
        {
            if (ColourChangeCooldown())
                //Debug.Log("cooldown complete");
                GetComponent<MeshRenderer>().material.color = Color.cyan;

            yield return 0;
        }

        Debug.Log("Exit: attacking state");
    }

    IEnumerator DeadState()
    {
        Debug.Log("Enter: dead state");

        //Disable colliders (for body and head) and set a timer for self-destruction
        StartCoroutine(DestroySelfTimer());
        //Enemy falls and disappears
        //Deleted from scene

        while (state == State.Dead)
        {
            GetComponent<MeshRenderer>().material.color = Color.grey;
            enemyNavMeshAgent.enabled = false;  //disables navmesh agent ... Is this the right approach?
            yield return 0;
        }

        Debug.Log("Exit: dead state");
        //No need for an exit transition
    }

    void Start()
    {
        //Initializa enemy stats
        health = maxHealth;
    }

    void Awake()
    {
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        state = State.Pathfinding;
        StartCoroutine(PathfindingState());
        meleeAttackZone = GetComponent<SphereCollider>();
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
        //Debug.Log("Trigger!");
        Debug.Log("attack zone!");
        state = State.Attacking;
        StartCoroutine(AttackingState());

        //THIS ISN'T OKAY

        /*
        if (other == meleeAttackZone)
        {
            Debug.Log("attack zone!");
            state = State.Attacking;
            StartCoroutine(AttackingState());
        }    */ 
    }
}