using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OVR;
using UnityEngine.EventSystems;

public class Interactive : MonoBehaviour {
    // ======================================================== Variables
    [Header("Info Board Interface")]
    public InfoBoardUI infoBoard;
    protected Color myColor;

    [Header("Game Manager")]
    public GameSettings gameManager;

    [Header("Interactive Properties")]
    public bool onlyClickOnce = false;
    public bool hasBeenClicked = false; //protected   <-- causes problems
    public bool isHighlighted = false;
    public bool matchTag = false;
    public string requiredTag = "Reticle";

    // ======================================================== Methods
    /// <summary>
    /// Initialize this interactive item.
    /// </summary>
    protected void Start() {
        // If renderer is not null, set my color to the renderer's color
        if (GetComponent<Renderer>() != null) {
            myColor = GetComponent<Renderer>().material.color;
        }

        // Use the base class initialize
        Initialize();
    }

    /// <summary>
    /// Sets up the script.
    /// </summary>
    protected virtual void Initialize()
    {
        // If info board is not set, then set it
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>(); // Should only be one InfoBoardUI (hopefully)

        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameSettings>(); // Should only be one GameManager
    }

    /// <summary>
    ///  Highlights this interactive item.
    /// </summary>
    protected virtual void Highlight() {
        if (GetComponent<Renderer>() != null) {
            GetComponent<Renderer>().material.color = infoBoard.selectedColor;
        }
    }

    /// <summary>
    /// Dims this interactive item.
    /// </summary>
    public virtual void Dim() {
        if (GetComponent<Renderer>() != null) {
            GetComponent<Renderer>().material.color = myColor;
        }
    }

    /// <summary>
    /// Overridden in children classes to do something when selected.
    /// </summary>
    protected virtual void Select() { } // OVERRIDE THIS METHOD TO DO SOMETHING WHEN SELECTED


    // ---------- MOUSE INTERACTION (NON-UI) ----------
    protected void OnMouseEnter() {if (!isHighlighted) { Highlight(); isHighlighted = true; } }
    protected void OnMouseExit() { if (isHighlighted) { Dim(); isHighlighted = false; } }
    protected void OnMouseDown() { Select(); }
    //    if (true) { // (!onlyClickOnce || (onlyClickOnce && !hasBeenClicked)) {
    //        //Debug.Log("What about you?");
    //        //hasBeenClicked = true; // only permit a single click   <-- this breaks things
    //        Select();
    //    }
    //}

    // ---------- MOUSE INTERACTION (UI) ----------
    //public override void OnPointerEnter(PointerEventData data) { if (!isHighlighted) { Highlight(); isHighlighted = true; } }
    //public override void OnPointerDown(PointerEventData data) { Select(); }

    // ---------- 3D RETICLE INTERACTION W/ COLLIDER ----------
    // Oncollision enter/exit are called once on the enter/exit event
    protected void OnCollisionEnter(Collision other) {
        //Debug.Log("Checking Collision-Enter: " + gameObject.name + " - " + other.gameObject.name);
        if (matchTag && other.gameObject.tag == requiredTag) {
            //Debug.Log("recognizes hover?");
            if (!isHighlighted) {
                Highlight();
                isHighlighted = true;
            }
        }
    }

    protected void OnCollisionExit(Collision other) {
        if (isHighlighted) {
            Dim();
            isHighlighted = false;
            //Debug.Log("Dimming OnCollisionExit - " + gameObject.name);
        }
    }


    // OncollisionStay is called every frame during a collision
    protected virtual void OnCollisionStay(Collision other) {
        if (other.gameObject.tag == requiredTag) {  //matchTag && 
                                                    // check for input

            // ** SECONDARY = RIGHT, PRIMARY = LEFT!
            if (!hasBeenClicked && ( Input.GetButtonDown("Fire1") || GetCorrectOVRInput())) { // Add regular GetButton / Get?
                hasBeenClicked = true;
                //Debug.Log("Click detected");
                // submit the answer
                //hasBeenClicked = false;
                Select();
                //Debug.Log(gameObject.name + "Has been clicked");
                // hasBeenClicked = false;
                //StartCoroutine(ReturnClickedToFalse());
            } else
            {

            }
            if (Input.GetButtonUp("Fire1") || OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger)) { hasBeenClicked = false; }


        }
    }


    // ---------- 3D RETICLE INTERACTION W/ TRIGGER REGION ----------
    protected void OnTriggerEnter(Collider other) {
        //Debug.Log("Checking Trigger-Enter: " + gameObject.name + " - " + other.gameObject.name);
        if (matchTag && other.gameObject.tag == requiredTag) {
            if (!isHighlighted) {
                Highlight();
                isHighlighted = true;
            }
            //Debug.Log("recognizes hover");
        }
    }

    protected virtual void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == requiredTag) { //matchTag && 
                                                   // check for input

            //if (((!onlyClickOnce && !hasBeenClicked) || (onlyClickOnce && !hasBeenClicked)) &&
            if (!hasBeenClicked && (Input.GetButtonDown("Fire1") || GetCorrectOVRInput())) { // Add regular GetButton / Get?
                Select();

                hasBeenClicked = true;
                // submit the answer
                
                //hasBeenClicked = false;
                //StartCoroutine(ReturnClickedToFalse());
                
            }
            if (Input.GetButtonUp("Fire1") || OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger)) { hasBeenClicked = false; }
        }
    }

    void OnTriggerExit() { hasBeenClicked = false; }
    void OnCollisionExit() { hasBeenClicked = false; }


    protected virtual void OnTriggerExit(Collider other) {
        if (isHighlighted) {
            Dim();
            isHighlighted = false;
            //Debug.Log("Dimming OnCollisionExit - " + gameObject.name);
        }
    }

    IEnumerator ReturnClickedToFalse ()
    {
        yield return new WaitForSeconds(1);
        //hasBeenClicked = false;
        StopCoroutine(ReturnClickedToFalse());
    }

    private bool GetCorrectOVRInput()
    {
        if (gameManager.dominantHand == DominantHand.RIGHT)
        {
            return OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
        }
        else
        {
            return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
        }
    }
}
