using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuestionEvent))]
public class OpenEndedQuestion : MonoBehaviour {
    // ======================================================== Variables
    public GameObject myGameManager;
    public QuestionEvent myQuestion;
    public bool forceSingleAnswer = true;

    // ======================================================== Methods
    void Start() {
        myQuestion = GetComponent<QuestionEvent>();

        // force single answer & open ended if desired
        if (forceSingleAnswer) {
            myQuestion.multipleAnswers = false;
            myQuestion.showAllOfTheAbove = false;
        }

        myQuestion.openEnded = true;

        Initialize();
    }

    protected virtual int GetAnswerIndex(List<AnswerManager> thingsSelected) {
        int selectedIndex = -1;
        string thingSelected = thingsSelected[0].answerText;
        Debug.Log("myQuestion: " + myQuestion.gameObject.name + " -- answer length: " + myQuestion.answers.Length.ToString());
        for (int i = 0; i < myQuestion.answers.Length; i++) {
            if (myQuestion.answers[i].answerText == thingsSelected[0].answerText) {
                selectedIndex = i;
            }
        }
        return selectedIndex;
    }

    public virtual void Initialize() {
        // OVERRIDE THIS TO DO SOMETHING AT STARTUP
    }

    public virtual void ProcessAnswers(List<AnswerManager> thingsSelected) {
        // OVERRIDE THIS TO DO SOMETHING WHEN ANSWER IS SUBMITTED
    }

}
