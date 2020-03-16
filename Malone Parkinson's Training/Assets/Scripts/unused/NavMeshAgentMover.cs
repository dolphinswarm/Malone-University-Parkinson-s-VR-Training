using UnityEngine;
using System.Collections;

public class NavMeshAgentMover : MonoBehaviour {

	public UnityEngine.AI.NavMeshAgent agentToMove; // the navmeshagent we are going to move
	public GameObject destinationObject; // the object that represents the destination in the scene
	private Vector3 destination; // the vecotr 3 of the postion of the destination

	// Use this for initialization
	void Start () {

		//sets the agent's destination to the position of the destinations position
		// then sets the vector3 named destination to that agent's destination
		agentToMove.destination = destinationObject.transform.position;
		destination = agentToMove.destination;

	}
	
	// Update is called once per frame
	void Update () {

		/*
		// checks to see if this object has moved. I.e if the vector recieved from vector3.Distance is 
		if(Vector3.Distance(destination, transform.position) > 1.0f) {
			destination = destinationObject.transform.position;
			agentToMove.destination = destination;
		}
		*/
	}
}
