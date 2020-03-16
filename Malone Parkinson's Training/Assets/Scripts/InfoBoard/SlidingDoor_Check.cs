using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum DoorCheck {warning, final};

[System.Serializable]
public enum DoorCheck_Dir { enter, exit };

public class SlidingDoor_Check : MonoBehaviour {

    public DoorCloseEvent doorEvent;
    public bool requireTag = true;
    public string targetTag = "Player";
    public DoorCheck checkType = DoorCheck.warning;
    public DoorCheck_Dir checkWhen = DoorCheck_Dir.enter;

    void OnTriggerEnter(Collider other) {
        // Use this code block when settings ask to check upon entering the trigger
        if (checkWhen == DoorCheck_Dir.enter) {
            // Only react when the proper collider is entering - e.g., player, but not NPCs
            if (!requireTag || (requireTag && other.CompareTag(targetTag))) {
                // Do Warning or FinalCheck as appropriate
                if (checkType == DoorCheck.warning) {
                    doorEvent.CheckDoor();
                   
                }
                else { doorEvent.FinalCheck(); }  // in this case checkType == DoorCheck.final
            }
        }
    }


    void OnTriggerExit(Collider other) {
        // Use this code block when settings ask to check upon entering the trigger
        if (checkWhen == DoorCheck_Dir.exit) {
            // Only react when the proper collider is exiting  - e.g., player, but not NPCs
            if (!requireTag || (requireTag && other.CompareTag(targetTag))) {
                // Do Warning or FinalCheck as appropriate
                if (checkType == DoorCheck.warning) { doorEvent.CheckDoor(); }
                else { doorEvent.FinalCheck(); }  // in this case checkType == DoorCheck.final
            }
        }
    }


}
