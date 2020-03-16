using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipAfterSuccessfulAttempt : MonoBehaviour {

    public int nTimesToTryVent = 2;
    private int nTimesSoFar = 0;

    float leftTriggerValue;
    float rightTriggerValue;
    bool isVenting = false;  // track current state, to avoid repeated inputs
    

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (CheckForInput()) {
            nTimesSoFar += 1;
        }

        if (nTimesSoFar >= nTimesToTryVent)
        {
            gameObject.GetComponent<TimedText>().unskippable = false;
        }
    }

    bool CheckForInput() {
        leftTriggerValue = Input.GetAxis("xBoxTrigger_L");
        rightTriggerValue = Input.GetAxis("xBoxTrigger_R");

        // ignore repeated inputs from axes by using the 'isVenting' flag
        // reset it here when the axes return to zero
        // this shouldn't matter for the keyboard, since it responds to keyDown events only
        // the settings (sensitivity = 100) should force trigger values to be zero or 1
        if (leftTriggerValue == 0 && rightTriggerValue == 0) { isVenting = false; }

        // respond to venting events here
        if (!isVenting) {  // only check keyboard and axes if we were not already venting
            if (Input.GetKeyDown(KeyCode.Space) || leftTriggerValue > 0.5 || rightTriggerValue > 0.5) {
                isVenting = true;
                return true;
            }
        }
        return false;
    }

}
