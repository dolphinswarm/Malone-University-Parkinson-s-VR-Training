using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandlerForSink : MonoBehaviour
{
    // ======================================================== Variables
    public Animator animator;

    // ======================================================== Methods
    // Start is called before the first frame update
    void Start()
    {
        // Get the animator component on this object
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
