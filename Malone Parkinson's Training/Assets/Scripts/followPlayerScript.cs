using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Makes sure this object has a nav mesh agent 
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class followPlayerScript : MonoBehaviour {

	GameObject playerRef;
	Transform playerTransform;
	[SerializeField]
	UnityEngine.AI.NavMeshAgent agent;
	float distanceFromPlayer;
	public float stopAt = 4.5f;
	public float teleportAt = 15f;
	public GameObject teleportPoint;
	// public float spawnDistance = -20f; // how far the follower spawn from the player when they teleport

	// Use this for initialization
	void Start () {
		playerRef = GameObject.FindGameObjectWithTag("Player");
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.stoppingDistance = stopAt; //  sets what distance the nav agent will stop
        teleportPoint = GameObject.Find("Follow_TeleportTarget");
	}
	
	// Update is called once per frame
	void Update () {
		followPlayer();		
	}

	// enables the navmesh to follow player
	// teleports if the distance is greater than 'teleportAt'
	void followPlayer() {
		agent.destination = playerTransform.position;
        float curDistance = getDistanceBetweenPlayer();

        if (curDistance > teleportAt) {
			print("teleporting");
			teleportNearPlayer();
		}

        else if (curDistance <= stopAt * .95f) {
            // trigger idle animation
            gameObject.GetComponent<Animator>().SetBool("waiting", true); //tells the animator that the character is waiting
        }
        else {
            gameObject.GetComponent<Animator>().SetBool("waiting", false ); //tells the animator that the character is moving
        } 
			/*
			 * disegard this segment
			// teleports follower behind player if too far
			if(getDistanceBetweenPlayer() > teleportAt) {
				gameObject.transform.position = 
					GameObject.FindGameObjectWithTag("Player").transform.position;
				gameObject.transform.position.Set(
					gameObject.transform.position.x,
					gameObject.transform.position.y,
					gameObject.transform.position.z - spawnDistance);
			}
			*/
		}

	//  returns the distance between the player and the follower
 	float getDistanceBetweenPlayer() {
		float distance = Vector3.Distance(gameObject.transform.position, playerTransform.position);
		//Debug.Log("Distance from player: " + distance);
		return distance;
	}

	//  teleports the follower to the teleport object on the player
	void teleportNearPlayer() {
        gameObject.GetComponent<NavMeshAgent>().Warp(teleportPoint.transform.position);
        //gameObject.transform.position = teleportPoint.transform.position;
	}
}
