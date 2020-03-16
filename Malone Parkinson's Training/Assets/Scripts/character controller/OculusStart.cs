/*
	checks for Oculus headset and sets up controller accordingly
*/

using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class OculusStart : MonoBehaviour {

	// public variables
	public GameObject	keyStart,
						oculusStart;
	public HideCursor	cursorControl;

	// local variables
	bool oculus;

	void Start() {

		if(UnityEngine.XR.XRDevice.isPresent) {

			// headset was detected
			Debug.Log("Oculus detected.");
			oculus = true;

			// set oculus controls, disable keyboard controls
			keyStart.SetActive(false);
			oculusStart.SetActive(true);

			// hide cursor
			//cursorControl.Hide();
		}
		else {

			// headset was not detected
			Debug.Log("Oculus not detected.");
			oculus = false;

			// set keyboard controls, disable oculus controls
			keyStart.SetActive(true);
			oculusStart.SetActive(false);
		}
	}
}
