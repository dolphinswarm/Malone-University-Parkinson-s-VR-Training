using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for skipping a question.
/// </summary>
public class ClickToSkip : InteractiveText {
    // ======================================================== Properties
    public GameObject clickHandler;

    // ======================================================== Methods
    protected override void Select() {
        //transform.parent.gameObject.SendMessage("Skip", SendMessageOptions.DontRequireReceiver);
        clickHandler.GetComponent<SkipHandler>().Skip();
    }

    // respond to mouse clicks for mouse/keyboard
    //void OnMouseDown() {
    //    Skip();
    //}


    // respond to VR aiming reticle collisions + button press
    //void OnTriggerStay(Collider other) {
    //    // check for input
    //    if (Input.GetButtonUp("Fire1")) {
    //        // respond to button release
    //        Skip();
    //    }
    //}
}