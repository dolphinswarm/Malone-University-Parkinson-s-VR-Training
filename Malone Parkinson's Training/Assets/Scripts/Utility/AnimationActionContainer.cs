using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple script for containing a particle system. Used by animations.
/// </summary>
public class AnimationActionContainer : MonoBehaviour
{
    // ======================================================== Variables
    private GameManager gameManager;
    public ParticleSystem particleSystemForAnimation;
    public AudioSource audioSource;
    public CapsuleCollider capsuleCollider;
    public GameObject objectToChangeMaterialOn;

    // ======================================================== Methods
    void Start() { gameManager = FindObjectOfType<GameManager>(); }

    public void PlayParticleSystem() {particleSystemForAnimation.Play(); }

    public void StopParticleSystem() { particleSystemForAnimation.Stop(); }

    public void PlaySound(AudioClip sound) {
        audioSource.clip = sound;
        audioSource.Play();
    }

    public void StopSound() { audioSource.Stop(); }

    public void ToggleCollider() { capsuleCollider.enabled = !capsuleCollider.enabled; }

    public void StartHaptics(float val)
    {
        if (gameManager.dominantHand == DominantHand.LEFT)
            OVRInput.SetControllerVibration(1.0f, val, OVRInput.Controller.LTouch);
        else
            OVRInput.SetControllerVibration(1.0f, val, OVRInput.Controller.RTouch);
    }

    public void StopHaptics()
    {
        if (gameManager.dominantHand == DominantHand.LEFT)
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        else
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }

    public void ChangeMaterial(Material material) { objectToChangeMaterialOn.GetComponent<Renderer>().material = material; }
}
