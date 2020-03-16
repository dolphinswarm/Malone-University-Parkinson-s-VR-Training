using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class changingLevels : MonoBehaviour {

	    
	//************ Scene Flags *********************
	public bool tutorialIsOpen = false;

	public bool pauseUp = false;

	public bool aVRSIsOpen = false;

	public bool logoIsUp = true;

	public bool endOpen = false;       
									   
	

	//*********** Control when restart pops up in menu *********
	public Text Restart;


	public GameObject roleCanvas;  //only leaving in here because idk if I want to delete the game object

	//Pick scenario splash screen
	public GameObject VRS_Splash;
	public bool change_VRS;

	//pick Role splash screen
	public GameObject Role_Splash;
	public bool change_Role;

	

	//********* in charge of role and sim change menus ********
	public GameObject role_change_cubes;
	public GameObject role_init_cubes;  //we dont use these anymore. i'll keep them on standby though
	public GameObject sim_change_cubes;

	//player hasnt picked a role yet
	public GameObject role_alert;



	//************ deals with how we load and unload? ***********
	string currentsceneloaded = "nothing for now";


	public GameObject myGameManager;

	//Can a "GameManager" exist within the same script of code?

	//Does what we're tracking have to be outside this script somewhere?

	public int simulation = 0;
	public int role = 0;

	public bool rolemenu; //again might not need 

	public GameObject player; // will need to be changed to hand or hands ******* which is why we may need the game manager?



	public GameObject youSure;
	public bool youSureUp;


	public GameObject skipTut;
	public bool skipTutYup;






	//*********************************************************************************************************
	public bool initialTut;



	public bool exitOut;

	void Start() {
		aVRSIsOpen = false;
		VRS_Splash.SetActive(false);
		Role_Splash.SetActive(false);

		StartCoroutine(logoRun());

		roleCanvas.gameObject.SetActive(false);


		role_change_cubes.SetActive(false);
		role_init_cubes.SetActive(false);
		sim_change_cubes.SetActive(false);

		role_alert.SetActive(false);

		youSure.SetActive(false);

		skipTut.SetActive(false);

}


	void OnMouseOver() {
		print(gameObject.name);
	}

	


	void Update() {

		Ray ray = new Ray(player.transform.position, player.transform.forward);  // hand(s)
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 100f)) {
			if(Input.GetMouseButtonDown(0)) {
				//****************** selecting a role ********************
				if(hit.collider.tag == "role1" || hit.collider.tag == "role2" || hit.collider.tag == "role3" || hit.collider.tag == "role4") {

					if(role_alert == true) {
						role_alert.SetActive(false);
					}

					if(hit.collider.tag == "role1") {
						if(rolemenu || change_Role) {
							role = 1;
							print("role = " + role);

							youSure.SetActive(true);
							youSureUp = true;

						}
					}
					if(hit.collider.tag == "role2") {
						if(rolemenu || change_Role) {
							role = 2;
							print("role = " + role);

							youSure.SetActive(true);
							youSureUp = true;

						}
					}
					if(hit.collider.tag == "role3") {
						if(rolemenu || change_Role) {
							role = 3;
							print("role = " + role);
							youSure.SetActive(true);
							youSureUp = true;

						}
					}
					if(hit.collider.tag == "role4") {
						if(rolemenu || change_Role) {
							role = 4;
							print("role = " + role);
							youSure.SetActive(true);
							youSureUp = true;

						}
					}
					
					

				}

				//********************** Getting into Tutorial *****************************

					if(pauseUp) {

						if(hit.collider.tag == "tutorial") {


						
							if(skipTutYup) {
							
								print("Consider this tutorial skipped");
								skipTut.SetActive(false);
							}

								if(currentsceneloaded == "_scene_VRS1" || currentsceneloaded == "_scene_VRS2" || currentsceneloaded == "_scene_VRS3" || currentsceneloaded == "_scene_VRS4") {

									if(currentsceneloaded == "_scene_VRS1") {
										print("for the love of all that's good, work!");
										SceneManager.UnloadSceneAsync("_scene_VRS1");
									}
									if(currentsceneloaded == "_scene_VRS2") {
										SceneManager.UnloadSceneAsync("_scene_VRS2");
									}
									if(currentsceneloaded == "_scene_VRS3") {
										SceneManager.UnloadSceneAsync("_scene_VRS3");
									}
									if(currentsceneloaded == "_scene_VRS4") {
										SceneManager.UnloadSceneAsync("_scene_VRS4");
									}


								}

								
									SceneManager.LoadSceneAsync("_scene_Tutorial", LoadSceneMode.Additive);
									tutorialIsOpen = true;
									aVRSIsOpen = false;
								
							
						}
					}
				
				if(hit.collider.tag == "tutorial" && pauseUp) {
					SceneManager.UnloadSceneAsync("_scene_Pause_Menu");
					pauseUp = false;

				}
				//*********************** The VRS's ********************
				if(hit.collider.tag == "VRS1" && !aVRSIsOpen && !logoIsUp && !endOpen && !rolemenu && !pauseUp) {
				

					youSure.SetActive(true);
					youSureUp = true;
					simulation = 1;

				
					currentsceneloaded = "_scene_VRS1";

					print("simulation = " + simulation + "role = " + role);
				}
				if(hit.collider.tag == "VRS2" && !aVRSIsOpen && !logoIsUp && !endOpen && !rolemenu && !pauseUp) {
					SceneManager.LoadSceneAsync("_scene_VRS2", LoadSceneMode.Additive);

					simulation = 2;

					
					currentsceneloaded = "_scene_VRS2";

					print("simulation = " + simulation + "role = " + role);
				}
				if(hit.collider.tag == "VRS3" && !aVRSIsOpen && !logoIsUp && !endOpen && !rolemenu && !pauseUp) {
					SceneManager.LoadSceneAsync("_scene_VRS3", LoadSceneMode.Additive);

					simulation = 3;

				
					currentsceneloaded = "_scene_VRS3";

					print("simulation = " + simulation + "role = " + role);
				}
				if(hit.collider.tag == "VRS4" && !aVRSIsOpen && !logoIsUp && !endOpen && !rolemenu && !pauseUp) {
					SceneManager.LoadSceneAsync("_scene_VRS4", LoadSceneMode.Additive);

					simulation = 4;

			
					currentsceneloaded = "_scene_VRS4";

					print("simulation = " + simulation + "role = " + role);
				}
				if(hit.collider.tag == "VRS1" || hit.collider.tag == "VRS2" || hit.collider.tag == "VRS3" || hit.collider.tag == "VRS4") {
					if(!pauseUp) {
						
					}
					if(tutorialIsOpen) {
						SceneManager.UnloadSceneAsync("_scene_Tutorial");
						tutorialIsOpen = false;
						aVRSIsOpen = true;
					}
				}

				//**************** Get to End Scene ***********************
				if(hit.collider.tag == "end" && aVRSIsOpen && !pauseUp) {
					//!menuUp && !tutorialIsOpen

			
					SceneManager.LoadSceneAsync("_scene_End", LoadSceneMode.Additive);
					endOpen = true;

				}
					if(hit.collider.tag == "end" && !pauseUp) {

						if(currentsceneloaded == "_scene_VRS1" || currentsceneloaded == "_scene_VRS2" || currentsceneloaded == "_scene_VRS3" || currentsceneloaded == "_scene_VRS4") {

							if(currentsceneloaded == "_scene_VRS1") {
								print("for the love of all that's good, work!");
								SceneManager.UnloadSceneAsync("_scene_VRS1");
							}
							if(currentsceneloaded == "_scene_VRS2") {
								SceneManager.UnloadSceneAsync("_scene_VRS2");
							}
							if(currentsceneloaded == "_scene_VRS3") {
								SceneManager.UnloadSceneAsync("_scene_VRS3");
							}
							if(currentsceneloaded == "_scene_VRS4") {
								SceneManager.UnloadSceneAsync("_scene_VRS4");
							}


							currentsceneloaded = "";
							simulation = 0;
							aVRSIsOpen = false;
						}
					}

			
			//************** Getting to Menu *********************

				//in tutorial
				if(hit.collider.tag == "menu" && tutorialIsOpen && pauseUp) {
					SceneManager.UnloadSceneAsync("_scene_Tutorial");
				
					tutorialIsOpen = false;
				

				}
				//in VRSims
					if(hit.collider.tag == "menu" && pauseUp && aVRSIsOpen) {

						youSure.SetActive(true);
						youSureUp = true;

						exitOut = true;

					
					}

				//in end scene
				if(hit.collider.tag == "menu" && endOpen) {
					SceneManager.LoadSceneAsync("_scene_Pause_Menu", LoadSceneMode.Additive);
					pauseUp = true;
				}
				if(hit.collider.tag == "menu" && endOpen) {
					SceneManager.UnloadSceneAsync("_scene_End");  
					endOpen = false;

				}


				//************** Restart Sim *****************
				
					if(hit.collider.tag == "restart" && pauseUp && aVRSIsOpen) {

						if(skipTutYup) {
							skipTut.SetActive(false);
						}

						if(currentsceneloaded == "_scene_VRS1" || currentsceneloaded == "_scene_VRS2" || currentsceneloaded == "_scene_VRS3" || currentsceneloaded == "_scene_VRS4") {

							if(currentsceneloaded == "_scene_VRS1") {
								SceneManager.UnloadSceneAsync("_scene_VRS1");
								SceneManager.LoadSceneAsync("_scene_VRS1", LoadSceneMode.Additive);
							}
							if(currentsceneloaded == "_scene_VRS2") {
								SceneManager.UnloadSceneAsync("_scene_VRS2");
								SceneManager.LoadSceneAsync("_scene_VRS2", LoadSceneMode.Additive);
							}
							if(currentsceneloaded == "_scene_VRS3") {
								SceneManager.UnloadSceneAsync("_scene_VRS3");
								SceneManager.LoadSceneAsync("_scene_VRS3", LoadSceneMode.Additive);
							}
							if(currentsceneloaded == "_scene_VRS4") {
								SceneManager.UnloadSceneAsync("_scene_VRS4");
								SceneManager.LoadSceneAsync("_scene_VRS4", LoadSceneMode.Additive);
							}


							pauseUp = false;

							SceneManager.UnloadSceneAsync("_scene_Pause_Menu");
							print("unload and reload successful");
						}

						if(change_VRS) {
							change_VRS = false;
							VRS_Splash.SetActive(false);
							sim_change_cubes.SetActive(false);
						}
						if(change_Role) {
							change_Role = false;
							Role_Splash.SetActive(false);
							role_change_cubes.SetActive(false);
						}

					}
				
				//*************** Unpause / Continue *********************
				if(hit.collider.tag == "continue" && pauseUp) {

					if(skipTutYup) {
						skipTut.SetActive(false);
					}

					if(aVRSIsOpen) {
						if(pauseUp && !change_VRS && !change_Role) {
							SceneManager.UnloadSceneAsync("_scene_Pause_Menu");
							pauseUp = false;
						}
					}
					if(change_VRS) {
							change_VRS = false;
							VRS_Splash.SetActive(false);
							sim_change_cubes.SetActive(false);
						}
						if(change_Role) {
							change_Role = false;
							Role_Splash.SetActive(false);
							role_change_cubes.SetActive(false);
						}

				

				}
				//************************* Change Sims ****************************
				if(hit.collider.tag == "VRSChange" && pauseUp && !change_Role) {
					
					if(!change_VRS && role >= 1 && role <= 4) {
						VRS_Splash.SetActive(true);
						change_VRS = true;
						sim_change_cubes.SetActive(true);
					}
					if(role == 0) {
						role_alert.SetActive(true);
					}

				}
				//**************************** Change Role **************************
				if(hit.collider.tag == "roleChange" && pauseUp && !change_VRS) {
				
					Role_Splash.SetActive(true);
					change_Role = true;
					role_change_cubes.SetActive(true);
				}




				//*************************** Change Sims VRS selection ****************************
				if(pauseUp && change_VRS) {
						
						if(hit.collider.tag == "VRS1" || hit.collider.tag == "VRS2" || hit.collider.tag == "VRS3" || hit.collider.tag == "VRS4") {

							if(role >= 1 && role <= 4) {

								if(currentsceneloaded == "_scene_VRS1" || currentsceneloaded == "_scene_VRS2" || currentsceneloaded == "_scene_VRS3" || currentsceneloaded == "_scene_VRS4") {

									if(currentsceneloaded == "_scene_VRS1") {
										print("for the love of all that's good, work!");
										SceneManager.UnloadSceneAsync("_scene_VRS1");
									}
									if(currentsceneloaded == "_scene_VRS2") {
										SceneManager.UnloadSceneAsync("_scene_VRS2");
									}
									if(currentsceneloaded == "_scene_VRS3") {
										SceneManager.UnloadSceneAsync("_scene_VRS3");
									}
									if(currentsceneloaded == "_scene_VRS4") {
										SceneManager.UnloadSceneAsync("_scene_VRS4");
									}

								}

								print("it has made it through this entire loop");

							}
						}
					}
				

				if(role >= 1 && role <= 4) {

					if(hit.collider.tag == "VRS1" && change_VRS) {
					

						youSure.SetActive(true);
						youSureUp = true;

						simulation = 1;
						print("reset simulation value to:" + simulation);
					}

					if(hit.collider.tag == "VRS2" && change_VRS) {



						youSure.SetActive(true);
						youSureUp = true;



						simulation = 2;
						print("reset simulation value to:" + simulation);
					}
					if(hit.collider.tag == "VRS3" && change_VRS) {


						youSure.SetActive(true);
						youSureUp = true;



						simulation = 3;
						print("reset simulation value to:" + simulation);

						
					}
					if(hit.collider.tag == "VRS4" && change_VRS) {


						youSure.SetActive(true);
						youSureUp = true;

					
						simulation = 4;
						print("reset simulation value to:" + simulation);
					}



					//*********** ARE YOU SURE FUNCTION ************************

					if(youSureUp) {

						if(hit.collider.tag == "yesImSure") {

							if(change_VRS) {

								//*************************** Official change in VRSim *******
								SceneManager.UnloadSceneAsync("_scene_Pause_Menu");
								youSure.SetActive(false);
								youSureUp = false;
								pauseUp = false;
								aVRSIsOpen = true;
								VRS_Splash.SetActive(false);
								sim_change_cubes.SetActive(false);
								change_VRS = false;

								skipTut.SetActive(false);

								if(simulation == 1) {
									SceneManager.LoadSceneAsync("_scene_VRS1", LoadSceneMode.Additive);

									currentsceneloaded = "_scene_VRS1";

								}
								if(simulation == 2) {
									SceneManager.LoadSceneAsync("_scene_VRS2", LoadSceneMode.Additive);
									currentsceneloaded = "_scene_VRS2";
								}
								if(simulation == 3) {
									SceneManager.LoadSceneAsync("_scene_VRS3", LoadSceneMode.Additive);
									currentsceneloaded = "_scene_VRS3";
								}
								if(simulation == 4) {
									SceneManager.LoadSceneAsync("_scene_VRS4", LoadSceneMode.Additive);
									currentsceneloaded = "_scene_VRS4";
								}

							}
							//********************** Change Role Official finalization ********
							if(change_Role) {
								youSure.SetActive(false);
								youSureUp = false;

								change_Role = false;
								Role_Splash.SetActive(false);
								role_change_cubes.SetActive(false);

							}

							//***********exiting out Official *************
							if(exitOut) {
								if(currentsceneloaded == "_scene_VRS1" || currentsceneloaded == "_scene_VRS2" || currentsceneloaded == "_scene_VRS3" || currentsceneloaded == "_scene_VRS4") {

									if(currentsceneloaded == "_scene_VRS1") {
										print("for the love of all that's good, work!");
										SceneManager.UnloadSceneAsync("_scene_VRS1");
									}
									if(currentsceneloaded == "_scene_VRS2") {
										SceneManager.UnloadSceneAsync("_scene_VRS2");
									}
									if(currentsceneloaded == "_scene_VRS3") {
										SceneManager.UnloadSceneAsync("_scene_VRS3");
									}
									if(currentsceneloaded == "_scene_VRS4") {
										SceneManager.UnloadSceneAsync("_scene_VRS4");
									}


									currentsceneloaded = "";
									simulation = 0;
									aVRSIsOpen = false;
									
								}
								youSureUp = false;
								youSure.SetActive(false);

								if(tutorialIsOpen) {
									SceneManager.UnloadSceneAsync("_scene_Tutorial");
									tutorialIsOpen = false;
								}
								exitOut = false;
							}

						}
						if(hit.collider.tag == "noImNot") {

							if(change_VRS) {
								youSure.SetActive(false);
								youSureUp = false;
							}
							if(change_Role) {
								role = 0;
								print("back to 0");
								youSure.SetActive(false);
								youSureUp = false;
							}
							if(exitOut) {
								youSure.SetActive(false);
								youSureUp = false;
								exitOut = false;
							}

						
						}

						}


				}

				//******************** Skip Tutorial Selected? *********************
				if(hit.collider.tag == "skipTut") {
					if(!skipTutYup) {
						skipTut.SetActive(true);
						skipTutYup = true;
						print("skip please");
					}
					else if(skipTutYup) {
						skipTut.SetActive(false);
						skipTutYup = false;
						print("dont skip");
					}

				}
			}
		}

		 //**************************** Get Pause/Menu Up ********************
		if(Input.GetKeyDown("escape") && !pauseUp && !logoIsUp && !endOpen) {
			SceneManager.LoadSceneAsync("_scene_Pause_Menu", LoadSceneMode.Additive);
			pauseUp = true;

			if(skipTutYup == true) {
				skipTut.SetActive(true);
			}

			if(tutorialIsOpen) {
				Restart.text = "";  //removing the idea that you can restart the tutorial
			}
			else {
				Restart.text = "Restart";
			}



		}

	} //is now the new update closer


	IEnumerator logoRun() {

		SceneManager.LoadSceneAsync("_scene_Logo", LoadSceneMode.Additive);
		logoIsUp = true;
		yield return new WaitForSeconds(5);


		if(!initialTut) {
			
			SceneManager.LoadSceneAsync("_scene_Pause_Menu", LoadSceneMode.Additive);
		
			SceneManager.UnloadSceneAsync("_scene_Logo");


			yield return new WaitForSeconds(1);
		
			logoIsUp = false;

		
			pauseUp = true;
		}
		if(initialTut) {
			SceneManager.LoadSceneAsync("_scene_Tutorial", LoadSceneMode.Additive);
			SceneManager.UnloadSceneAsync("_scene_Logo");
			logoIsUp = false;
			tutorialIsOpen = true;

		}
		
	}


	


	//****************************************************************************
	//Here is the old way you made of changing out of a scene 

	/*for(int n = 0; n < SceneManager.sceneCount; ++n) {
			for(int i = 0; i < scenes.Count; i++) {
				if(scenes[i] == SceneManager.GetSceneAt(n).name && Input.GetKeyUp("r") && pauseUp && aVRSIsOpen) {
					//SceneManager.UnloadSceneAsync(i);

					SceneManager.UnloadSceneAsync("_scene_VRS1");

					//Was a change made to what UnloadsceneAsync means???




					//EditorApplication.currentScene

					
					SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive);
					pauseUp = false;

					SceneManager.UnloadSceneAsync("_scene_Pause_Menu");
					print("unload and reload successful");
				}

			}

		}*/



}