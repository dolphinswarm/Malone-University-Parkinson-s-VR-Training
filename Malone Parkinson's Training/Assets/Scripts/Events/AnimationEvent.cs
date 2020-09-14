using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Animation Event Properties")]
    public Animator animator;
    public float delayBeforePlaying = 0.1f;
    public string nameOfAnimationToPlay;

    // ======================================================== Methods
    public override void Go(int prevEventNum)
    {
        // If we have info text, add it
        if (infoText != "")
        {
            infoBoard.ShowInstructions(infoText);
        }

        // Call coroutine for playing animation
        StartCoroutine(PlayAnimation());

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting + " + name + " (Animation Event: Event #" + myEventNum + ")");
    }
    
    /// <summary>
    /// Plays a given animation.
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAnimation()
    {
        // Wait a bit to delay before playing
        yield return new WaitForSeconds(delayBeforePlaying);

        // Play the animation by hitting the trigger
        if (animator != null)
            animator.SetTrigger("HasBeenTriggered");

        // Wait the animation lenght seconds
        yield return new WaitForSeconds(GetAnimationDuration(nameOfAnimationToPlay));

        // Call Clicked
        base.Clicked();
    }

    /// <summary>
    /// Gets the length of a specified animation.
    /// </summary>
    /// <param name="animationName">The name of the animation.</param>
    /// <returns>The length of the animation.</returns>
    public float GetAnimationDuration(string animationName)
    {
        // Set a base length
        float length = 0.0f;

        // Get a list of animations and try to find a match in the animator
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == animationName)
            {
                length = ac.animationClips[i].length;
            }
        }

        // Set the delay before advance to the animation duration
        return length;
    }
}
