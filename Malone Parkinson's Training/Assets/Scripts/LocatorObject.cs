using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatorObject : InteractiveObject
{
    // ======================================================== Variables
    [Header("Locator Object Properties")]
    private bool oneClick = false;
    public bool hideOnClick = true;             // Should this object be hidden on click?

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
            gameManager = FindObjectOfType<GameManager>(); // Should only be one GameManager

        // Set the proper looking tag
        requiredTag = "Reticle";
    }

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
            if (TryGetComponent<LocatorEvent>(out LocatorEvent locator))
                locator.targets.Remove(this);
            owningEvent.Clicked();

            // Set this to inactive
            if (hideOnClick)
                gameObject.SetActive(false);
        }
    }
}
