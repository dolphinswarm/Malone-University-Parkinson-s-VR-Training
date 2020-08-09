using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitAnswer : InteractiveText {
    // ======================================================== Variables

    // ======================================================== Methods
    /// <summary>
    /// Sumbits answers for checking.
    /// </summary>
    protected override void Select() {
        // submit the answer
        myQuestion.CheckAnswers();
    }

}
