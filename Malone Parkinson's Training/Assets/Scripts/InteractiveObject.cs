﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enumeration to determine the type of interactive object.
/// </summary>
public enum ObjectType { PICKUP, TOUCH };

/// <summary>
/// An enumeration to determine the type of interactive object.
/// </summary>
public enum HighlightType { BODY_DISTANCE, HAND_DISTANCE, LOOKAT, POINTAT, ALWAYS, NEVER };

/// <summary>
/// The class for a generic interactive object.
/// </summary>
public class InteractiveObject : Interactive
{
    // ======================================================== Variables
    [Header("Interactive Object Properties")]
    public InfoBoardEvent owningEvent;                      // The object this script is calling back to.
    public ObjectType objectType;                           // The type of interactive object.
    public Animator animator;                               // The animator for playing animations.


    [Header("Object Hightlight")]
    public Dictionary<int, Material[]> objectMaterials;     // A dictionary of this object's materials.
    public Material highlightMaterial;                      // The highlight material of this object.
    public HighlightType highlightType;                     // How should this object be highlit?
    [Range(0.0f, 25.0f)]
    public float highlightDistance = 10.0f;                 // The highlight distance.
    private bool hideBeforeEvent = false;                   // Should this object be hidden before its event?


    [Header("Animation Properties")]
    public string animationName;                             // The name of the animation to play upon triggering.


    private Transform cameraPos;                            // The position of the camera

    // ======================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    protected override void Start()
    {
        // Call the base start
        base.Start();
    }

    /// <summary>
    /// Initialize this pickup object.
    /// </summary>
    protected override void Initialize()
    {
        // Set owning event, if null
        if (owningEvent == null)
            owningEvent = GetComponentInParent<InfoBoardEvent>();

        // If info board is not set, then set it
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>();

        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>(); // Should only be one GameManager

        // Get all materials of this object and its children
        objectMaterials = new Dictionary<int, Material[]>();
        foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            objectMaterials.Add(renderer.GetHashCode(), renderer.materials);
        }

        // If highlight shader is not set, get the game manager's
        if (highlightMaterial == null)
            highlightMaterial = gameManager.hightlightMaterial;

        // Set the proper looking tag
        if (gameManager.controlType == ControlType.OCULUS)
        {
            requiredTag = "OVR Grabber";
        }
        else
        {
            requiredTag = "Reticle";
        }

        // Lastly, set this to inactive after intialization
        enabled = false;
        isCurrentlyInteractable = false;
        if (hideBeforeEvent) gameObject.SetActive(false);
    }

    /// <summary>
    /// On frame update...
    /// </summary>
    protected void Update()
    {
        // If currently interactive...
        if (isCurrentlyInteractable)
        {
            // Get the camera and hand vectors
            Vector3 cameraVec = transform.position - cameraPos.position;
            Vector3 domHandVec = transform.position - gameManager.currentHand.transform.position;
            Vector3 offHandVec = transform.position - gameManager.offHand.transform.position;

            // If distance, check it via the distance
            if (highlightType == HighlightType.BODY_DISTANCE && cameraVec.magnitude < highlightDistance)
            {
                Highlight();
            }
            // If distance, check it via the distance
            else if (highlightType == HighlightType.HAND_DISTANCE && (domHandVec.magnitude < highlightDistance || 
                offHandVec.magnitude < highlightDistance))
            {
                Highlight();
            }
            // Else, if look at, check it via the dot procduct
            else if (highlightType == HighlightType.LOOKAT && Vector3.Dot(cameraVec.normalized, cameraPos.forward) > 0.95f)
            {
                Highlight();
            }
            // Else, if point at, check it via the dot procduct
            else if (highlightType == HighlightType.POINTAT && (isMouseOver || 
                Vector3.Dot(domHandVec.normalized, gameManager.currentHand.transform.forward) > 0.95f ||
                Vector3.Dot(offHandVec.normalized, gameManager.offHand.transform.forward) > 0.95f))
            {
                Highlight();
            }
            // Else, if always, highlight
            else if (highlightType == HighlightType.ALWAYS)
            {
                Highlight();
            }
            // Else, if never, dim
            else if (highlightType == HighlightType.NEVER)
            {
                Dim();
            }
            // Else, dim
            else
            {
                Dim();
            }
        }
    }

    /// <summary>
    /// Highlight the interactive object with a shader.
    /// </summary>
    protected override void Highlight()
    {
        // Check all the renderers attached to this object and its children
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            // Make a temporary array, modify it, then set the old array to this.
            // This is a bit roundabout, but it's the best way I could find to do it ¯\_(ツ)_/¯ 
            Material[] tmp = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                tmp[i] = highlightMaterial;
            }
            renderer.materials = tmp;
        }
    }

    /// <summary>
    /// Return to showing the default material.
    /// </summary>
    public override void Dim()
    {
        // Check all the renderers attached to this object and its children
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.materials = objectMaterials[renderer.GetHashCode()];
        }
    }

    /// <summary>
    /// On selection, send a "click" event, turn off if this is currently interactable, and play animation.
    /// </summary>
    protected override void Select()
    {
        // If we have an owning event AND we can currently interact with this object...
        if (owningEvent != null && isCurrentlyInteractable)
        {
            // If we have an animator and an animation name, pass them to the owning event
            if (animator != null && animationName != "")
                owningEvent.SetAnimation(animator, animationName);

            // Turn off interactivity to this object
            isCurrentlyInteractable = false;

            // Toggle "clicked" in the owning event
            owningEvent.Clicked();

            // If we have an animator, play the animation
            if (animator != null)
                animator.SetBool("HasBeenTriggered", true);
        }     
    }

    /// <summary>
    /// A method for returning this game object.
    /// </summary>
    /// <returns>This script's owning game object.</returns>
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    // ---------- MOUSE INTERACTION ---------- (OVERRIDE THE PARENT CLASS SO EVERY MOUSEOVER DOESN'T HIGHLIGHT)
    /// <summary>
    /// On mouse enter...
    /// </summary>
    protected override void OnMouseEnter()
    {
        // If not highlighted, highlight
        if (!isHighlighted && gameManager.controlType == ControlType.MOUSE_KEYBOARD && isCurrentlyInteractable)
        {
            isMouseOver = true;
        }
    }

    /// <summary>
    /// On mouse exit...
    /// </summary>
    protected override void OnMouseExit()
    {
        // If not highlighted, dim
        if (!isHighlighted && gameManager.controlType == ControlType.MOUSE_KEYBOARD && isCurrentlyInteractable)
        {
            isMouseOver = false;
        }
    }

    /// <summary>
    /// Toggle whether this object should be hidden before its event.
    /// </summary>
    /// <param name="hide">Should the gameobject be hidden?</param>
    public void SetHideBeforeEvent(bool hide)
    {
        hideBeforeEvent = hide;
    }

    /// <summary>
    /// Sets the camera's transform.
    /// </summary>
    /// <param name="transform">The transform of the main camera.</param>
    public void SetCameraPos(Transform transform)
    {
        cameraPos = transform;
    }
}