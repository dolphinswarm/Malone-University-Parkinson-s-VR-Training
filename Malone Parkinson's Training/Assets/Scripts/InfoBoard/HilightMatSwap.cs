using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HilightMatSwap : MonoBehaviour {

    public Material baseMat;
    public Material toonMat;
    bool toonOn = false;

	// Use this for initialization
	public void Setup () {
        baseMat = GetComponent<Renderer>().material;
    }
	
	public void Hilight (bool onOff) {
        if (onOff) {
            GetComponent<Renderer>().material = toonMat;
            //toonMat.mainTexture = baseMat.mainTexture;
        }
        else {
            GetComponent<Renderer>().material = baseMat;
            //toonMat.mainTexture = null;
        }	
	}
}
