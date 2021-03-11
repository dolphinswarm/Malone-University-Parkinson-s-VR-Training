﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OVR;
using System;

/// <summary>
/// Enumeration for the control type.
/// </summary>
[Serializable]
public enum ControlType { MOUSE_KEYBOARD, OCULUS };

/// <summary>
/// Enumeration for the dominant hand, if using Oculus.
/// </summary>
[Serializable]
public enum DominantHand { LEFT, RIGHT };

/// <summary>
/// Which state should we start the game at?
/// </summary>
[Serializable]
public enum SimState { Tutorial, State1And2, State3, State4, State5, State6 }

//*****************************MainMenu Additions
[Serializable]
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
    public GameObject mouseController;                         // The game's mouse controller
    public GameObject ovrController;                           // The game's OVR controller
    public DominantHand dominantHand = DominantHand.RIGHT;      // The dominant hand of the player
    private GameObject leftReticle;                             // The left hand reticle
    private GameObject rightReticle;                            // The right hand reticle
    public GameObject currentReticle;                           // The active reticle
    public GameObject currentHand;                              // The current hand
    public GameObject offHand;                                  // The OFF hand
    public OVRHapticsManager leftHandOVRHaptics;
    public OVRHapticsManager rightHandOVRHaptics;
    //public bool hideCursor = false;                           // Should the cursor be hidden?


    [Header("Game Flow")]
    public SimState currentState;                               // The current state of the game
    public SimState startAtState;                               // The state we should start at
    public InfoBoardUI infoBoard;                               // The game's info board (should only be one)
    public List<FirstEvent> firstEvents;                        // A list of all first events, IN ORDER!
    private Dictionary<SimState, FirstEvent> firstEventDictionary;          // A private dictionary of first event / sim state associations
    public FirstEvent firstEvent;                               // The first event in the chain
    public InfoBoardEvent currentEvent;                         // The current info board event.
    public GameObject controlChooseEvent;                       // The event which houses the control selection stuff.
    public bool skipControlChoose;                              // Skip choosing the control settings?
    public bool skipHandSelection;                              // Skip hand selection?
    public GameObject tutorialParentEvent;                      // The event which houses the tutorial stuff.
    public OVRScreenFade screenFader;                           // Oculus screen fade script
    public bool playAnimations = true;                          // Should we play animations?
    public DominantHand mostRecentHandUsed = DominantHand.RIGHT;

    [Header("Clipboard and Scoring Mangaing")]
    public GameObject clipboardUI;                              // The clipboard object for the camera.
    public GameObject clipboardObject;                          // The clipboard object for the camera.
    public ClipboardScript clipboardText;                       // The script for getting / changing clipboard information.
    public GameObject clipboardUIMouseAndKeyboard;              // The clipboard object for the camera.
    public ClipboardScript clipboardTextMouseAndKeyboard;       // The script for getting / changing clipboard information.
    public DominantHand clipboardHand = DominantHand.RIGHT;     // The hand the keyboard is in.
    public OVRInput.RawButton targetButtonType;                 // The OVR button type to check for.
    public KeyCode targetKey;                                   // The keyboard type to check for.
    public ReportCardManager reportCardManager;                 // The report card manager, for collecting user metrics


    // Private stuff for managing control select
    private GameObject settingsText;
    private GameObject ovrButton;
    private GameObject mouseButton;
    private GameObject leftButton;
    private GameObject rightButton;
    private bool canUseClipboard = true;

    // Start time
    public float absoluteStartTime = 0.0f;
    public float absoluteEndTime = 0.0f;

    // ======================================================== Methods
    /// <summary>
    /// When the game is started, initialize the game settings.
    /// </summary>
    void Start()
    {
        // Print to console
        Debug.Log("==================================================== Starting simulation!");

        // Set the absolute start time
        absoluteStartTime = Time.time;

        // Immediately set the game state to pre-tutorial
        currentState = SimState.Tutorial;

        // Immediately disable both controllers
        if (mouseController == null)
            mouseController = GameObject.Find("Mouse and Keyboard Player Controller");
        mouseController.SetActive(false);

        if (ovrController == null)
            ovrController = GameObject.Find("OVR Player Controller");
        ovrController.SetActive(false);

        // If info board is not set, then set it
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>(); // Should only be one InfoBoardUI (hopefully)

        // If the control choose event is present, set it to active
        if (controlChooseEvent != null)
            controlChooseEvent.SetActive(true);
        // Else, if not present, try to find it
        else
            controlChooseEvent = GameObject.FindGameObjectWithTag("ControlSelectStep");

        // Find the tutorial event, if not present
        if (tutorialParentEvent == null)
            tutorialParentEvent = GameObject.FindGameObjectWithTag("TutorialStep");

        // Populate the "first event" dictionary
        firstEventDictionary = new Dictionary<SimState, FirstEvent>();
        firstEventDictionary.Add(SimState.Tutorial, firstEvents[0]);
        firstEventDictionary.Add(SimState.State1And2, firstEvents[1]);
        firstEventDictionary.Add(SimState.State3, firstEvents[2]);
        firstEventDictionary.Add(SimState.State4, firstEvents[3]);
        firstEventDictionary.Add(SimState.State5, firstEvents[4]);
        firstEventDictionary.Add(SimState.State6, firstEvents[5]);

        // if we should ignore animations, remove them all from game
        if (!playAnimations)
        {
            // Get all animators
            Animator[] animators = FindObjectsOfType<Animator>();

            // Foreach animator, remove the animations attached
            foreach (Animator animator in animators) animator.enabled = false;
        }

        // Get the audio player and set its song
        audioPlayer = GetComponent<AudioSource>();
        if (BGM != null)
        {
            audioPlayer.clip = BGM;
            audioPlayer.volume = 0.25f;
            audioPlayer.loop = true;
        }

        // Make the cursor visible
        HideCursor(false);

        // Set the default control type and dominant hand, if skipped
        if (skipControlChoose)
        {
            // Set control types
            if (OVRManager.isHmdPresent)
            {
                controlType = ControlType.OCULUS;
                ovrController.SetActive(true);
            }
            else
            {
                controlType = ControlType.MOUSE_KEYBOARD;
            }

            // If we also want to skip the hand selection...
            if (skipHandSelection)
            {
                // Choose right hand by default
                dominantHand = DominantHand.RIGHT;

                // Proceed to title screen
                FinishSimulationSetup();
            }
            // Else, show it
            else
            {
                // Find the needed components
                settingsText = GameObject.Find("Settings Text");
                ovrButton = GameObject.Find("Oculus Button");
                mouseButton = GameObject.Find("Mouse and Keyboard Button");
                leftButton = GameObject.Find("Left Hand Button");
                rightButton = GameObject.Find("Right Hand Button");

                // Set the components
                settingsText.GetComponent<Text>().text = "Select your dominant hand:";
                ovrButton.SetActive(false);
                mouseButton.SetActive(false);
                leftButton.SetActive(true);
                rightButton.SetActive(true);
            }



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

    /// <summary>
    /// Uses a special trigger event.
    /// </summary>
    /// <param name="eventName">The name of the calling event</param>
    public void CallTrigger(string eventName)
    {
        if (eventName == "8. Pick Up Clipboard")
        {
            canUseClipboard = true;
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
            FinishSimulationSetup();
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
        FinishSimulationSetup();
    }

    /// <summary>
    /// Method for the buttons to select the right hand as the dominant hand.
    /// </summary>
    public void SelectRightHand()
    {
        // Set the dominant hand
        dominantHand = DominantHand.RIGHT;

        // Proceed to the title screen
        FinishSimulationSetup();
    }

    /// <summary>
    /// Method for the buttons to select the left hand as the dominant hand.
    /// </summary>
    public void SelectLeftHand()
    {
        // Set the dominant hand
        dominantHand = DominantHand.LEFT;

        // Proceed to the title screen
        FinishSimulationSetup();
    }

    /// <summary>
    /// Method for starting the display of the title screen
    /// </summary>
    private void FinishSimulationSetup()
    {
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
            screenFader = Camera.main.GetComponent<OVRScreenFade>();

        // Set the camera position of each interactive object
        Transform cameraTransform = Camera.main.transform;
        foreach (InteractiveObject interactiveObject in Resources.FindObjectsOfTypeAll(typeof(InteractiveObject)))
            interactiveObject.SetCameraPos(cameraTransform);

        // Set the clipboard properties
        //clipboardUI.transform.parent = Camera.main.transform;
        //clipboardUI.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        //clipboardUI.GetComponentInChildren<Canvas>().planeDistance = 0.5f;
        //clipboardUI.SetActive(false);
        //clipboardText = clipboardUI.GetComponentInChildren<ClipboardScript>();
        if (startAtState == SimState.Tutorial) canUseClipboard = false;

        // Set which event is the first
        foreach (var item in firstEventDictionary)
        {
            if (item.Key == startAtState) 
                firstEvent = item.Value;
        }

        // Start the first event
        firstEvent.GetStarted();
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
        // Commmand for skipping the current event
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab) && currentEvent != null)
        {
            currentEvent.Finished();
        }

        // Check if we should swap the dominant hand
        if (OVRManager.isHmdPresent)
        {
            // Clipboard button
            //if (canUseClipboard && OVRInput.GetDown(targetButtonType) || Input.GetKeyDown(targetKey))
            if (canUseClipboard)
            {
                // If we want to switch to the right hand, do so
                if (OVRInput.GetDown(OVRInput.RawButton.A) && clipboardObject.activeSelf && clipboardHand == DominantHand.LEFT)
                {
                    // Get the right hand
                    var rightHand = GameObject.Find("hand_right");
                    clipboardObject.transform.parent = rightHand.transform;
                    clipboardObject.transform.localPosition = new Vector3(-0.13f, 0.098f, -0.034f);
                    clipboardObject.transform.localRotation = Quaternion.Euler(45.0f, 180.0f, 0.0f);

                    // Set the hand to right
                    clipboardHand = DominantHand.RIGHT;
                }
                // If we want to switch to the left hand, do so
                else if (OVRInput.GetDown(OVRInput.RawButton.X) && clipboardObject.activeSelf && clipboardHand == DominantHand.RIGHT)
                {
                    // Get the left hand
                    var leftHand = GameObject.Find("hand_left");
                    clipboardObject.transform.parent = leftHand.transform;
                    clipboardObject.transform.localPosition = new Vector3(0.13f, 0.098f, -0.034f);
                    clipboardObject.transform.localRotation = Quaternion.Euler(45.0f, 180.0f, 0.0f);

                    // Set the hand to left
                    clipboardHand = DominantHand.LEFT;
                }
                // Else, hide or show the clipboard
                else if ((OVRInput.GetDown(OVRInput.RawButton.A) && clipboardHand == DominantHand.RIGHT) || (OVRInput.GetDown(OVRInput.RawButton.X) && clipboardHand == DominantHand.LEFT))
                {
                    clipboardObject.SetActive(!clipboardObject.activeSelf);
                }
            }

            // Left hand
            //if (OVRInput.GetDown(OVRInput.RawButton.B))
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && dominantHand == DominantHand.LEFT && !currentEvent.hideReticles)
            {
                // Set the dominant hand
                dominantHand = DominantHand.RIGHT;

                // Mess with the reticles
                if (leftReticle.activeSelf)
                {
                    rightReticle.SetActive(true);
                    leftReticle.SetActive(false);
                }
                currentReticle = rightReticle;

                // Mess with the hands
                currentHand = GameObject.Find("hand_right");
                offHand = GameObject.Find("hand_left");
            }

            // Right hand
            //else if (OVRInput.GetDown(OVRInput.RawButton.Y))
            else if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) && dominantHand == DominantHand.RIGHT && !currentEvent.hideReticles)
            {
                // Set the dominant hand
                dominantHand = DominantHand.LEFT;

                // Mess with the reticles
                if (rightReticle.activeSelf)
                {
                    rightReticle.SetActive(false);
                    leftReticle.SetActive(true);
                }
                currentReticle = leftReticle ;

                // Mess with the hands
                currentHand = GameObject.Find("hand_left");
                offHand = GameObject.Find("hand_right");
            }
        }

        // If we don't have an Oculus controller
        else 
        {
            if (Input.GetKeyDown(KeyCode.Z) && canUseClipboard)
            {
                clipboardUIMouseAndKeyboard.SetActive(!clipboardUIMouseAndKeyboard.activeSelf);
            }
        }

    }
}


