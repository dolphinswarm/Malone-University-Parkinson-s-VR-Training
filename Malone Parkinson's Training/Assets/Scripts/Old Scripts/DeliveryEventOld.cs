using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryEventOld : InfoBoardEvent
{
    // ======================================================== Variables
    public InteractiveObjectOld interactionNode;
    public bool showAfterDelivery = true;

    // ======================================================== Methods
    // instructions info
    //public string infoText = "";            // text to display - inherited
    //public AudioClip voiceOver;             // none by default

    protected override void Initialize() {
        interactionNode.infoBoard = infoBoard;
        interactionNode.owningEvent = this;

        // ensure that visual indicators and click-functionality are disabled until needed
        interactionNode.transform.parent.gameObject.SetActive(false);
        if (showAfterDelivery) {
            interactionNode.visibleObjectParentNode.gameObject.SetActive(false);
        }
    }

    public override void Clicked() {
        interactionNode.Dim();
        // Log stuff to file here

        // Hide Item that is picked up if requested
        if (showAfterDelivery) {
            interactionNode.visibleObjectParentNode.gameObject.SetActive(true);
        }

        // play sound effect  &  move on to next thing
        if (completedSFX != null) {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(completedSFX);
            Invoke("Finished", completedSFX.length + delayBeforeAdvance);
        }
        else if (infoBoard.correctSFX != null) {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(infoBoard.correctSFX);
            Invoke("Finished", infoBoard.correctSFX.length + delayBeforeAdvance);
        }
        else { Invoke("Finished", delayBeforeAdvance); }
    }

    public override void Go(int prevEventNum) {
        interactionNode.transform.parent.gameObject.SetActive(true);
        if (infoText != null) {
            infoBoard.ShowInstructions(infoText);
        }
        if (voiceOver != null) {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(voiceOver);
        }
        base.Go(prevEventNum);
    }

    public override void Finished() {
        interactionNode.transform.parent.gameObject.SetActive(false);
        base.Finished();
    }

}
