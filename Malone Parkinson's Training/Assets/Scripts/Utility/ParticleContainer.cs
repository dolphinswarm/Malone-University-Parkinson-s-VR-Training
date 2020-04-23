using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple script for containing a particle system. Used by animations.
/// </summary>
public class ParticleContainer : MonoBehaviour
{
    // ======================================================== Variables
    public ParticleSystem particleSystemForAnimation;

    // ======================================================== Methods
    public void PlayParticleSystem() { particleSystemForAnimation.Play(); }

    public void StopParticleSystem() { particleSystemForAnimation.Stop(); }
}
