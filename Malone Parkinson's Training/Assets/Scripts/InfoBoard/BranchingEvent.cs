using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(InfoBoardEvent))]
public class BranchingEvent : MonoBehaviour {

    public GameSettings myGameManager;

    public InfoBoardEvent nextEvent_MonitorTech;
    public InfoBoardEvent nextEvent_RespTherapist;
    public InfoBoardEvent nextEvent_Nurse;


    // Use this for initialization
    void Start () {

        // Set the game manager
        myGameManager = FindObjectOfType<GameSettings>(); //GameObject.Find("Game Manager").GetComponent<GameSettings>();

        // Check in with game manager to see what the current role is... 
	    //switch (myGameManager.jobRole){

     //       // and set the next event appropriately for each role
     //       case JobRole.MonitorTech:
     //           GetComponent<InfoBoardEvent>().nextEvent = nextEvent_MonitorTech;
     //           break;
     //       case JobRole.RespTherapist:
     //           GetComponent<InfoBoardEvent>().nextEvent = nextEvent_RespTherapist;
     //           break;
     //       case JobRole.Nurse:
     //           GetComponent<InfoBoardEvent>().nextEvent = nextEvent_Nurse;
     //           break;
     //   } 	    //switch (myGameManager.jobRole){

     //       // and set the next event appropriately for each role
     //       case JobRole.MonitorTech:
     //           GetComponent<InfoBoardEvent>().nextEvent = nextEvent_MonitorTech;
     //           break;
     //       case JobRole.RespTherapist:
     //           GetComponent<InfoBoardEvent>().nextEvent = nextEvent_RespTherapist;
     //           break;
     //       case JobRole.Nurse:
     //           GetComponent<InfoBoardEvent>().nextEvent = nextEvent_Nurse;
     //           break;
     //   } 
        

	}
}
