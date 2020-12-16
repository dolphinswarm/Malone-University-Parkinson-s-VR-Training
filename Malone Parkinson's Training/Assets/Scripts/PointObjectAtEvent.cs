using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A dummy class for use in the list. Tracks whether objects have been pointed at
/// </summary>
[Serializable]
public class PointAtObject
{
    // ======================================================== Variables
    public GameObject gameObject;
    public bool hasBeenPointedAt = false;
    public float pointAtDuration = 0.0f;

    private float timePointedSoFar = 0.0f;

    // ======================================================== Methods
    public void SetTime(float time) { timePointedSoFar = time; }

    public float GetTime() { return timePointedSoFar; }
}


/// <summary>
/// An event where an object needs to be pointed at one or more other objects.
/// </summary>
public class PointObjectAtEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Point At Object Event Properties")]
    public Animator animator;                // The animator for playing animations.
    public GameObject pointerObject;
    public List<PointAtObject> objectsToPointAt;
    [Range(0.5f, 1.0f)]
    public float anglePrecision = 0.9f;
    [Range(0.01f, 10.0f)]
    public float minDistance = 1.0f;
    public bool interactWithAllObjects = false;
    public bool requireButtonPress = false;
    public KeyCode keyButton = KeyCode.Z;
    public OVRInput.RawButton oVRButton = OVRInput.RawButton.RIndexTrigger;

    // ======================================================== Methods
    /// <summary>
    /// Initializes this pickup event.
    /// </summary>
    protected override void Initialize()
    {
        // Call base intialize
        base.Initialize();
    }

    /// <summary>
    /// When the event should go...
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum)
    {
        // If we have info text, add it
        if (infoText != "")
        {
            infoBoard.ShowInstructions(infoText);
        }

        // Hide the reticles
        hideOnStart.Add(gameManager.currentReticle);
        showAtEnd.Add(gameManager.currentReticle);

        // Go to base event
        base.Go(prevEventNum);

        // Update to left hand, if applicable
        if (gameManager.dominantHand == DominantHand.LEFT && oVRButton == OVRInput.RawButton.RIndexTrigger)
        {
            oVRButton = OVRInput.RawButton.LIndexTrigger;
        }
            
        // Print message to console
        Debug.Log("*** Starting + " + name + " (Point-At Event: Event #" + myEventNum + ")");
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked()
    {
        // Check if we should continue or not. if we should...
        if ((interactWithAllObjects && CheckChildrenForPointed()) || !interactWithAllObjects)
        {
            // Check animation
            if (animator != null && animationName != "")
                SetAnimation(animator, animationName);

            // If we have an animator, play the animation
            if (animator != null)
                animator.SetTrigger("HasBeenTriggered");

            // Call base clicked
            base.Clicked();
        }
        // 
        else
        {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(infoBoard.minorCorrectSFX);
        }
    }

    /// <summary>
    /// When event is finished...
    /// </summary>
    public override void Finished()
    {
        base.Finished();
    }

    /// <summary>
    /// On frame update, check against each object for pointing.
    /// </summary>
    void Update()
    {
        // If the script is active...
        if (isActive)
        {
            // If we have a mouse and keyboard controller, wait check if we
            if (gameManager.controlType == ControlType.MOUSE_KEYBOARD)
            {
                foreach (PointAtObject pointObject in objectsToPointAt)
                {
                    // Calculate the vector between the two
                    Vector3 vector = gameManager.mouseController.transform.position - pointerObject.transform.position;

                    // Find the distance and angle
                    float dist = vector.magnitude;
                    float angle = Vector3.Dot(vector.normalized, gameManager.mouseController.transform.forward);

                    // Check distance and angles. If good...
                    if (dist <= minDistance && angle >= anglePrecision)
                    {
                        // Increment the time
                        pointObject.SetTime(pointObject.GetTime() + Time.deltaTime);

                        // If time is sufficient, mark as good
                        if (pointObject.GetTime() >= pointObject.pointAtDuration)
                        {
                            // If we require a button press and the key is down, activate
                            if ((requireButtonPress && (Input.GetKeyDown(keyButton) || OVRInput.GetDown(oVRButton))) || !requireButtonPress)
                            {
                                pointObject.hasBeenPointedAt = true;
                                Clicked();
                            }
                        }
                    }
                }
                return;
            }

            // For each gameobject...
            foreach (PointAtObject pointObject in objectsToPointAt)
            {
                if (!pointObject.hasBeenPointedAt)
                {
                    // Calculate the vector between the two
                    Vector3 vector = pointObject.gameObject.transform.position - pointerObject.transform.position;

                    // Find the distance and angle
                    float dist = vector.magnitude;
                    float angle = Vector3.Dot(vector.normalized, pointerObject.transform.forward);

                    // Check distance and angles. If good...
                    if (dist <= minDistance && angle >= anglePrecision)
                    {
                        // Increment the time
                        pointObject.SetTime(pointObject.GetTime() + Time.deltaTime);

                        // If time is sufficient, mark as good
                        if (pointObject.GetTime() >= pointObject.pointAtDuration)
                        {
                            // If we require a button press and the key is down, activate
                            if ((requireButtonPress && (Input.GetKeyDown(keyButton) || OVRInput.GetDown(oVRButton))) || !requireButtonPress)
                            {
                                pointObject.hasBeenPointedAt = true;
                                Clicked();
                            }
                        }
                    }
                    // Else, reset time
                    else
                    {
                        pointObject.SetTime(0.0f);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check this event's interactive objects, if we need to interact with all.
    /// </summary>
    /// <returns></returns>
    private bool CheckChildrenForPointed()
    {
        // Create a temp bool
        bool haveAllbeenSelected = true;

        // Loop through each interactive object, checking to see if any are still interactable
        foreach (PointAtObject pointObject in objectsToPointAt)
        {
            // If we find one, break
            if (!pointObject.hasBeenPointedAt)
            {
                haveAllbeenSelected = false;
                break;
            }
        }

        // Return the value
        return haveAllbeenSelected;
    }
}
