using UnityEngine;
using System.Collections;

public class TextLookAtPlayer : MonoBehaviour {

	public GameObject target; //object to Behaviour looked at
	private Transform targetTrans;

	// Use this for initialization
	void Start () {
        // THIS SHOULD BE UPDATED!!!!
        // - search for the game manager, and ask it which FPC / camera to use.
        if (target == null) {
            target = GameObject.Find("CenterEyeAnchor");  // was "Main Camera"
            if (target == null) {
                target = GameObject.Find("Main Camera");
            }
        }
        targetTrans = target.transform; // update this
    }
	
	// Update is called once per frame
	void Update () {

		// The target should be the main camera, and the position is updated every frame to be the same height
		// as the main camera so that there is no weird tilting. Additionally the text twists to look at the
		// player every frame.
		if(target != null) {
            //Debug.Log("Rotating to face " + target.name + " -- " + transform.localEulerAngles.ToString());
			//this.transform.position = new Vector3(this.transform.position.x, targetTrans.position.y, this.transform.position.z);
			transform.LookAt(targetTrans);
		}
	}
}
