using UnityEngine;
using System.Collections;

public class testNavMeshFollow : MonoBehaviour {

	UnityEngine.AI.NavMeshAgent navAgent; // the navmesh agent for this object
	Transform target; // the target's transform
	float distance; // the distance between the target's transform and this object's transform.
	public float maxDistance = 4; //how the far the object is allowd to be from the player
	Animator thisAnimator; // the animator of thi object

	// Use this for initialization
	void Start () {
		thisAnimator = gameObject.GetComponent<Animator>();
		navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

	}

	// Update is called once per frame
	void Update () {

		handleNavMesh();
	}

	/*
	 * Gets the distance betweent eh layer and this object and checks if its too far away or not.
	 * if it is, move the navmesha nd update the animator
	 * else, we stop the navmesh and update the animatior 
	 */
	void handleNavMesh() {
		distance = Vector3.Distance(transform.position, target.transform.position);
		if(distance >= maxDistance) {
			print("out of range");
			navAgent.Resume();
			navAgent.SetDestination(target.position);
			thisAnimator.SetFloat("speed", 1);
		}
		else {
			print("in range");
			navAgent.Stop();
			thisAnimator.SetFloat("speed", 0);
		}
	}
}
