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
		GameObject serverList = GameObject.Find ("ServerList");
		GameObject restartButton = GameObject.Find ("RestartButton");
		GameObject exitButton = GameObject.Find ("ExitButton");

		Vector3 screenPos = Camera.main.WorldToScreenPoint(cost);

		refreshList.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,(Screen.height/3)*2, screenPos.z));
		quitButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/2, screenPos.z));

		resumeGame.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,(Screen.height/3)*2, screenPos.z));
		exit.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/2, screenPos.z));

		backButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/6,Screen.height/10, screenPos.z));
		nextButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3((Screen.width/6)*5,Screen.height/10, screenPos.z));
		serverList.transform.position = Camera.main.ScreenToWorldPoint (new Vector3((Screen.width/6)*3,Screen.height/3, screenPos.z));

		restartButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,(Screen.height/3)*2, screenPos.z));
		exitButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/2, screenPos.z));
	}

	public static void controllerPositioning() {
		Vector3 cost = new Vector3(0,0,-1);

		GameObject steer = GameObject.Find ("Steer");
		GameObject pause = GameObject.Find ("Pause");
		GameObject pauseLabel = GameObject.Find ("PauseLabel");
		GameObject lever = GameObject.Find("Lever");
		GameObject leverPlane = GameObject.Find("LeverPlane");
		GameObject brake = GameObject.Find("Brake");
		GameObject brakeLabel = GameObject.Find("BrakeLabel");
		GameObject leverInfo = GameObject.Find("LeverInfo");

		Vector3 screenPos = Camera.main.WorldToScreenPoint(cost);

		steer.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/2, screenPos.z)); 
		pause.transform.position = Camera.main.ScreenToWorldPoint (new Vector3((Screen.width/10)*9,Screen.height/10, screenPos.z));
		pauseLabel.transform.position = Camera.main.ScreenToWorldPoint (new Vector3((Screen.width/10)*9,(Screen.height/5), screenPos.z));

		leverPlane.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/3,Screen.height/2, screenPos.z));
		brake.transform.position = Camera.main.ScreenToWorldPoint (new Vector3((Screen.width/3)*2,Screen.height/2, screenPos.z));
		brakeLabel.transform.position = Camera.main.ScreenToWorldPoint (new Vector3((Screen.width/3)*2,(Screen.height/10)*6, screenPos.z));
		leverInfo.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/3,Screen.height/5, screenPos.z));

		float sup = leverPlane.transform.position.y + 
			(((MeshRenderer)leverPlane.GetComponent("MeshRenderer")).bounds.size.y)/2f;
		lever.transform.position = 
			new Vector3(leverPlane.transform.position.x,sup,-1f);
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

		((MeshRenderer) resumeGame.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) resumeGame.GetComponent("BoxCollider")).enabled = enabled;
		
		((MeshRenderer) exit.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) exit.GetComponent("BoxCollider")).enabled = enabled;
	}

	//metodi per far vedere i controllers
	public static void showLever(bool enabled)
	{
		GameObject lever = GameObject.Find("Lever");
		GameObject leverPlane = GameObject.Find("LeverPlane");
		GameObject brake = GameObject.Find("Brake");
		GameObject brakeLabel = GameObject.Find("BrakeLabel");
		GameObject leverInfo = GameObject.Find("LeverInfo");

		((MeshRenderer) lever.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) lever.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) leverPlane.GetComponent("MeshRenderer")).enabled = enabled;

		((MeshRenderer) brake.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) brake.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) brakeLabel.GetComponent("MeshRenderer")).enabled = enabled;
		((MeshRenderer) leverInfo.GetComponent("MeshRenderer")).enabled = enabled;
	}

	public static void showPauseButton(bool enabled)
	{
		GameObject pause = GameObject.Find("Pause"); 
		GameObject pauseLabel = GameObject.Find("PauseLabel");

		((MeshRenderer) pause.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) pause.GetComponent("BoxCollider")).enabled = enabled;
		
		((MeshRenderer) pauseLabel.GetComponent("MeshRenderer")).enabled = enabled;
	}

	public static void showSteer(bool enabled)
	{
		GameObject steer = GameObject.Find("Steer"); 
		GameObject steerFront = GameObject.Find("SteerFront");

		((MeshRenderer) steer.GetComponent("MeshRenderer")).enabled = enabled;
		((SphereCollider) steer.GetComponent("SphereCollider")).enabled = enabled;

		((MeshRenderer) steerFront.GetComponent("MeshRenderer")).enabled = enabled;
	}

	public static void showServerList(bool enabled) 
	{
		GameObject backButton = GameObject.Find ("BackButton");
		GameObject nextButton = GameObject.Find ("NextButton");
		GameObject serverList = GameObject.Find ("ServerList");

		((MeshRenderer) backButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) backButton.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) nextButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) nextButton.GetComponent("BoxCollider")).enabled = enabled;

		((MeshRenderer) serverList.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) serverList.GetComponent("BoxCollider")).enabled = enabled;
	}

	public static void showEndMenu(bool enabled) {
		GameObject restartButton = GameObject.Find ("RestartButton");
		GameObject exitButton = GameObject.Find ("ExitButton");

		((MeshRenderer) restartButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) restartButton.GetComponent("BoxCollider")).enabled = enabled;
		
		((MeshRenderer) exitButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) exitButton.GetComponent("BoxCollider")).enabled = enabled;
	}
}
