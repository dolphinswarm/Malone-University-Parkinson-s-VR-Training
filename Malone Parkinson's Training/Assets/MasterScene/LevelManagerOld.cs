using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class LevelInfo {

    public string sceneFile;
    //public Scene sceneFile;
    public bool loadHospital = false;
    public bool isLoaded = false;

    // stats
    public float startTime;
    public float endTime;
    public float score;

    // consider putting load / unload methods here instead of below?
}

//[System.Serializable]
//public class SimLevelInfo : LevelInfo {
//    public Sim simID;   // include this for simulations, but not logo scene / report card / etc.
//}



public class LevelManagerOld : MonoBehaviour {

    public GameObject myGameManager;

    //Levels / Simulations available
    public LevelInfo[] simulations;
    public LevelInfo tutorialLevel;
    public LevelInfo reportCardLevel;
    public LevelInfo splashScreenLevel;
    public LevelInfo creditsLevel;
    public LevelInfo masterLevel;

    // Sub-levels with geometry
    public LevelInfo hospitalGeometryLevel;

    public GameObject[] HideOnLevelLoad;


    // internal status variables 
    public LevelInfo curLevel;
    public bool needToUnloadScene = false; //made public so it can be reached during the return to masterscene after a completed sim
    //List<LevelInfo> levelsLoaded = new List<LevelInfo>();  // <-- is this redundant with .isLoaded property of each level?

    public bool CribIsActive;
    public bool BasketIsActive;
    public bool O2TankIsActive;

    // -------------------------------------------------------------------
    // Use this for initialization
    void Start () {
		
	}

    //LevelInfo FindLevel(Sim requestedSim) {
    //    LevelInfo newLevel = new LevelInfo();
    //    foreach (LevelInfo level in simulations) {
    //        if (level.simID == requestedSim) {
    //            newLevel = level;
    //        }
    //    }

    //    return newLevel;
    //}

    void LoadLevel(LevelInfo whichLevel) {
        SceneManager.LoadSceneAsync(whichLevel.sceneFile, LoadSceneMode.Additive);
        whichLevel.isLoaded = true;
        
        //levelsLoaded.Add(whichLevel);     // possible alt way to track what's loaded
        if (whichLevel != hospitalGeometryLevel) {
            curLevel = whichLevel;
            needToUnloadScene = true;
        }

        foreach (GameObject thing in HideOnLevelLoad) {
            thing.SetActive(false);
        }
    }

    void UnloadLevel(LevelInfo whichLevel) {
        Debug.Log(whichLevel.sceneFile);//It wants to unload endScene but seeing as the player is already back to master it can't find it
        SceneManager.UnloadSceneAsync(whichLevel.sceneFile);
        if (whichLevel != hospitalGeometryLevel)
        {
            curLevel = null;
           // curLevel.sceneFile = "";
            //curLevel.simID = 0;
           // curLevel.loadHospital = false;

            needToUnloadScene = false;

        }
        whichLevel.isLoaded = false;
        //levelsLoaded.Remove(whichLevel);
        
    }

    //public void StartSimulation(Sim whichSim) {
    //    LevelInfo newLevel = FindLevel(whichSim);

    //    // Unload current sim asynchronosly (in the background) ....if one is running
    //    if (needToUnloadScene) {
    //        //curLevel.sceneFile != ""
    //        Debug.Log("It is thinking there is something to get rid of");
    //        Debug.Log("Attempting to unload CurLevel: " + curLevel.sceneFile );
    //        UnloadLevel(curLevel);

    //        // Log current status to file?   ....note that we're switching?
    //        // reset score / time values, cur-event, etc.?
    //        // prompt to save?
    //    }

    //    // Load the new sim asynchronosly (in the background)
    //    LoadLevel(newLevel);

    //    // unload hospital if it's not needed....
    //    if (hospitalGeometryLevel.isLoaded && !newLevel.loadHospital) {
    //        UnloadLevel(hospitalGeometryLevel);
    //    }
    //    // ....or start loading the hospital if it wasn't loaded but we need it
    //    else if (!hospitalGeometryLevel.isLoaded && newLevel.loadHospital) {
    //        LoadLevel(hospitalGeometryLevel);
    //    }
    //    //else { } ....leave it as is (loaded and needed / unloaded and not needed)
    //}

    public void AllDone() { // why pass this in?  should always be curLevel
        // unload currently loaded level
        if(needToUnloadScene ) //curLevel.sceneFile != ""
        {
            UnloadLevel(curLevel);
            // unload hospital geometry scene too if necessary
            if (hospitalGeometryLevel.isLoaded)
            {
                UnloadLevel(hospitalGeometryLevel);
            }

            if(GameObject.Find("TimerObject").GetComponent<CompletionTimer>().timerOn == true)
            {

                GameObject.Find("TimerObject").GetComponent<CompletionTimer>().timerOn = false;
                GameObject.Find("TimerObject").GetComponent<CompletionTimer>().ResetTimer();

            }
        }
        else
        {

        }
        

    }

    public void ReturnMasterScene()
    {
        foreach (GameObject thing in HideOnLevelLoad)
        {
            thing.SetActive(true);
        }
    }

    public void LoadReportCard( ) {
        // Need to make sure reportCard Data is saved before level is unloaded

        // unload the scene
        AllDone();

        // load report card
        LoadLevel(reportCardLevel);
    }

    // force load master scene... use this when pressing the run button from Unity editor
    //  Have a node search for, e.g., the game manager or FPC at Start, and call this if search fails
    public void LoadMainScene() {
        SceneManager.LoadScene(masterLevel.sceneFile);
    }
}
