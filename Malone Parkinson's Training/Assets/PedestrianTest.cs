using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianTest : MonoBehaviour {

    public GameObject self;

    public float waitTime,      // approx. how long to wait at a destination
                        waitVariance,   // waitTime will be + or - this much randomly
                        stuckTime;      // how long a path can take before it is considered stuck
    public string destinationTag;   // tag name of destinations for this agent

    GameObject[] destinations;  // positions of destinations
    public Vector3 currentDestination; // current destination
    UnityEngine.AI.NavMeshAgent agent;          // this pedestrian's nav mesh agent
    float waitTimer,        // timer keeping track of wait time
                        stuckTimer;     // timer keepign tack of path time
    bool waiting;       // whether or not the agent is waiting

    void Start()
    {

        // define agent
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // find destination positions
        destinations = GameObject.FindGameObjectsWithTag(destinationTag);

        // start waiting
        resetWaitTimer();
        waiting = true;
        gameObject.GetComponent<Animator>().SetBool("waiting", true); //tells the animator that the character is waiting
    }

    void Update()
    {

        if (waiting)
        {

            // count down time to wait at a destination
            waitTimer -= Time.deltaTime;
            //Debug.Log ( "Wait timer: " + waitTimer.ToString() );

            if (waitTimer <= 0)
            {

                // stop waiting
                waiting = false;
                gameObject.GetComponent<Animator>().SetBool("waiting", false);
                //Debug.Log ("Not Waiting");

                // reset stuck timer
                stuckTimer = stuckTime;

                // pick a random destination to start moving towards
                currentDestination = destinations[(int)Mathf.Floor(Random.value * destinations.Length)].transform.position;
                agent.SetDestination(currentDestination);

                self.GetComponent<test_npc_avoidance>().enabled = true; //turn on npc avoidance when moving to new destination


            }

            //Debug.Log ( "I'm still waiting.");


        }
        else
        {   // if not waiting

            // pick a new destination if stuck
            stuckTimer -= Time.deltaTime;
            if (stuckTimer <= 0)
            {
                waiting = true;
                gameObject.GetComponent<Animator>().SetBool("waiting", true);
                //Debug.Log ("Waiting 'cuz I'm stuck");

            }

            // when the agent arrives, enter waiting
            if (Vector3.Distance(transform.position, currentDestination) < (agent.stoppingDistance * 2))
            {
                waiting = true;
                gameObject.GetComponent<Animator>().SetBool("waiting", true);
                //Debug.Log ("Waiting 'cuz I've arrived");
                resetWaitTimer();


                self.GetComponent<test_npc_avoidance>().enabled = false; //turn off npc avoidance when idle
            }
        }
    }

    // reset waitTimer to somewhere between waitTime+waitVariance and waitTime-waitVariance
    void resetWaitTimer()
    {
        waitTimer = waitTime + ((Random.value * waitVariance * 2) - waitVariance);
        //Debug.Log ( "New wait time: " + waitTimer.ToString() );
    }
}
