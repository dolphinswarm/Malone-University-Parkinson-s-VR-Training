using UnityEngine;
using System.Collections;

public class navMeshTesting : MonoBehaviour {


	public Transform dest1;
	public Transform dest2;
	public Transform dest3;
	public Transform dest4;
	UnityEngine.AI.NavMeshAgent agent;
	Animator anim;

	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		//checks to see if the agent still has to walk a distance to reach its destination
		//if it does, it is walking. If not, then its not
		if(agent.remainingDistance > 0) {
			anim.SetFloat("walking", agent.remainingDistance);
		}
		else if (agent.remainingDistance == 0) {
			anim.SetFloat("walking", 0);
		}
		if(Input.GetKeyDown("1")) {
			agent.destination = dest1.position;
		}
		else if(Input.GetKeyDown("2")) {
			agent.destination = dest2.position;
		}
		else if(Input.GetKeyDown("3")) {
			agent.destination = dest3.position;
		}
		else if(Input.GetKeyDown("4")) {
			agent.destination = dest4.position;
		}
		
		
	}
}
