using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// A simple script for containing a particle system. Used by animations.
/// </summary>
public class AnimationActionContainer : MonoBehaviour
{
    // ======================================================== Variables
    [Header("Handlers")]
    private GameManager gameManager;
    public ParticleSystem particleSystemForAnimation;
    public AudioSource audioSource;
    public CapsuleCollider capsuleCollider;
    public GameObject objectToChangeMaterialOn;
    public TMP_Text text;

    // ======================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Starts a given particle system.
    /// </summary>
    public void PlayParticleSystem()
    {
        particleSystemForAnimation.Play();
    }

    /// <summary>
    /// Stops a given particle system.
    /// </summary>
    public void StopParticleSystem()
    {
        particleSystemForAnimation.Stop();
    }

    /// <summary>
    /// Plays a given sound with an attached audio source.
    /// </summary>
    /// <param name="sound">A sound.</param>
    public void PlaySound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    /// <summary>
    /// Stops the audio source from playing.
    /// </summary>
    public void StopSound()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// Turns on / off a collider.
    /// </summary>
    public void ToggleCollider()
    {
        capsuleCollider.enabled = !capsuleCollider.enabled;
    }

    /// <summary>
    /// Starts haptics on the dominant hand.
    /// </summary>
    /// <param name="val">The haptics volume.</param>
    public void StartHaptics(float val)
    {
        if (gameManager.mostRecentHandUsed == DominantHand.LEFT)
            OVRInput.SetControllerVibration(1.0f, val, OVRInput.Controller.LTouch);
        else
            OVRInput.SetControllerVibration(1.0f, val, OVRInput.Controller.RTouch);
    }

    /// <summary>
    /// Stops haptics on the dominant hand.
    /// </summary>
    public void StopHaptics()
    {
        if (gameManager.mostRecentHandUsed == DominantHand.LEFT)
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        else
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }

    /// <summary>
    /// Changes the material of an object to a new material.
    /// </summary>
    /// <param name="material">The new material to use.</param>
    public void ChangeMaterial(Material material)
    {
        objectToChangeMaterialOn.GetComponent<Renderer>().material = material;
    }

    /// <summary>
    /// Changes the text of the child text object
    /// </summary>
    /// <param name="newText"></param>
    void ChangeText(string newText)
    {
        text.text = newText;
    }
}
