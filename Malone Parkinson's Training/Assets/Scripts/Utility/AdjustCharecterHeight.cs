using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustCharecterHeight : MonoBehaviour {

    public float increment = 0.1f;  // meters per sec
    public float desiredHeight = 1.8f;  // desired height in meters

	// Update is called once per frame
	void Update () {
        // manually adjust eye height
        if (Input.GetKey(KeyCode.PageUp)) {
            // raise eye height
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + (increment*Time.deltaTime),
                transform.position.z);
        }

        if (Input.GetKey(KeyCode.PageDown)) {
            // lower eye height
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - (increment * Time.deltaTime),
                transform.position.z);
        }

        if (Input.GetKey(KeyCode.Home)) {
            // Reset eye height
            //Debug.Log("Adjusting from: " + transform.position.ToString());
            transform.position = new Vector3(
                transform.position.x,
                desiredHeight,
                transform.position.z);
        }

    }


    public void KeyboardMode(bool keyboardMode) {
        // KeyboardMode
        if (keyboardMode) {
            transform.position = new Vector3(
                transform.position.x,
                desiredHeight,
                transform.position.z);
        }
    }

}
