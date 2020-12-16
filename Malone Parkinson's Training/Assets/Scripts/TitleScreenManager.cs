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
            ovrController.SetActive(true);
            genericTitleSceenCamera.SetActive(false);
            screenFade = ovrController.GetComponentInChildren<OVRScreenFade>();
        }
        else
        {
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
