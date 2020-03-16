using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidingDoor_TextToggle : MonoBehaviour {

    public slidingDoorToggle_2 doorManager;
    public bool requireTag = true;
    public string targetTag = "Player";

    void OnTriggerEnter(Collider other) {
        //print ("enter sliding door col");
        if (!requireTag || (requireTag && other.CompareTag(targetTag))) {
            doorManager.ShowText(true);
        }
    }


    void OnTriggerExit(Collider other) {
        //print ("Exited sliding door col"); 
        if (!requireTag || (requireTag && other.CompareTag(targetTag))) {
            doorManager.ShowText(false);
        }
    }
}
