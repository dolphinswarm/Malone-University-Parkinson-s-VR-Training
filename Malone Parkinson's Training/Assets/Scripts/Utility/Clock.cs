using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script for managing the clock's time and appearance.
/// </summary>
public class Clock : MonoBehaviour
{
    // ======================================================== Variables
    [Header("Clock Properties")]
    [Range(0, 11f)]
    public int hours = 0;
    [Range(0, 59f)]
    public int minutes = 0;
    public GameObject hourHand;
    public GameObject minuteHand;
    public GameObject secondHand;
    public AudioClip clockTick;
    public bool isClockRunning = false;

    private Coroutine clockCoroutine;

    // ======================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // If second hand is not attached, get it
        if (hourHand == null)
            hourHand = GameObject.Find("Clock:Hour");

        // If second hand is not attached, get it
        if (minuteHand == null)
            minuteHand = GameObject.Find("Clock:Minute");

        // If second hand is not attached, get it
        if (secondHand == null)
            secondHand = GameObject.Find("Clock:Second");
    }

    /// <summary>
    /// Starts the clock's second hand animation
    /// </summary>
    public void StartClock()
    {
        isClockRunning = true;
        clockCoroutine = StartCoroutine(MoveSecondsHand());
    }

    /// <summary>
    /// Stops the clock's animation.
    /// </summary>
    public void StopClock()
    {
        isClockRunning = false;
        if (clockCoroutine != null) StopCoroutine(clockCoroutine);
    }

    /// <summary>
    /// Sets the clock to a specified time.
    /// </summary>
    /// <param name="newHours">The hour parameter.</param>
    /// <param name="newMinutes"></param>
    public void SetTime(int newHours, int newMinutes)
    {
        // Set the current hour and minutes
        hours = newHours;
        minutes = newMinutes;

        // Apply the rotation
        hourHand.transform.rotation = Quaternion.Euler(new Vector3((30.0f * hours) + ((minutes / 60.0f) * 30.0f), 0.0f, 0.0f)); // Add some extra time to hour hand
        minuteHand.transform.rotation = Quaternion.Euler(new Vector3(6.0f * minutes, 0.0f, 0.0f));
    }

    /// <summary>
    /// Couroutine for moving the seconds hand. Rotate it 1/60, then wait a second.
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveSecondsHand()
    {
        while (isClockRunning)
        {
            secondHand.transform.Rotate(Vector3.right, 6.0f); // 1/60th of 360 degrees
            AudioSource.PlayClipAtPoint(clockTick, transform.position, 0.5f);
            yield return new WaitForSeconds(1);
        }
    }
}
