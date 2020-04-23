using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Signifies that this is the first event in the event series.
/// </summary>
public class FirstEvent : MonoBehaviour {
    // ======================================================== Variables

    // ======================================================== Methods
    /// <summary>
    /// Initalize the first event.
    /// </summary>
    //void Start () {
    //    // wait a few frames for all the start routines to run
    //    Invoke("GetStarted", 0.05f);
    //}

    /// <summary>
    /// Starts the first event.
    /// </summary>
    public void GetStarted()
    {
        // Call the event attached to this GameObject, setting the previous event number to 0
        GetComponent<InfoBoardEvent>().Go(0);
    }

    public void StartFirstEvent()
    {
        // wait a few frames for all the start routines to run
        Invoke("GetStarted", 0.05f);
    }
}
