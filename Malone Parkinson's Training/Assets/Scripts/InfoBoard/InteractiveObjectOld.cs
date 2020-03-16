using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interactive object.
/// </summary>
public class InteractiveObjectOld : Interactive {
    // ======================================================== Variables
    public Transform visibleObjectParentNode;   // ???
    public Material highlightShader;            // The highlight shader.
    public InfoBoardEvent owningEvent;          // The object this script is calling back to.
    //public bool grabbale;                       // Is this object grabbable?

    // ======================================================== Methods

    protected void AddOutlinerToChildren(Transform myObject) {    
        foreach (Transform child in myObject) {
            if (child.gameObject.GetComponent<HilightMatSwap>() == null && child.gameObject.GetComponent<Renderer>() != null) {
                HilightMatSwap myToonShader = child.gameObject.AddComponent<HilightMatSwap>();
                myToonShader.toonMat = highlightShader;
                myToonShader.Setup();
            }
            /*if (child.gameObject.GetComponent<Outline>() == null && child.gameObject.GetComponent<Renderer>() !=null) {
                Outline myOutline = child.gameObject.AddComponent<Outline>();
                myOutline.color = hilightColorIndex;
                myOutline.enabled = false;
            } //*/
            
            AddOutlinerToChildren(child);
        }
    }

    protected void ToggleOutlinerOnChildren(Transform myObject, bool onOff) {
        foreach (Transform child in myObject) {
            if (child.gameObject.GetComponent<HilightMatSwap>() != null) {
                child.gameObject.GetComponent<HilightMatSwap>().Hilight(onOff);
            }

            /*if (child.gameObject.GetComponent<Outline>() != null) {
                child.gameObject.GetComponent<Outline>().enabled = onOff;
            }//*/
            ToggleOutlinerOnChildren(child, onOff);
        }
    }

    // Use this for initialization
    protected override void Initialize() {
        AddOutlinerToChildren(visibleObjectParentNode);
    }

    protected override void Highlight() {
        ToggleOutlinerOnChildren(visibleObjectParentNode, true);
    }

    public override void Dim() {
        ToggleOutlinerOnChildren(visibleObjectParentNode, false);
    }

    protected override void Select() {
        Debug.Log("I've been selected!");
        if (owningEvent != null) {
            Debug.Log("Calling out to: " + owningEvent.name);
            owningEvent.Clicked();
        }
    }
}
