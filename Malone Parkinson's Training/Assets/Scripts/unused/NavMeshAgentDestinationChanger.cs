using UnityEngine;
using System.Collections;

public class NavMeshAgentDestinationChanger : MonoBehaviour {

	public Transform endDestination;
	UnityEngine.AI.NavMeshAgent agent;
	Animator anim;
	// Use this for initialization
	void Start () {

		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		anim = GetComponent<Animator>();
		agent.destination = endDestination.position;
	}
	
	// Update is called once per frame
	void Update () {

		//checks to see if the agent still has to walk a distance to reach its destination
		//if it does, it is walking. If not, then its not
		if(agent.remainingDistance > 0) {
			anim.SetFloat("walking", 1);
		}
		else if(agent.remainingDistance == 0) {
			anim.SetFloat("walking", 0);
		}



		
	}
}
