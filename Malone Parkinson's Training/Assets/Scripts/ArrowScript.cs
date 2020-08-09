using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for handling arrows on the info board.
/// </summary>
public class ArrowScript : MonoBehaviour
{
    // ======================================================== Variables

    // ======================================================== Methods

    /// <summary>
    /// Sets the position of the arrow on the board.
    /// </summary>
    /// <param name="position">The position to be set to.</param>
    /// <param name="rotation">The rotation to be set to.</param>
    public void PositionArrow(Vector3 position, Vector3 rotation)
    {
        // Set the arrow's position and rotation
        transform.position = position;
        transform.rotation = Quaternion.Euler(rotation);
    }

}
