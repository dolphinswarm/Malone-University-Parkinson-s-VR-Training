using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForReportCardFolder : MonoBehaviour {

    public string reportCardPathName = "ReportCards";

    // Use this for initialization
    void Start () {
        if (!System.IO.Directory.Exists(reportCardPathName)) { 
            System.IO.DirectoryInfo myDir = System.IO.Directory.CreateDirectory(reportCardPathName);
            Debug.Log("Creating Report Card Directory: " + myDir.ToString());
        } else
        {
            Debug.Log("Report Card Directory Detected");
        }
    }
	

}
