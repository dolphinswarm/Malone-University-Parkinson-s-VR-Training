using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class DeliveryManager : Interactive
{
    // ======================================================== Variables
    public InfoBoardEvent owningEvent;          // The object this script is calling back to.
    private bool oneClick = false;

    // ======================================================== Methods
    protected override void Initialize()
    {
        // Set owning event, if null
        if (owningEvent == null)
            owningEvent = GetComponentInParent<InfoBoardEvent>();

        // Set info board and destination trigger
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>();

        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameSettings>(); // Should only be one GameManager

        // Set the proper looking tag
        if (gameManager.controlType == ControlType.OCULUS)
        {
            requiredTag = "OVR Grabber";
        }
        else
        {
            requiredTag = "Reticle";
        }
    }

    protected override void Select()
    {
        if (owningEvent != null && !oneClick && owningEvent.isActive)
        {
            oneClick = true;
            owningEvent.Clicked();
        }
    }
}
