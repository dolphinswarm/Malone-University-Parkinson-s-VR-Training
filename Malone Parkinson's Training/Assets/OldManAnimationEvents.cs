using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script for containing an old man's animation events.
/// </summary>
public class OldManAnimationEvents : MonoBehaviour
{
    // ======================================================== Variables
    [Header("Sounds")]
    public AudioClip groan;

    // ======================================================== Methods
    /// <summary>
    /// Plays the old man's groan sound.
    /// </summary>
    public void PlayGroanSound()
    {
        AudioSource.PlayClipAtPoint(groan, transform.position);
    }
}
