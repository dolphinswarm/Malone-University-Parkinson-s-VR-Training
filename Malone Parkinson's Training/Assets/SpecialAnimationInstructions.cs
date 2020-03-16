using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class for special animations after an event.
/// </summary>
public class SpecialAnimationInstructions : MonoBehaviour
{
    // ======================================================== Variables
    public InfoBoardEvent owningEvent; // The owning event.

    // ======================================================== Methods
    /// <summary>
    /// The animation - should be overwritten.
    /// </summary>
    public virtual void PlayAnimation() { }
}
