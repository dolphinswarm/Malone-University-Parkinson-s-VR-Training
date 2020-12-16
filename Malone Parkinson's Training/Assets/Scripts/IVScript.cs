using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IVScript : EndActivate
{
    // ======================================================== Variables
    [Header("Main Items")]
    public GameManager gameManager;
    public AudioSource audioSource;

    [Header("IV Screen")]
    public InteractiveObject iVOnButton;
    public InteractiveObject iVUpButton;
    public InteractiveObject iVDownButton;
    public TMP_Text iVLabel;
    private int ivValue = 50;

    [Header("NGT Screen")]
    public InteractiveObject nGTOnButton;
    public InteractiveObject nGTUpButton;
    public InteractiveObject nGTDownButton;
    public TMP_Text nGTLabel;
    private int ngtValue = 100;


    // ======================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>(); // Should only be one GameManager
    }

    /// <summary>
    /// Activates an event callback based on the name of the object calling.
    /// </summary>
    /// <param name="name"></param>
    public override void Activate(string name)
    {
        // If name is IV On Button
        if (name == "IV On Button") {
            // If IV is on, turn it off
            if (ivValue > 0)
            {
                iVLabel.text = "off";
                ivValue = 0;
            }
            else
            {
                ivValue = 100;
                iVLabel.text = ivValue + " ml/hr";
            }
        }
        // If name is NGT On Button
        else if (name == "NGT On Button") {
            // If IV is on, turn it off
            if (ngtValue > 0)
            {
                nGTLabel.text = "off";
                ngtValue = 0;
                audioSource.Stop();
            }
            else
            {
                ngtValue = 100;
                nGTLabel.text = ngtValue + " ml/hr";
                audioSource.Play();
            }
        }
        // If name is NGT Up Button
        else if (name == "NGT Up Button") {
            ngtValue += 25;
            nGTLabel.text = ngtValue + " ml/hr";
        }
         // If name is NGT Down Button
        else if (name == "NGT Down Button") {
            ngtValue -= 25;
            nGTLabel.text = ngtValue + " ml/hr";
        }
        // If name is IV Up Button
        else if (name == "IV Up Button") {
            ivValue += 25;
            iVLabel.text = ivValue + " ml/hr";
        }
         // If name is IV Down Button
        else if (name == "IV Down Button") {
            ivValue -= 25;
            iVLabel.text = ivValue + " ml/hr";
        }

    }

    /// <summary>
    /// Sets the value of the IV to a given number.
    /// </summary>
    /// <param name="num"></param>
    public void SetIVValue(int num)
    {
        if (num == 0)
        {
            iVLabel.text = "OFF";
        }
        else
        {
            ivValue = num;
            iVLabel.text = ivValue + " ml/hr";
        }
    }
    
    /// <summary>
    /// Sets the value of the NGT to a given number.
    /// </summary>
    /// <param name="num"></param>
    public void SetNGTValue(int num)
    {
        if (num == 0)
        {
            nGTLabel.text = "OFF";
        }
        else
        {
            ngtValue = num;
            nGTLabel.text = ngtValue + " ml/hr";
        }
    }


}
