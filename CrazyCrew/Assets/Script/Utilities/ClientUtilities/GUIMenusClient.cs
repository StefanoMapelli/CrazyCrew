using UnityEngine;
using System.Collections;

public static class GUIMenusClient {

	//chiamato solo una volta all'inzio , al fine di centrare il MainMenu e il Pause Menu nello schermo
	public static void menuPositioning()
	{
		//vettore utilizzato per mandenere costante la coordinata z per tutti gli oggetti
		Vector3 cost = new Vector3(0,0,-1);

		GameObject refreshList = GameObject.Find("RefreshListText");
		GameObject quitButton = GameObject.Find("Quit");
		GameObject resumeGame = GameObject.Find("ResumeGameButton");
		GameObject exit = GameObject.Find("ExitToMenuButton");
		GameObject backButton = GameObject.Find ("BackButton");
		GameObject nextButton = GameObject.Find ("NextButton");
		GameObject returnButton = GameObject.Find ("ReturnButton");
		GameObject label = GameObject.Find ("Label");
		GameObject serverList = GameObject.Find ("ServerList");
		GameObject restartButton = GameObject.Find ("RestartButton");
		GameObject exitButton = GameObject.Find ("ExitButton");
		GameObject restartFromPause = GameObject.Find ("RestartFromPauseButton");

		Vector3 screenPos = Camera.main.WorldToScreenPoint(cost);

		refreshList.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,(Screen.height/3)*2, screenPos.z));
		quitButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/2, screenPos.z));

		resumeGame.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,(Screen.height/3)*2, screenPos.z));
		exit.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/2, screenPos.z));
		restartFromPause.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/3,screenPos.z));

		backButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/3,Screen.height/2, screenPos.z));
		nextButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3((Screen.width/3)*2,Screen.height/2, screenPos.z));
		serverList.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/2, screenPos.z));
		returnButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/8,Screen.height/8, screenPos.z));
		label.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,(Screen.height/10)*9, screenPos.z));

		restartButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,(Screen.height/3)*2, screenPos.z));
		exitButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/2, screenPos.z));
	}

	public static void controllerPositioning() {
		Vector3 cost = new Vector3(0,0,-1);

		GameObject leftSteer = GameObject.Find ("LeftSteer");
		GameObject rightSteer = GameObject.Find ("RightSteer");
		GameObject pause = GameObject.Find ("Pause");
		GameObject lever = GameObject.Find("Lever");
		GameObject leverPlane = GameObject.Find("LeverPlane");
		GameObject brake = GameObject.Find("Brake");
		GameObject leverInfo = GameObject.Find("LeverInfo");
		GameObject powerUpButton = GameObject.Find ("PowerUpButton");

		Vector3 screenPos = Camera.main.WorldToScreenPoint(cost);

		// try to scale correctly the steer controls
		float height = Camera.main.orthographicSize * 2f;
		float width = (height * Screen.width / Screen.height);
		// LEVER
		leverPlane.transform.localScale = new Vector3(width/25,(height/4)*3,1f);
		lever.transform.localScale = new Vector3(width/6,width/(100f/3f),1f);
		leverPlane.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/4,Screen.height/2, screenPos.z));
		leverInfo.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/4,Screen.height/16, screenPos.z));
		// BRAKE
		brake.transform.localScale = new Vector3(width/2,height/3,1f);
		brake.transform.position = Camera.main.ScreenToWorldPoint (new Vector3((Screen.width/4)*3,Screen.height-(Screen.height/6),screenPos.z));
		// POWER-UP BUTTON
		powerUpButton.transform.localScale = new Vector3(width/2,height/3,1f);
		powerUpButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3((Screen.width/4)*3,Screen.height/2, screenPos.z));

		// PAUSE
		pause.transform.localScale = new Vector3(width/2,height/3,1f);
		pause.transform.position = Camera.main.ScreenToWorldPoint (new Vector3((Screen.width/4)*3,Screen.height-((Screen.height/6)*5), screenPos.z));
		// STEER
		leftSteer.transform.localScale = new Vector3(width/3, height, 1f);
		rightSteer.transform.localScale = new Vector3(width/3, height, 1f);
		leftSteer.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/6,Screen.height/2, screenPos.z));
		rightSteer.transform.position = Camera.main.ScreenToWorldPoint (new Vector3((Screen.width/6)*5,Screen.height/2, screenPos.z));

		//estremo superiore del LeverPlane
		float sup = leverPlane.transform.position.y + 
			(((MeshRenderer)leverPlane.GetComponent("MeshRenderer")).bounds.size.y)/2f
			- (((MeshRenderer)lever.GetComponent("MeshRenderer")).bounds.size.y)/2f;
		lever.transform.position = 
			new Vector3(leverPlane.transform.position.x,sup,-2f);
	}

	public static void mainMenu(bool enabled)
	{
		GameObject refreshList = GameObject.Find("RefreshListText");
		GameObject quitButton = GameObject.Find("Quit");

		((MeshRenderer) refreshList.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) refreshList.GetComponent("BoxCollider")).enabled = enabled;
		((MeshRenderer) quitButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) quitButton.GetComponent("BoxCollider")).enabled = enabled;
	}

	public static void connectionError(bool enabled) {
		GameObject connectionError = GameObject.Find("ConnectionErrorText");

		Vector3 screenPos = Camera.main.WorldToScreenPoint(connectionError.transform.position);
		connectionError.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/10, screenPos.z));

		((MeshRenderer) connectionError.GetComponent("MeshRenderer")).enabled = enabled;

		if(enabled)
			connectionError.audio.Play();
	}

	public static void readyButton(bool enabled) {
		GameObject readyButton = GameObject.Find("ReadyText");

		((MeshRenderer) readyButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) readyButton.GetComponent("BoxCollider")).enabled = enabled;
	}

	public static void readyButton(string msg) {
		TextMesh readyButtonMesh = (TextMesh) GameObject.Find("ReadyText").GetComponent("TextMesh");
		readyButtonMesh.text = msg;
	}

	public static void pauseMenu(bool enabled)
	{
		GameObject resumeGame = GameObject.Find("ResumeGameButton");
		GameObject exit = GameObject.Find("ExitToMenuButton");
		GameObject restartFromPause = GameObject.Find ("RestartFromPauseButton");

		((MeshRenderer) resumeGame.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) resumeGame.GetComponent("BoxCollider")).enabled = enabled;
		
		((MeshRenderer) exit.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) exit.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) restartFromPause.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) restartFromPause.GetComponent("BoxCollider")).enabled = enabled;
	}

	//metodi per far vedere i controllers
	public static void showLever(bool enabled)
	{
		GameObject lever = GameObject.Find("Lever");
		GameObject leverPlane = GameObject.Find("LeverPlane");
		GameObject brake = GameObject.Find("Brake");
		GameObject leverInfo = GameObject.Find("LeverInfo");

		((MeshRenderer) lever.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) lever.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) leverPlane.GetComponent("MeshRenderer")).enabled = enabled;

		((MeshRenderer) brake.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) brake.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) leverInfo.GetComponent("MeshRenderer")).enabled = enabled;
	}

	public static void showPauseButton(bool enabled)
	{
		GameObject pause = GameObject.Find("Pause"); 

		((MeshRenderer) pause.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) pause.GetComponent("BoxCollider")).enabled = enabled;
	}

	public static void showSteer(bool enabled)
	{
		GameObject leftSteer = GameObject.Find("LeftSteer"); 
		GameObject rightSteer = GameObject.Find("RightSteer");

		((MeshRenderer) leftSteer.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) leftSteer.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) rightSteer.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) rightSteer.GetComponent("BoxCollider")).enabled = enabled;
	}

	public static void showPowerUpController(bool enabled)
	{
		GameObject powerUpButton = GameObject.Find ("PowerUpButton");

		((MeshRenderer) powerUpButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) powerUpButton.GetComponent("BoxCollider")).enabled = enabled;
	}

	public static void showServerList(bool enabled) 
	{
		GameObject backButton = GameObject.Find ("BackButton");
		GameObject nextButton = GameObject.Find ("NextButton");
		GameObject serverList = GameObject.Find ("ServerList");
		GameObject returnButton = GameObject.Find ("ReturnButton");
		GameObject label = GameObject.Find ("Label");

		((MeshRenderer) backButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) backButton.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) nextButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) nextButton.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) serverList.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) serverList.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) returnButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) returnButton.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) label.GetComponent("MeshRenderer")).enabled = enabled;
	}

	public static void showEndMenu(bool enabled) {
		GameObject restartButton = GameObject.Find ("RestartButton");
		GameObject exitButton = GameObject.Find ("ExitButton");

		((MeshRenderer) restartButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) restartButton.GetComponent("BoxCollider")).enabled = enabled;
		
		((MeshRenderer) exitButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) exitButton.GetComponent("BoxCollider")).enabled = enabled;
	}

	public static void positioningSteerPause() {
		GameObject pause = GameObject.Find ("Pause");
		Vector3 cost = new Vector3(0,0,-1);

		Vector3 screenPos = Camera.main.WorldToScreenPoint(cost);

		// place pause button and pause label in the center
		pause.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2f,Screen.height/2f, screenPos.z));

		// scale pause button properly
		float height = Camera.main.orthographicSize * 2f;
		float width = (height * Screen.width / Screen.height);
		pause.transform.localScale = new Vector3(width/3, height, 1f);
	}
}
