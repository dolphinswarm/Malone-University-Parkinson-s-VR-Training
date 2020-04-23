using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goToDestination : MonoBehaviour {

	public  Transform destination1;
	public Transform destination2;
	private UnityEngine.AI.NavMeshAgent Agent;
	private Animator anims;
	private Transform playerTransform;
	[SerializeField]
	private float waitingDistance = 4;
	[SerializeField]
	private float agentMaxSpeed = 3.5f;
	private bool  waiting = false;
	private Transform thisTransform;
	public float damping = 2f;
	Vector3 positionToLook;

	// Use this for initialization
	void Start () {
		Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		anims = GetComponent<Animator>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		thisTransform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {

		stopAndWaitForPlayer();
		changeAnimation();

		if(Input.GetKeyDown("1")) {
			moveTodestination(1);
		}
		else if(Input.GetKeyDown("2")) {
			moveTodestination(2);
		}

			//rotateToPlayer();
		
	}

	float getDistanceBetweenPlayer() {
		float distance = Vector3.Distance(gameObject.transform.position, playerTransform.position);
		//Debug.Log("Distance from player: " + distance);
		return distance;
	}

	void stopAndWaitForPlayer() {

		// start to slow down if player is too far behind
		if((getDistanceBetweenPlayer() > waitingDistance) && (Agent.remainingDistance > 0)) {
			Debug.Log("Waiting for player");
			anims.SetBool("waiting", true);
			anims.SetFloat("speed", 0);
			Agent.speed = 0;
			Agent.Stop();
			waiting = true;
			rotateToPlayer();
		}

		else {
			Agent.Resume();
			anims.SetBool("waiting", false);
			waiting = false;
		}
	}

	void changeAnimation() {
		//Debug.Log("Remaining distance:" + Agent.remainingDistance);
		if((Agent.remainingDistance) > 0 && !waiting) {
			anims.SetFloat("speed", Agent.speed);
			Agent.speed = agentMaxSpeed;
		}
		
		else if(Agent.remainingDistance == 0) {
			anims.SetFloat("speed", Agent.speed);
			Agent.speed = 0;
		}
	}

	void moveTodestination(int i) {
		if(i == 1) {
			Agent.destination = destination1.position;
		}

		else if(i == 2) {
			Agent.destination = destination2.position;
		}
	}

	public void rotateToPlayer() {
		print("rotating");
		positionToLook = playerTransform.position - thisTransform.position;
		positionToLook.y = 0;
		Quaternion rotation = Quaternion.LookRotation(positionToLook);
		thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, rotation, Time.deltaTime * damping);
	}
}
