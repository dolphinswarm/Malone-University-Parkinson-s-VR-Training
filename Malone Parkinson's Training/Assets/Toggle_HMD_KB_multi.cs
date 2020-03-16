using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class Toggle_HMD_KB_multi : MonoBehaviour {

    public GameObject FPC_KB;
    public GameObject FPC_HMD;

    public KeyCode kb_KeyCode = KeyCode.K;
    public KeyCode hmd_KeyCode = KeyCode.O;


    public GameObject myFPC;    // camera rig....  repurpose to store currently used FPC from choices above

    //public GameObject mainFPC;  // main FPC     // ???

    //public GameObject myCam;                // probably not necessary
    //public GameObject myHeightAdj;          // probably not necessary
    //public Transform evacBasket;            // no longer necessary


    public GameObject[] manualObjects;

    //public float evacHeight_KB = -0.4f;
    //public float evacHeight_HMD = -0.7f;

    // at start, check pitch/roll of neck joint for X seconds
    //   ...if oculus is connected, these values will be non-zero
    public float autoDetectForSec = 0.1f;
    private float timeSoFar = 0.0f;
    private bool autoDetectFinished = false;
    //public float nonZeroThreshold = 0.001f;  // how many degrees is non-zero?   // probably not used

    // field of view settings
    //public float keyboardFOV = 60f;                 // FOV for each controller's camera should be pre-set
    //public float oculusFOV = 94f;
    //public float oculusFOV_DK2 = 104.5737f;

    // mouselook sensitivity settings
    //public float mouseSensitivity = 6f;
    //public float joystickSensitivity = 0.95f;       // use OVR controller defaults

    // movement speed settings
    //public float oculusMoveSpeed = 1f;              // use OVR controller defaults
    //public float kbMoveSpeed = 2f;                  // 

    // Used to store the Game Manager.
    //private GameObject gameManager;                 // this script is on the game manager... wasted variable
    private ReportCardManager reportCard;

    // flag
    public string currentMode = "hmd";

    // Use this for initialization
    void Start() {
        //if (myFPC == null) { myFPC = GameObject.Find("OVRCameraRig"); }
        myFPC = FPC_HMD;    // default to HMD mode
        //if (myHeightAdj == null) { myFPC = GameObject.Find("TrackingSpace"); }
        //if (myCam == null) { myCam = GameObject.Find("CenterEyeAnchor"); }

        // Assigns gameManager to a gameobject named "Game Manager".
        //gameManager = GameObject.Find("Game Manager");            // game manager is this gameObject
        reportCard = GetComponent<ReportCardManager>();
    }


    void GoToKBMouseControl() {
        Debug.Log("------ Switching to Keyboard and Mouse Controls ------");
        reportCard.writeLine("Using Mouse and Keyboard controls");

        // ---- Toggle FPCs appropriately ----
        // move new FPC to current one's position
        FPC_KB.transform.position = myFPC.transform.position;
        FPC_KB.transform.rotation = myFPC.transform.rotation;

        // note the new current FPC
        myFPC = FPC_KB;

        // toggle visibility
        FPC_HMD.SetActive(false);
        FPC_KB.SetActive(true);
        gameObject.GetComponent<GameSettings>().currentFPC = FPC_KB;


        // Keyboard FPC should already be set up with appropriate movement / FOV values


        // find KB helper objects and enable them
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("KeyboardMouse")) {
            i.GetComponent<MeshRenderer>().enabled = true;
            //Debug.Log("Turning on " + i.name.ToString() );
        }
        foreach (GameObject ii in manualObjects) {  // some objects are inactive, and thus won't toggle correctly
            if (ii.tag == "KeyboardMouse") {
                ii.SetActive(true);
                //Debug.Log("Turning on " + ii.name.ToString() );
            }
        }


        // find Oculus helper objects and disable them
        foreach (GameObject j in GameObject.FindGameObjectsWithTag("Oculus")) {
            j.GetComponent<MeshRenderer>().enabled = false;
        }
        foreach (GameObject jj in manualObjects) {  // some objects are inactive, and thus won't toggle correctly
            if (jj.tag == "Oculus")
            {
                jj.SetActive(false);
            }
        }
    }

    void GoToOculusMode() {
        Debug.Log("------ Switching to Oculus Controls ------");
        reportCard.writeLine("Using Virtual Reality controls");
        // O for Oculus

        // ---- Toggle FPCs appropriately ----
        // move new FPC to current one's position
        FPC_HMD.transform.position = myFPC.transform.position;
        FPC_HMD.transform.rotation = myFPC.transform.rotation;

        // note the new current FPC
        myFPC = FPC_HMD;

        // toggle visibility
        FPC_KB.SetActive(false);
        FPC_HMD.SetActive(true);
        gameObject.GetComponent<GameSettings>().currentFPC = FPC_HMD;


        // HMD FPC should already be set up with appropriate movement / FOV values

        // recenter view
        UnityEngine.XR.InputTracking.Recenter();

             
        // Reset FPC orientation to zero ...for consistent tracking???
        myFPC.transform.rotation = Quaternion.identity;

        // find Oculus helper objects and enable them
        foreach (GameObject k in GameObject.FindGameObjectsWithTag("Oculus")) {
            k.GetComponent<MeshRenderer>().enabled = true;
            //Debug.Log("Turning on " + k.name.ToString() );
        }
        foreach (GameObject kk in manualObjects) {  // some objects are inactive, and thus won't toggle correctly
            if (kk.tag == "Oculus")
            {
                kk.SetActive(true);
                //Debug.Log("Turning on " + kk.name.ToString() );
            }
        }

        // find Kb helper objects and disable them
        foreach (GameObject m in GameObject.FindGameObjectsWithTag("KeyboardMouse")) {
            m.GetComponent<MeshRenderer>().enabled = false;
        }
        foreach (GameObject mm in manualObjects) {  // some objects are inactive, and thus won't toggle correctly
            if (mm.tag == "KeyboardMouse")
            {
                mm.SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Try to detect HMD for a brief time on startup, then quit checking
        if (!autoDetectFinished) {
            timeSoFar += Time.deltaTime;        
            if (timeSoFar <= autoDetectForSec) {
                //  check neck for non-zero pitch or roll
                if (UnityEngine.XR.XRDevice.isPresent) {
                    //  If we're in here, the HMD is present
                    autoDetectFinished = true;
                    Debug.Log("----Oculus Detected; Keeping HMD controls----");
                    GoToOculusMode();
                    currentMode = "hmd";
                }
            }
            else {
                // time's up, and no tracker was detected
                //  so stop checking
                autoDetectFinished = true;
                //  ...and switcht to keyboard mode
                Debug.Log("----No Oculus Detected; Requesting KB Mouse Controls----");
                GoToKBMouseControl();
                currentMode = "keyboard";
            }
        }


        if (Input.GetKeyDown(kb_KeyCode)) {
            GoToKBMouseControl();
            currentMode = "keyboard";

        }

        if (Input.GetKeyDown(hmd_KeyCode)) {
            GoToOculusMode();
            currentMode = "hmd";

        }
    }
}
