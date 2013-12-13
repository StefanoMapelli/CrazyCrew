using UnityEngine;
using System.Collections;

public class ClientBogieCar : MonoBehaviour {

	private bool leverUp;
	private LeverController leverController;
	private BrakeController brakeController;
	private ClientGameManager clientGameManager;

	// Use this for initialization
	void Start () {
		GameObject lever = GameObject.Find("Lever");
		GameObject brake = GameObject.Find("Brake");

		leverController = (LeverController) lever.GetComponent("LeverController");
		brakeController = (BrakeController) brake.GetComponent("BrakeController");

		clientGameManager = (ClientGameManager) gameObject.GetComponent("ClientGameManager");
	}

	[RPC]
	void assignLever1()
	{
		clientGameManager.setRole("Lever1");
		leverController.setRole("Lever1");
		brakeController.setRole("Lever1");

		GUIMenusClient.showLever(true);
		GUIMenusClient.showPauseButton(true); 
	}

	[RPC]
	void assignLever2()
	{
		clientGameManager.setRole("Lever2");
		leverController.setRole("Lever2");
		brakeController.setRole("Lever2");

		GUIMenusClient.showLever(true);
		GUIMenusClient.showPauseButton(true);
	}

	[RPC]
	void blockLever(bool blocked)
	{
		leverController.setBlocked(blocked);
	}
	
	[RPC]
	void assignSteer()
	{
		GUIMenusClient.showSteer(true);
		GUIMenusClient.showPauseButton(true);
		clientGameManager.setRole("Steer");
	}

	[RPC]
	void pushLever(string role, float force)
	{
	}

	[RPC]
	void rotateSteer(float steerRotation) 
	{ 
	}

	[RPC]
	void brakeOn(string role)
	{
	}

	[RPC]
	void brakeOff(string role)
	{
	}
}