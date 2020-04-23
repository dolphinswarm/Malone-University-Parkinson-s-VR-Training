using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OVR;
using UnityEngine.EventSystems;

/// <summary>
/// The base class for interaction.
/// </summary>
public class Interactive : MonoBehaviour {
    // ======================================================== Variables
    [Header("Info Board Interface")]
    public InfoBoardUI infoBoard;
    protected Color myColor;

    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("Interactive Properties")]
    public bool isHighlighted = false;
    public bool matchTag = false;
    public string requiredTag = "Reticle";
    public bool isMouseOver = false;
    public bool isCurrentlyInteractable = true;

    // ======================================================== Methods
    /// <summary>
    /// Initialize this interactive item.
    /// </summary>
    protected virtual void Start() {
        // If renderer is not null, set my color to the renderer's color
        if (GetComponent<Renderer>() != null) {
            myColor = GetComponent<Renderer>().material.color;
        }

        // Use the base class initialize
        Initialize();
    }

    /// <summary>
    /// Sets up the script.
    /// </summary>
    protected virtual void Initialize()
    {
        // If info board is not set, then set it
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>(); // Should only be one InfoBoardUI (hopefully)

        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>(); // Should only be one GameManager
    }

    /// <summary>
    ///  Highlights this interactive item.
    /// </summary>
    protected virtual void Highlight() {
        if (GetComponent<Renderer>() != null) {
            GetComponent<Renderer>().material.color = infoBoard.selectedColor;
        }
    }

    /// <summary>
    /// Dims this interactive item.
    /// </summary>
    public virtual void Dim() {
        if (GetComponent<Renderer>() != null) {
            GetComponent<Renderer>().material.color = myColor;
        }
    }

    /// <summary>
    /// Overridden in children classes to do something when selected.
    /// </summary>
    protected virtual void Select() { } // OVERRIDE THIS METHOD TO DO SOMETHING WHEN SELECTED

    // ---------- MOUSE INTERACTION ----------
    /// <summary>
    /// On mouse enter...
    /// </summary>
    protected virtual void OnMouseEnter() 
    { 
        // If not highlighted, highlight
        if (!isHighlighted && gameManager.controlType == ControlType.MOUSE_KEYBOARD && isCurrentlyInteractable)
        {
            Highlight(); 
            isHighlighted = true;
            isMouseOver = true;
        } 
    }

    /// <summary>
    /// On mouse exit...
    /// </summary>
    protected virtual void OnMouseExit()
    {
        // If not highlighted, dim
        if (isHighlighted && gameManager.controlType == ControlType.MOUSE_KEYBOARD && isCurrentlyInteractable)
        {
            Dim();
            isHighlighted = false;
            isMouseOver = false;
        }
    }

    /// <summary>
    ///  On mouse down...
    /// </summary>
    protected void OnMouseDown() { Select(); }

    // ---------- 3D RETICLE INTERACTION W/ COLLIDER ----------
    /// <summary>
    /// On collision enter...
    /// </summary>
    /// <param name="other">The other collision.</param>
    protected void OnCollisionEnter(Collision other) 
    {
        // If we should match tag and the other game object matches that tag, OR
        // If we don't check for matching tags...
        if (((matchTag && other.gameObject.CompareTag(requiredTag)) || !matchTag) && !isHighlighted && isCurrentlyInteractable)
        {
            Highlight();
            isHighlighted = true;
        }
    }

    /// <summary>
    /// On collision exit...
    /// </summary>
    /// <param name="other">The other collision.</param>
    protected void OnCollisionExit(Collision other)
    {
        // If currently highlighted, dim
        if (isHighlighted && isCurrentlyInteractable)
        {
            Dim();
            isHighlighted = false;
        }
    }

    /// <summary>
    /// On collision stay....
    /// </summary>
    /// <param name="other">The other collision.</param>
    protected virtual void OnCollisionStay(Collision other)
    {
        // If we should match tag and the other game object matches that tag, OR
        // If we don't check for matching tags...
        if ((matchTag && other.gameObject.CompareTag(requiredTag) || !matchTag) && isCurrentlyInteractable)
        {
            // If we haven't been clicked and we receive the appropriate input button, toggle
            if (Input.GetButtonDown("Fire1") || GetCorrectOVRInput()) Select();
        }
    }


    // ---------- 3D RETICLE INTERACTION W/ TRIGGER REGION ----------
    /// <summary>
    /// On trigger enter...
    /// </summary>
    /// <param name="other">The other collider.</param>
    protected virtual void OnTriggerEnter(Collider other) 
    {
        // If we should match tag and the other game object matches that tag, OR
        // If we don't check for matching tags...
        if (((matchTag && other.gameObject.CompareTag(requiredTag)) || !matchTag) && !isHighlighted && isCurrentlyInteractable)
        {
            Highlight();
            isHighlighted = true;
        }
    }

    /// <summary>
    /// On trigger exit...
    /// </summary>
    /// <param name="other">The other collider.</param>
    protected virtual void OnTriggerExit(Collider other)
    {
        // If currently highlighted, dim
        if (isHighlighted && isCurrentlyInteractable)
        {
            Dim();
            isHighlighted = false;
        }
    }

    /// <summary>
    /// On trigger stay....
    /// </summary>
    /// <param name="other">The other collider.</param>
    protected virtual void OnTriggerStay(Collider other) 
    {
        // If we should match tag and the other game object matches that tag, OR
        // If we don't check for matching tags...
        if ((matchTag && other.gameObject.CompareTag(requiredTag) || !matchTag) && isCurrentlyInteractable)
        {
            // If we haven't been clicked and we receive the appropriate input button, toggle
            if (Input.GetButtonDown("Fire1") || GetCorrectOVRInput()) Select();
        }
    }

    /// <summary>
    /// Gets the correct OVR input from the user, based off the dominant hand.
    /// </summary>
    /// <returns></returns>
    private bool GetCorrectOVRInput()
    {
        if (gameManager.dominantHand == DominantHand.RIGHT)
        {
            return OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
        }
        else
        {
            return OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger);
        }
    }
}