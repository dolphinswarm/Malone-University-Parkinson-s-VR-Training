using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateScript : MonoBehaviour {

	// This script forces the object to face towards the player

	private Transform playerTransform;
	private Transform thisTransform;
	public float damping = 2f; //  how fast to turn

	Vector3 positionToLook;
	
	// Use this for initialization
	void Start () {
		thisTransform = gameObject.transform;
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	//  makes the object ALWAYS face towards the player
	void Update() {
		rotateToPlayer();
	}

	//  makes this object face towards the player
	public void rotateToPlayer() {
		//  print("roating");
		positionToLook = playerTransform.position - thisTransform.position;
		positionToLook.y = 0;
		Quaternion rotation = Quaternion.LookRotation(positionToLook);
		thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, rotation, Time.deltaTime * damping);
	}
}
