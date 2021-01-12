using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;
using OVR;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    // ======================================================== Variables
    [Header("Controllers")]
    public GameObject ovrController;                           // The game's OVR controller
    public GameObject genericTitleSceenCamera;                  // The title screen camera object.
    public OVRHapticsManager leftHandOVRHaptics;
    public OVRHapticsManager rightHandOVRHaptics;
    public OVRScreenFade screenFade;
    public DominantHand dominantHand = DominantHand.RIGHT;      // The dominant hand of the player
    private GameObject leftReticle;                             // The left hand reticle
    private GameObject rightReticle;                            // The right hand reticle
    public GameObject currentReticle;                           // The active reticle

    [Header("Music")]
    public AudioSource music;

    // ======================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // If an Oculus headset is present, switch the main camera to that
        if (OVRManager.isHmdPresent)
        {
            // Set the controller
            ovrController.SetActive(true);
            genericTitleSceenCamera.SetActive(false);
            screenFade = ovrController.GetComponentInChildren<OVRScreenFade>();

            // Set the reticles
            leftReticle = GameObject.Find("Pointer-Left");
            rightReticle = GameObject.Find("Pointer-Right");
            leftReticle.SetActive(false);
        }
        else
        {
            // Set the camera
            ovrController.SetActive(false);
            genericTitleSceenCamera.SetActive(true);
            screenFade = genericTitleSceenCamera.GetComponentInChildren<OVRScreenFade>();
        }

        // Start the music
        if (music == null)
            music = GetComponent<AudioSource>();

        music.loop = true;
        music.Play();
    }

    /// <summary>
    /// On frame update..
    /// </summary>
    void Update()
    {
        // Check if we should swap the dominant hand
        if (OVRManager.isHmdPresent)
        {
            // Left hand
            //if (OVRInput.GetDown(OVRInput.RawButton.B))
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && dominantHand == DominantHand.LEFT)
            {
                // Set the dominant hand
                dominantHand = DominantHand.RIGHT;

                // Mess with the reticles
                if (leftReticle.activeSelf)
                {
                    rightReticle.SetActive(true);
                    leftReticle.SetActive(false);
                }
                currentReticle = rightReticle;
            }

            // Right hand
            //else if (OVRInput.GetDown(OVRInput.RawButton.Y))
            else if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) && dominantHand == DominantHand.RIGHT)
            {
                // Set the dominant hand
                dominantHand = DominantHand.LEFT;

                // Mess with the reticles
                if (rightReticle.activeSelf)
                {
                    rightReticle.SetActive(false);
                    leftReticle.SetActive(true);
                }
                currentReticle = leftReticle;
            }
        }
    }

    /// <summary>
    /// Starts the simulation by loading the main sim.
    /// </summary>
    public void StartSimulation()
    {
        // Fade the screen
        screenFade.FadeOut();

        // Load the main game scene
        StartCoroutine(WaitToLoadScene());
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitSimulation()
    {
        // Quits the game
        Application.Quit();
    }

    /// <summary>
    /// Coroutine for waiting 3 seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitToLoadScene()
    {
        // Wait 3 seconds
        yield return new WaitForSeconds(3);

        // Load the main game scene
        SceneManager.LoadScene("Main Scene");
    }

    /// <summary>
    /// Stops the music.
    /// </summary>
    public void StopMusic()
    {
        music.Stop();
    }
}
