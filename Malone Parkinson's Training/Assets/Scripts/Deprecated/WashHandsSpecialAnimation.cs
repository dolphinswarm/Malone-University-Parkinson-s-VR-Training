using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The special animation for hand washing.
/// </summary>
public class WashHandsSpecialAnimation : SpecialAnimationInstructions
{
    // ======================================================== Variables
    public GameObject sinkHandle;
    public ParticleSystem water;

    // ======================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        water.Stop();
    }

    /// <summary>
    /// The animation - should be overwritten.
    /// </summary>
    public override void PlayAnimation()
    {
        StartCoroutine(RunSink());
    }

    /// <summary>
    /// The coroutine for playing the hand washing animation.
    /// </summary>
    /// <returns></returns>
    IEnumerator RunSink()
    {
        // Turn the sink handle
        for (int i = 0; i < 90; i++)
        {
            sinkHandle.transform.Rotate(Vector3.up, 1.0f);
            yield return new WaitForSeconds(0.01f);
        }

        // Start the particle effects
        water.Play();

        // Wait 3 seconds
        yield return new WaitForSeconds(3);

        // Stop the particle effect
        water.Stop();

        // Turn the sink handle
        for (int i = 0; i < 90; i++)
        {
            sinkHandle.transform.Rotate(Vector3.up, -1.0f);
            yield return new WaitForSeconds(0.01f);
        }

        // Play the finish of the owning game object
        owningEvent.Finished();
        
        // Stop the coroutine
        yield return null;
    }
}
