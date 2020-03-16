using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// A generic event.
/// </summary>
[DisallowMultipleComponent]  // shouldn't have multiple event as components of the same gameObject
public class InfoBoardEvent : MonoBehaviour {
    // ======================================================== Variables
    [Header("Info Board")]
    public InfoBoardUI infoBoard;               // Which message board are we displaying to?
    public string infoText = "";                // Text to display on infromation board
    public string helpReminderText = "";        // Text to display in help / reminder popup

    [Header("Game Manager")]
    public GameSettings gameManager;

    [Header("Event Properties")]
    public InfoBoardEvent nextEvent;            // should an event be triggered after this?
    public bool isActive = false;               // is this event active?  (e.g., info is shown, audio is playing, end not yet reached)
    protected bool completed = false;           // has this event been completed?
    protected float startTime;                  // log the starting time
    public int myEventNum;                      // track the event number
    private bool useEventNum = true;            // Should the event num be used?
    public AudioClip voiceOver;                 // none by default
    public AudioClip completedSFX;              // The sound effect which plays when a player completes a task
    public float delayBeforeAdvance;            // The duration before starting the next event
    public bool isTutorialEvent = false;        // This is event a tutorial event?

    [Header("GameObjects to Show / Hide")]
    public List<GameObject> showOnStart;
    public List<GameObject> hideOnStart;
    public List<GameObject> showAtEnd;
    public List<GameObject> hideAtEnd;

    //public GameObject[] showOnStart;
    //public GameObject[] hideOnStart;
    //public GameObject[] showAtEnd;
    //public GameObject[] hideAtEnd;

    //protected TextMesh reminderObject;


    // ======================================================== Methods
    /// <summary>
    /// Toggles each item in a provided array to be on or off.
    /// </summary>
    /// <param name="listOfThings">A list of GameObjects</param>
    /// <param name="onOff">On or off? On = true, off = false</param>
    protected void Show( List<GameObject> listOfThings, bool onOff) {
        foreach( GameObject thing in listOfThings) {
            thing.SetActive(onOff); 
        }
    }

    /// <summary>
    /// On start of game, initialize this event.
    /// </summary>
    void Start() {
        // Call the intialize script
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

    // WHAT IS CLICK FOR?
    public virtual void Clicked() { }

    /// <summary>
    /// The alter Go method, which takes a parameter for tracking the event number. Called externally to start an event.
    /// </summary>
    /// <param name="prevEventNum">The previous event number</param>
	public virtual void Go(int prevEventNum) {
        Go(); // this needs to come before toggling useEventNum to true
        useEventNum = true;

        // If not tutorial event, increment
        if (!isTutorialEvent)
            myEventNum = prevEventNum + 1;      // assign this event the next number in the sequence
    }

    /// <summary>
    /// The default Go method, which takes no parameters. Called externally to start an event.
    /// </summary>
    public virtual void Go() {
        useEventNum = false;
        isActive = true;
        //GameObject.Find("Instruction_Text").GetComponent<TextMeshPro>().text = helpReminderText;
        startTime = Time.time;

        // Show / hide things as necessary
        Show(showOnStart, true);        // if there are things to be shown, then show them
        Show(hideOnStart, false);       // if there are things to be hidden, then hide them
    }

    /// <summary>
    /// Finish this event, and begin transition to next event.
    /// </summary>
    public virtual void Finished() {
        // Mark this step as completed
        isActive = false;
        completed = true;

        // Reset the info board
        infoBoard.Reset();

        // Show / hide things as necessary
        Show(showAtEnd, true);          // if there are things to be shown, then show them
        Show(hideAtEnd, false);         // if there are things to be hidden, then hide them

        // probably should write data in each event, not here
        
        // Finally.... if there's another event after this, make it go
        if (nextEvent != null) {
            //Debug.Log("trying to start event: " + nextEvent.name);
            if (useEventNum) { nextEvent.Go(myEventNum); }  // or change nextEvent type to InfoBoardEvent
            else { nextEvent.Go(); }
        }
    }
}
