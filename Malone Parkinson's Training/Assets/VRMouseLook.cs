using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMouseLook : MonoBehaviour {

    // THIS SCRIPT CAUSES MAJOR PROBLEMS

    private float mouseX = 0;
    private float mouseY = 0;
    private float mouseZ = 0;

    public Transform pitchNode;
    public bool usePitchNode = true;

    //public bool enableYaw = false;  // OVR controller already tracks yaw, so leave this false
    public bool enablePitch = true;   // need to toggle pitch support if going to KB mode
    //public float yawGain = 5.0f;
    public float pitchGain = 2.4f;
    public float pitchMin = -85f;
    public float pitchMax = 85f;
    public bool autoRecenterPitch = true;
    //public bool autoRecenterRoll = true;


    // Update is called once per frame
    void Update() {
        // Check to see if Yaw control is enabled
        /*
        if (enableYaw) {
            //Debug.Log("I'm changing Yaw!");
            // ...if so, then calculate rotation around the Vertical (Y) axis
            mouseX += Input.GetAxis("Mouse X") * yawGain;
            // ...and make sure to correct for circularity
            if (mouseX <= -180) { mouseX += 360; }
            else if (mouseX > 180) { mouseX -= 360; }
        }
        // */

        // Check to see if Pitch control is enabled
        if (enablePitch) {
            // ...if so, then calculate rotation around the lateral (X) axis
            mouseY -= Input.GetAxis("Mouse Y") * pitchGain;
            mouseY = Mathf.Clamp(mouseY, pitchMin, pitchMax);
        }
        // if pitch movenet is disabled, lerp back to zero to recenter view
        if (!enablePitch && autoRecenterPitch)
        {
            mouseY = Mathf.Lerp(mouseY, 0, Time.deltaTime / (Time.deltaTime + 0.1f));
        }
        // Finally, apply the motion to the transform
        //if (!usePitchNode) {
            //transform.localRotation = Quaternion.Euler(mouseY, mouseX, 0);
            //transform.localRotation = Quaternion.Euler(mouseY, mouseX, transform.localRotation.z);
        //}
        if (usePitchNode) {
            //transform.localRotation = Quaternion.Euler(transform.localRotation.x, mouseX, transform.localRotation.z);  // was (0, mouseX, 0)
            //pitchNode.localRotation = Quaternion.Euler(mouseY, pitchNode.localRotation.y, pitchNode.localRotation.z);   // was (mouseY, 0,0)
            pitchNode.eulerAngles = new Vector3(mouseY, pitchNode.eulerAngles.y, pitchNode.eulerAngles.z);
        }


        /*
        // ORIGINAL CODE BELOW -- FROM https://forums.oculus.com/community/discussion/27747/mouse-look-during-development
        bool rolled = false;
        bool pitched = false;
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            pitched = true;
            if (enableYaw)
            {
                mouseX += Input.GetAxis("Mouse X") * 5;
                if (mouseX <= -180)
                {
                    mouseX += 360;
                }
                else if (mouseX > 180)
                {
                    mouseX -= 360;
                }
            }
            mouseY -= Input.GetAxis("Mouse Y") * 2.4f;
            mouseY = Mathf.Clamp(mouseY, -85, 85);
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            rolled = true;
            mouseZ += Input.GetAxis("Mouse X") * 5;
            mouseZ = Mathf.Clamp(mouseZ, -85, 85);
        }
        if (!rolled && autoRecenterRoll)
        {
            // People don't usually leave their heads tilted to one side for long.
            mouseZ = Mathf.Lerp(mouseZ, 0, Time.deltaTime / (Time.deltaTime + 0.1f));
        }
        if (!pitched && autoRecenterPitch)
        {
            // People don't usually leave their heads tilted to one side for long.
            mouseY = Mathf.Lerp(mouseY, 0, Time.deltaTime / (Time.deltaTime + 0.1f));
        }
        transform.localRotation = Quaternion.Euler(mouseY, mouseX, mouseZ);

        // */
    }


}