using UnityEngine;
using System.Collections;

public class changeNavMeshDestination : MonoBehaviour {

	public GameObject navMeshToMove;
	public GameObject navDestination;
	public GameObject gameObjectEventOfOccurence; // which event gameobject will cause this navMesh to move when it's current
	public GameObject[] multipleEventsOfOccurence; // Ojects where the events will occur for diffeernt roles
	public bool movesForMultipleEvents = false;

	private EventManager eventManagerRef;
	private bool onlyTriggerOnce = false; //Makes sure it only sets the destination once
	private bool reachedDestination; // becomes true if the navMesh agent reaches its desitnation
	
	Animator anim;

	//bool hasRotated = false; //makes sure it will only rotate once.
	//public bool alsoChangeRotation; // set wheter this will also change the navMesh's rotation to the destination rotation
	//public float rotateSpeed = 10; // how fast the object will rotate

	//for testing
	//public bool moveNavMeshNow = false;

	// Use this for initialization
	void Start () {

		anim = navMeshToMove.GetComponent<Animator>();

		if(gameObjectEventOfOccurence.GetComponent<EventManager>() != null) {
			//Debug.Log("got Event!");
			eventManagerRef = gameObjectEventOfOccurence.GetComponent<EventManager>();
		}
	}
	
	// Update is called once per frame
	void Update () {

		//for testing 
		/*
		if(moveNavMeshNow && !onlyTriggerOnce) {
			navMeshToMove.GetComponent<NavMeshAgent>().destination = navDestination.transform.position;
			onlyTriggerOnce = true;
		}
		*/

		setNavDestiination();
		setAnim();
		
		//Couldn't get rotating to work
		/*
		if((navMeshToMove.transform.position == navDestination.transform.position) && alsoChangeRotation && !hasRotated) {
			
		}
		*/

	}

	
	void setNavDestiination() {
		// checks if there are multiple events to check and if it hasn't already triggered and if one of the events is the current one
		if(multipleEventsOfOccurence != null && movesForMultipleEvents && !onlyTriggerOnce) {
			foreach(GameObject events in multipleEventsOfOccurence) {

				// gets the current eventmanager of the gameObject
				if(events.GetComponent<EventManager>() != null) {
					eventManagerRef = events.GetComponent<EventManager>();
				}

				if(eventManagerRef.current) {
					navMeshToMove.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = navDestination.transform.position;
					onlyTriggerOnce = true;
					reachedDestination = true;
					break;
				}
				//onlyTriggerOnce = false;
			}
		}

		// checks if there is only one event to move during and if it hasn't already triggered and the event is the current one
		else if(!movesForMultipleEvents && !onlyTriggerOnce && eventManagerRef.current) {
			navMeshToMove.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = navDestination.transform.position;
			onlyTriggerOnce = true;
		}


		// check to see if the navmesh has been moved from its destination. If it has, move it back.
		if(navMeshToMove.GetComponent<UnityEngine.AI.NavMeshAgent>().remainingDistance > 0.3f && reachedDestination) {
			reachedDestination = false;
			onlyTriggerOnce = false; // sets the navMeshDestination again and makes it travel back to its intended spot.
		}
	}

	void setAnim() {
		//makes the character walk if they are far enough from the destination
		if(navMeshToMove.GetComponent<UnityEngine.AI.NavMeshAgent>().remainingDistance > 1f) {
			anim.SetBool("waiting", false);
		}
		else {
			anim.SetBool("waiting", true);
		}

	}
	//was going to be used to rotate the object when it reached its destination
	//couldn't get it to work in the end
	/*
	void rotateMesh() {
		Debug.Log("rotated");
		Quaternion lookRotation = navDestination.transform.rotation;
		navMeshToMove.transform.rotation = Quaternion.Slerp(navMeshToMove.transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
		hasRotated = true;
	}
	*/
}
