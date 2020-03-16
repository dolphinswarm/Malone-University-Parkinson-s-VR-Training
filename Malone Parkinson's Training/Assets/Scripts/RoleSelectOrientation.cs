using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSelectOrientation : MonoBehaviour
{
    public bool arrayFilled;

    public GameObject Question4Answers;
    public GameObject myGameManager;

    public GameObject[] allAnswers; 

    void Start()
    {
        myGameManager = GameObject.Find("Game Manager");

    }

    void Update () {

        

        if (gameObject.GetComponent<QuestionEvent>().isActive)
        {
            if (!arrayFilled)
            {
              
                arrayFilled = true;
            }
           

          //  Debug.Log("notes is active");
            for (int i = 0; i < allAnswers.Length; i++)
            {
               // Debug.Log("is looping");
                if(allAnswers[i].GetComponent<AnswerManager>().selected)
                {
                    //Debug.Log("Sees one of the answers is selected");
                    // public enum JobRole { MonitorTech, RespTherapist, Nurse, None };
                    /*if (allAnswers[i].GetComponent<AnswerManager>().answerText == "Monitor Technician")
                    {
                    //    myGameManager.GetComponent<GameSettings>().jobRole = JobRole.MonitorTech;
                    }*/
                    //myGameManager.GetComponent<GameSettings>().jobRole = (JobRole)i;
                    //Debug.Log("if reading this then should have executed change of role in GameSettings by now");
                 
                }
            }

        }
    }

  


}
