using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatorObject : Interactive
{
    // ======================================================== Variables
    public LocatorEvent owningEvent;          // The object this script is calling back to.
    private bool oneClick = false;

    // ======================================================== Methods
    /// <summary>
    /// Initialize this pickup object.
    /// </summary>
    protected override void Initialize()
    {
        // Set owning event, if null
        if (owningEvent == null)
            owningEvent = GetComponentInParent<LocatorEvent>();

        // Set info board and destination trigger
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>();

        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameSettings>(); // Should only be one GameManager

        // Set the proper looking tag
        requiredTag = "Reticle";
    }

    /// <summary>
    /// Override the base highlight and dim to do nothing
    /// </summary>
    protected override void Highlight() { }
    public override void Dim() { }

    /// <summary>
    /// On pickup...
    /// </summary>
    protected override void Select()
    {
        // If not selected...
        if (owningEvent != null && !oneClick)
        {
            // Hide this object
            oneClick = true;
            owningEvent.targets.Remove(this);
            owningEvent.Clicked();

            // Set this to inactive
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// A method for returning this game object.
    /// </summary>
    /// <returns>This script's owning game object.</returns>
    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
