using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PedestrianController : MonoBehaviour {

	public float		waitTime,		// approx. how long to wait at a destination
						waitVariance,	// waitTime will be + or - this much randomly
						stuckTime;		// how long a path can take before it is considered stuck
	public string		destinationTag;	// tag name of destinations for this agent
	
	GameObject[]		destinations;	// positions of destinations
	Vector3				currentDestination;	// current destination
	UnityEngine.AI.NavMeshAgent		agent;			// this pedestrian's nav mesh agent
	float				waitTimer,		// timer keeping track of wait time
						stuckTimer;		// timer keepign tack of path time
	bool				waiting;		// whether or not the agent is waiting

    //*********************************************************************************** Extra Task Animations Additions ********
    bool atTypingLoc;

    Transform agentTransform;

    //declare a var and eventually make it equal to destination position of current dest
    int destInt = 3;
    int newDest;
    List<int> otherDestinations = new List<int>();


    //Slerping rotation stuff
    public float rotSpeed;

    bool doRotation;


   

    //*************************************

    void Start() {

		// define agent
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        //**********************************************************************************
        agentTransform = GetComponent<Transform>();

        
        //*********************************

		// find destination positions
		destinations = GameObject.FindGameObjectsWithTag(destinationTag);

		// start waiting
		resetWaitTimer();
		waiting = true;
		gameObject.GetComponent<Animator>().SetBool("waiting",true); //tells the animator that the character is waiting
	}

	void Update() {

		if(waiting) {

			// count down time to wait at a destination
			waitTimer -= Time.deltaTime;
            //Debug.Log ( "Wait timer: " + waitTimer.ToString() );

            doRotation = true;



			if(waitTimer <= 0) {
				
				// stop waiting
				waiting = false;
				gameObject.GetComponent<Animator>().SetBool("waiting",false);
                //Debug.Log ("Not Waiting");


                //*********************************************************** Stopping typing sooner
                /*if (atTypingLoc == true)
                {
                    atTypingLoc = false;
                    gameObject.GetComponent<Animator>().SetBool("atTypingLoc", false);
                }*/

                doRotation = false;


                // reset stuck timer
                stuckTimer = stuckTime;

                // pick a random destination to start moving towards

                /* for (int i = 0; i < destinations.Length; i++) //********************************************** see if this works to not repeat the same destination //update...currently does exact opposite
                 {

                     
                     if(i != destInt) {
                        
                         otherDestinations.Add(i);

                     }


                 }                                           //change back to otherDest.Count if you feel you have a chance 
                

                 destInt = newDest;*/
                //may have to make the list outside of update. In that case, we may need to somehow wipe out all values in it 
                newDest = (int)Mathf.Floor(Random.value * destinations.Length);
                    currentDestination = destinations[newDest].transform.position;
                    agent.SetDestination(currentDestination);
                    destInt = newDest;
              
               
            }

            //Debug.Log ( "I'm still waiting.");


        } else {   // if not waiting

			// pick a new destination if stuck
			stuckTimer -= Time.deltaTime;
			if(stuckTimer <= 0) {
				waiting = true;
				gameObject.GetComponent<Animator>().SetBool("waiting",true);
				//Debug.Log ("Waiting 'cuz I'm stuck");

			}

			// when the agent arrives, enter waiting
			if(Vector3.Distance(transform.position, currentDestination) < (agent.stoppingDistance * 1)) { //changed 2 to 1
				waiting = true;
				gameObject.GetComponent<Animator>().SetBool("waiting",true);
				//Debug.Log ("Waiting 'cuz I've arrived");
				resetWaitTimer();
			}
		}
	}
	
	// reset waitTimer to somewhere between waitTime+waitVariance and waitTime-waitVariance
	void resetWaitTimer() {
		waitTimer = waitTime + ((Random.value * waitVariance * 2) - waitVariance);
		//Debug.Log ( "New wait time: " + waitTimer.ToString() );
	}



    //*********************************************************** Extra Task Animations*************************************
    void OnTriggerEnter(Collider other)
    {
        //put overarching list of tags in an if statement (not sure if more or less efficient than just listing under each tag)
        //then change rotation

       /* if (other.tag == destinationTag)
        {
            agentTransform.rotation = Quaternion.Lerp(agentTransform.rotation, other.transform.rotation, Time.time * rotSpeed);
        }*/

        if (other.tag == "TypingLocation")
        {
            //agentTransform.rotation = other.transform.rotation;
            atTypingLoc = true;
            gameObject.GetComponent<Animator>().SetBool("atTypingLoc", true);
        }

        if (other.tag == destinationTag)      //This wont work here because it is overridden by having to continue moving
        {
            //agentTransform.rotation = Quaternion.Lerp(agentTransform.rotation, other.transform.rotation, Time.time * rotSpeed);
            //^^^will make perpetually slower //will it be noticable/bothersome is the question
            //StartCoroutine(BeginRotLerp());
        }
    }

    void OnTriggerStay(Collider other)
    {
        
        if (other.tag == "TypingLocation")
        {
                                                                  
                             
                                                                                                                                                   
            atTypingLoc = true;
            gameObject.GetComponent<Animator>().SetBool("atTypingLoc", true);

        }

        if (other.tag == destinationTag)
        {

            //begunRotation = true; us if using update for Slerp



            if (doRotation == true && agentTransform.rotation != other.transform.rotation)
            {
                agentTransform.rotation = Quaternion.Slerp(agentTransform.rotation, other.transform.rotation, 1 * rotSpeed);
            
            }
         
        }
    }


    void OnTriggerExit(Collider other) //using this can make it look weird when npc is leaving but not yet completely out of trigger
    {
        if (other.tag == "TypingLocation")
        {
            atTypingLoc = false;
            gameObject.GetComponent<Animator>().SetBool("atTypingLoc", false);
        }
    }
}
