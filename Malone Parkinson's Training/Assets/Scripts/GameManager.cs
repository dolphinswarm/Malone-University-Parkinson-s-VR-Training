using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OVR;

/// <summary>
/// Enumeration for the control type.
/// </summary>
[System.Serializable]
public enum ControlType { MOUSE_KEYBOARD, OCULUS };

/// <summary>
/// Enumeration for the dominant hand, if using Oculus.
/// </summary>
[System.Serializable]
public enum DominantHand { LEFT, RIGHT };

/// <summary>
/// The state of the simulation.
/// </summary>
[System.Serializable]
public enum SimState { PRETUTORIAL, TUTORIAL, SIMULATION };

//*****************************MainMenu Additions
[System.Serializable]
public enum NextMenuAction { None, Tutorial, VRS1, VRS2, VRS3, VRS4 };

/// <summary>
/// A script for managing global settings of the simulation.
/// </summary>
public class GameManager : MonoBehaviour {
    // ======================================================== Properties
    [Header("Graphical Settings")]
    public float highlightPct = 0.1f;                           // The highlight percentage of a color  
    public Color selectedColor = Color.green;                   // The default "selected" color
    public Color normalColor = Color.gray;                      // The default "normal" (standard) color
    public Color rightColor = Color.green;                      // The default "right answer" color
    public Color wrongColor = Color.red;                        // The default "wrong answer" color
    public Material hightlightMaterial;                         // The highlight material for "highlit" objects


    [Header("Audio Settings")]
    public AudioClip correctSFX;                                // The default "right answer" sound effect
    public AudioClip wrongSFX;                                  // The default "wrong answer" sound effect
    public AudioClip BGM;                                       // The background music for the simulation
    protected AudioSource audioPlayer;                          // The audio player for playing sounds


    [Header("Player Settings")]
    public ControlType controlType = ControlType.OCULUS;        // The default control type
    public GameObject currentFPC;                               // Current first person controller
    private GameObject mouseController;                         // The game's mouse controller
    private GameObject ovrController;                           // The game's OVR controller
    public DominantHand dominantHand = DominantHand.RIGHT;      // The dominant hand of the player
    private GameObject leftReticle;                             // The left hand reticle
    private GameObject rightReticle;                            // The right hand reticle
    public GameObject currentReticle;                           // The active reticle
    public GameObject currentHand;                              // The current hand
    public GameObject offHand;                                  // The OFF hand
    //public bool hideCursor = false;                           // Should the cursor be hidden?


    [Header("Game Flow")]
    public SimState currentState;                               // The current state of the simulation.
    public ReportCardManager reportCardManager;                 // The report card manager, for collecting user metrics
    public InfoBoardUI infoBoard;                               // The info board.
    public FirstEvent firstEvent;                               // The first event in the chain
    public GameObject controlChooseEvent;                       // The event which houses the control selection stuff.
    public bool skipControlChoose;                              // Skip choosing the control settings?
    public bool skipHandSelection;                              // Skip hand selection?
    public GameObject tutorialParentEvent;                      // The event which houses the tutorial stuff.
    public bool skipTutorial;                                   // Skip the tutorial?
    public OVRScreenFade screenFader;                           // Oculus screen fade script
    public Transform tutorialStartPosition;                     // The start position of the tutorial
    public Transform simStartPosition;                          // The start position of the simulaiton


    [Header("Clipboard and Scoring Mangaing")]
    public GameObject clipboardUI;                              // The clipboard object for the camera.
    public OVRInput.RawButton targetButtonType;                 // The OVR button type to check for.
    public KeyCode targetKey;                                   // The keyboard type to check for.


    // Private stuff for managing control select
    private GameObject settingsText;
    private GameObject ovrButton;
    private GameObject mouseButton;
    private GameObject leftButton;
    private GameObject rightButton;
    private float waitTimer = 10.0f;
    private float time = 0.0f;

    // ======================================================== Methods
    /// <summary>
    /// When the game is started, initialize the game settings.
    /// </summary>
    void Start()
    {
        // Print to console
        Debug.Log("==================================================== Starting simulation!");

        // Immediately set the game state to pre-tutorial
        currentState = SimState.PRETUTORIAL;

        // Immediately disable both controllers
        mouseController = GameObject.Find("Mouse and Keyboard Player Controller");
        mouseController.SetActive(false);
        ovrController = GameObject.Find("OVR Player Controller");
        ovrController.SetActive(false);

        // If info board is not set, then set it
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>(); // Should only be one InfoBoardUI (hopefully)

        // If the control choose event is present, set it to active
        if (controlChooseEvent != null)
        {
            controlChooseEvent.SetActive(true);
        }
        // Else, if not present, try to find it
        else
        {
            controlChooseEvent = GameObject.FindGameObjectWithTag("ControlSelectStep");
        }

        // Find the tutorial event, if not present
        if (tutorialParentEvent == null)
            tutorialParentEvent = GameObject.FindGameObjectWithTag("TutorialStep");

        // If we should skip tutorial, set it to inactive and get the first regular event
        if (skipTutorial)
        {
            tutorialParentEvent.SetActive(false); // Hides this first event from the scene
            firstEvent = FindObjectOfType<FirstEvent>();
        }
        // Else, make the first event this item
        else
        {
            firstEvent = tutorialParentEvent.GetComponentInChildren<FirstEvent>();
        }

        // Get the audio player and set its song
        audioPlayer = GetComponent<AudioSource>();
        if (BGM != null)
        {
            audioPlayer.clip = BGM;
            audioPlayer.volume = 0.25f;
            audioPlayer.loop = true;
        }

        // Set the default control type and dominant hand, if skipped
        if (skipControlChoose)
        {
            // Set control types
            controlType = ControlType.OCULUS;
            dominantHand = DominantHand.RIGHT;
            ovrController.SetActive(true);

            // Proceed to title screen
            StartTutorial();
        }
        // Else, get the UI elements and wait until a button is selected
        else
        {
            settingsText = GameObject.Find("Settings Text");
            ovrButton = GameObject.Find("Oculus Button");
            mouseButton = GameObject.Find("Mouse and Keyboard Button");
            leftButton = GameObject.Find("Left Hand Button");
            leftButton.SetActive(false);
            rightButton = GameObject.Find("Right Hand Button");
            rightButton.SetActive(false);
        }
    }

    // ---------- BUTTON SCRIPTS ----------
    /// <summary>
    /// Method for the buttons to select Oculus controls.
    /// </summary>
    public void SelectOculusControls()
    {
        // Set the control type
        controlType = ControlType.OCULUS;

        // Proceed to the title screen, if skip bool flag toggled
        if (skipHandSelection)
        {
            StartTutorial();
        }
        // Hide both buttons and show the hand buttons
        else
        {
            settingsText.GetComponent<Text>().text = "Select your dominant hand:";
            ovrButton.SetActive(false);
            mouseButton.SetActive(false);
            leftButton.SetActive(true);
            rightButton.SetActive(true);
        }
    }

    /// <summary>
    /// Method for the buttons to select Mouse / Keyboard controls.
    /// </summary>
    public void SelectMouseKeyboardControls()
    {
        // Set the control type
        controlType = ControlType.MOUSE_KEYBOARD;

        // Proceed to the title screen
        StartTutorial();
    }

    /// <summary>
    /// Method for the buttons to select the right hand as the dominant hand.
    /// </summary>
    public void SelectRightHand()
    {
        // Set the dominant hand
        dominantHand = DominantHand.RIGHT;

        // Proceed to the title screen
        StartTutorial();
    }

    /// <summary>
    /// Method for the buttons to select the left hand as the dominant hand.
    /// </summary>
    public void SelectLeftHand()
    {
        // Set the dominant hand
        dominantHand = DominantHand.LEFT;

        // Proceed to the title screen
        StartTutorial();
    }

    /// <summary>
    /// Method for starting the display of the title screen
    /// </summary>
    private void StartTutorial()
    {
        // Immediately change the game state
        currentState = SimState.TUTORIAL;

        // Hide the cursor
        HideCursor(true);

        // Enable the appropriate camera
        // Mouse and keyboard
        if (controlType == ControlType.MOUSE_KEYBOARD)
        {
            mouseController.SetActive(true);
            currentFPC = mouseController;
            currentHand = GameObject.Find("Main Hand");
            offHand = GameObject.Find("Off Hand");
            currentReticle = GameObject.FindGameObjectWithTag("Reticle");
        }
        // Oculus
        else
        {
            ovrController.SetActive(true);
            currentFPC = ovrController;
            leftReticle = GameObject.Find("Pointer-Left");
            rightReticle = GameObject.Find("Pointer-Right");

            // Hide the appropriate reticle
            if (dominantHand == DominantHand.RIGHT)
            {
                currentReticle = rightReticle;
                leftReticle.SetActive(false);
                currentHand = GameObject.Find("hand_right");
                offHand = GameObject.Find("hand_left");
            }
            else
            {
                currentReticle = leftReticle;
                rightReticle.SetActive(false);
                currentHand = GameObject.Find("hand_left");
                offHand = GameObject.Find("hand_right");
            }
        }

        // Disable the UI and Camera
        GameObject.FindGameObjectWithTag("ControlSelectStep").GetComponentInChildren<Camera>().gameObject.SetActive(false); // Probably more efficient way to do this?
        GameObject.FindGameObjectWithTag("ControlSelectStep").GetComponentInChildren<Canvas>().gameObject.SetActive(false); // Ditto

        // Get the screen fader of the appropriate camera, if not set
        if (screenFader == null)
            screenFader = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OVRScreenFade>();

        // Set the camera position of each interactive object
        Transform cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        foreach (InteractiveObject interactiveObject in Resources.FindObjectsOfTypeAll(typeof(InteractiveObject)))
            interactiveObject.SetCameraPos(cameraTransform);

        // Attach the UI to the main camera
        clipboardUI.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
        clipboardUI.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
        clipboardUI.SetActive(false);

        // Do things depending on if the tutorial should be skipped
        if (skipTutorial)
        {
            StartSimulation(null);
        }
        // Else, do tutorial as normal
        else
        {
            // Move the info board to the first location
            infoBoard.gameObject.transform.position = GameObject.Find("Info Board Location #1").transform.position;
            infoBoard.gameObject.transform.rotation = GameObject.Find("Info Board Location #1").transform.rotation;

            // Move the FPC to the start room
            CharacterController charCont = currentFPC.GetComponent<CharacterController>();
            charCont.enabled = false;                                     // All this is necessary to move the character controller
            charCont.transform.position = tutorialStartPosition.position; // Else, it will just snap back to its default position
            charCont.enabled = true;

            // Start the tutorial
            firstEvent.GetStarted();

            // Fade in the screen
            screenFader.FadeIn();
        }
    }

    /// <summary>
    /// Starts the actual simulation.
    /// </summary>
    public void StartSimulation(InfoBoardEvent callEvent)
    {
        // If call event is not null, call the callEvent's finished
        if (callEvent != null)
        {
            // Fade the user out and move them
            StartCoroutine(Fade(callEvent));
        }
        // Else, start the first event
        else
        {
            // Move the info board to the second location
            infoBoard.gameObject.transform.position = GameObject.Find("Info Board Location #2").transform.position;
            infoBoard.gameObject.transform.rotation = GameObject.Find("Info Board Location #2").transform.rotation;

            // Move the user to a new position
            CharacterController charCont = currentFPC.GetComponent<CharacterController>();
            charCont.enabled = false;
            charCont.transform.position = simStartPosition.position;
            charCont.enabled = true;

            // Fade in the screen
            screenFader.FadeIn();

            // Start the clock
            GameObject.FindObjectOfType<Clock>().StartClock();

            // Start the tutorial
            firstEvent.GetStarted();
        }
    }

    IEnumerator Fade(InfoBoardEvent callEvent)
    {
        // Fade the screen out
        screenFader.FadeOut();

        yield return new WaitForSeconds(2);

        // Move the info board to the second location
        infoBoard.gameObject.transform.position = GameObject.Find("Info Board Location #2").transform.position;
        infoBoard.gameObject.transform.rotation = GameObject.Find("Info Board Location #2").transform.rotation;

        // Move the user to a new position
        CharacterController charCont = currentFPC.GetComponent<CharacterController>();
        charCont.enabled = false;
        charCont.transform.position = simStartPosition.position;
        charCont.enabled = true;

        // Start the clock
        GameObject.FindObjectOfType<Clock>().StartClock();

        // Fade in the screen
        screenFader.FadeIn();

        // Finish call event
        callEvent.Finished();

        yield return null;
    }

    public void HideCursor(bool shouldBeHidden)
    {
        // If should be hidden...
        Cursor.visible = !shouldBeHidden;
        if (shouldBeHidden)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        // Else, show
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /// <summary>
    /// On frame update, check if we should pull up the clipboard.
    /// </summary>
    void Update()
    {
        // Check if the button is pressed
        if (OVRInput.GetDown(targetButtonType) || Input.GetKeyDown(targetKey))
        {
            clipboardUI.SetActive(!clipboardUI.activeSelf);
        } 
    }
}


