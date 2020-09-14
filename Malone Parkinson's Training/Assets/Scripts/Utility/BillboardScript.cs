using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Billboards this object towards the main camera.
/// </summary>
public class BillboardScript : MonoBehaviour
{
    // ======================================================== Variables

    // ======================================================== Methods
    /// <summary>
    /// Face the main camera on each frame.
    /// </summary>
    void Update()
    {
        if (Camera.main != null)
            transform.LookAt(Camera.main.transform);
    }
}
