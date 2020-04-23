using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An event, which uses the microphone to pick up if someone is speaking.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MicrophoneEvent : InfoBoardEvent
{
    // ======================================================== Variables
    private AudioSource audioSource;         // The audio source.
    private bool validMic = false;           // Does this have a valid microphone?

    public bool loudFlag = false;
    public bool quietFlag = false;

    // ======================================================== Methods
    /// <summary>
    /// Initializes this pickup event.
    /// </summary>
    protected override void Initialize()
    {
        // Get the audio source component attached to this object
        audioSource = GetComponent<AudioSource>();

        // Call base intialize
        base.Initialize();
    }

    /// <summary>
    /// When the event should go...
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum)
    {
        // Check if we have a valid mic
        ////////////////////////////////////////////// validMic = (Microphone.devices.Length > 0);

        // If we do, then record it
        if (validMic)
            RecordAudio();

        // Else, wait 5 seconds then advance
        else
            StartCoroutine(WaitFiveSeconds());

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting Microphone Event: Event #" + myEventNum);
    }

    private void RecordAudio()
    {
        // Get a recording from the primary microphone
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 3, 44100);
        audioSource.loop = true;
        audioSource.volume = 0.0f;

        // Play back the recording
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
        float[] samples = new float[audioSource.clip.samples * audioSource.clip.channels];

        //if (audioSource.clip.GetData())
        //{

        //}
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked()
    {
        
    }

    /// <summary>
    /// When event is finished...
    /// </summary>
    public override void Finished()
    {
        base.Finished();
    }

    /// <summary>
    /// A backup coroutine, which waits 5 seconds then advances if no microphones are found.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitFiveSeconds()
    {
        yield return new WaitForSeconds(5);
        Clicked();
    }
}
