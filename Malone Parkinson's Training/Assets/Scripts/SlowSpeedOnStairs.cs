using UnityEngine;
using System.Collections;

public class SlowSpeedOnStairs : MonoBehaviour {

	GameObject myFPC;
	public float speedReductionFactor = 0.5f;


	// Use this for initialization
	void Start () {
		myFPC = GameObject.Find("First Person Controller");
	}
	


	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			// Slow the player down on the stairs
			myFPC.GetComponent<FirstPersonOculusControls>().speed *= speedReductionFactor;
			myFPC.GetComponent<FirstPersonOculusControls>().strafeSpeed *= speedReductionFactor;
			myFPC.GetComponent<FirstPersonOculusControls>().backupSpeed *= speedReductionFactor;
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") {
			// Speed the player back up when they leave the stairwell
			myFPC.GetComponent<FirstPersonOculusControls>().speed /= speedReductionFactor;
			myFPC.GetComponent<FirstPersonOculusControls>().strafeSpeed /= speedReductionFactor;
			myFPC.GetComponent<FirstPersonOculusControls>().backupSpeed /= speedReductionFactor;

		}
	}


}
