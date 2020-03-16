using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An event for picking up an object
/// </summary>
public class PickupEventOld : InfoBoardEvent {
    // ======================================================== Variables
    [Header("Pickup Properties")]
    public InteractiveObjectOld interactionNode;
    public bool hideAfterPickup = true;
    public GameObject pickupObject;

    // ======================================================== Methods
    /// <summary>
    /// Initializes this pickup event.
    /// </summary>
    protected override void Initialize() {
        // If the pickup object is not set, get it
        if (pickupObject == null)
            pickupObject = GetComponentInChildren<InteractiveObject>().GetGameObject();

        // Set the interaction node's info board and callback object
        interactionNode.infoBoard = infoBoard;
        interactionNode.owningEvent = this;

        // Ensure that visual indicators and click-functionality are disabled until needed
        interactionNode.transform.parent.gameObject.SetActive(false);

        // If info board is not set, then set it
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>(); // Should only be one InfoBoardUI (hopefully)

        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameSettings>(); // Should only be one GameManager
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked() {
        // Log stuff to file here

        // Hide Item that is picked up if requested
        if (hideAfterPickup) {
            pickupObject.SetActive(false);
            
        }

        // Else, attach to grabbed hand
        else
        {
            if (gameManager.controlType == ControlType.OCULUS)
            {
                pickupObject.transform.SetParent(GameObject.Find("hand_right").transform);
            }
        }

        // play sound effect  &  move on to next thing
        if (completedSFX != null)
        {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(completedSFX);
            Invoke("Finished", completedSFX.length + delayBeforeAdvance);
        }
        else if (infoBoard.correctSFX != null)
        {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(infoBoard.correctSFX);
            Invoke("Finished", infoBoard.correctSFX.length + delayBeforeAdvance);
        }
        else { Invoke("Finished", delayBeforeAdvance); }
    }
   
    public override void Go(int prevEventNum) {

        //infoBoard.SetInfo(gameObject.GetComponent<InfoObject>());  // pass a reference to this script

        interactionNode.transform.parent.gameObject.SetActive(true);
        if (infoText != null) {
            infoBoard.ShowInstructions(infoText);
        }
        if (voiceOver != null) {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(voiceOver);
        }

        if (gameManager.controlType == ControlType.OCULUS)
        {
            showOnStart.Add(gameManager.currentReticle);
            hideAtEnd.Add(gameManager.currentReticle);
            //hideOnStart.Add(gameManager.currentReticle);
            //showAtEnd.Add(gameManager.currentReticle);
        }

        base.Go(prevEventNum);
    }

    public override void Finished() {
        interactionNode.transform.parent.gameObject.SetActive(false);
        base.Finished();
    }




}
