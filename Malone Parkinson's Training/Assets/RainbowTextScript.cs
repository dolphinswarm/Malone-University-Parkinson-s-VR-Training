using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainbowTextScript : MonoBehaviour
{
    // ======================================================== Variables
    public Text textToChangeColor;
    private float r = 0.5f;
    private float g = 0.5f;
    private float b = 0.5f;

    // ======================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // Find the text, if not present
        if (textToChangeColor == null)
        {
            textToChangeColor = gameObject.GetComponent<Text>();
        }
    }

    /// <summary>
    /// On frame update...
    /// </summary>
    void Update()
    {
        // Create a new color
        //r += Random.Range(-0.05f, 0.05f);
        //g += Random.Range(-0.05f, 0.05f);
        //b += Random.Range(-0.05f, 0.05f);
        r = Mathf.Sin(Time.time);
        b = Mathf.Cos(Time.time);
        g = Mathf.Sin(Time.time) * 2;

        Color color = new Color(r, g, b);

        // Change the color
        textToChangeColor.color = color;
    }
}
