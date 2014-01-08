using UnityEngine;
using System.Collections;

public class ClientBogieCar : MonoBehaviour {

	private bool leverUp;
	private LeverController leverController;
	private BrakeController brakeController;
	private PowerUpController powerUpController;
	private ClientGameManager clientGameManager;

	// Use this for initialization
	void Start () {
		GameObject lever = GameObject.Find("Lever");
		GameObject brake = GameObject.Find("Brake");
		GameObject powerUpButton = GameObject.Find("PowerUpButton");

		leverController = (LeverController) lever.GetComponent("LeverController");
		brakeController = (BrakeController) brake.GetComponent("BrakeController");
		powerUpController = (PowerUpController) powerUpButton.GetComponent("PowerUpController");

		clientGameManager = (ClientGameManager) gameObject.GetComponent("ClientGameManager");
	}

	[RPC]
	void assignLever1()
	{
		clientGameManager.setRole("Lever1");
		leverController.setRole("Lever1");
		brakeController.setRole("Lever1");
		powerUpController.setRole("Lever1");

		GUIMenusClient.showLever(true);
		GUIMenusClient.showPauseButton(true); 
		GUIMenusClient.showSteer(false);
		GUIMenusClient.showPowerUpController(true, "Lever1");

		GUIMenusClient.controllerPositioning();
	}

	[RPC]
	void assignLever2()
	{
		clientGameManager.setRole("Lever2");
		leverController.setRole("Lever2");
		brakeController.setRole("Lever2");
		powerUpController.setRole("Lever2");

		GUIMenusClient.showLever(true);
		GUIMenusClient.showPauseButton(true);
		GUIMenusClient.showSteer(false);
		GUIMenusClient.showPowerUpController(true, "Lever2");

		GUIMenusClient.controllerPositioning();
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
		GUIMenusClient.showPowerUpController(false, "Steer");

		GUIMenusClient.showLever(false);
		GUIMenusClient.positioningSteerPause();
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

	[RPC]
	void activateBonus()
	{
	}
	
	[RPC]
	void reduceMalus(string malusName)
	{
	}

	[RPC]
	void hasBonus(bool hasBonus, string bonusName)
	{
		powerUpController.setHasBonus(hasBonus);
		powerUpController.setBonusName(bonusName);
		powerUpController.updateBonusLabel();
	}

	[RPC]
	void hasMalus(bool hasMalus, string malusName)
	{
		powerUpController.setHasMalus(hasMalus);
		powerUpController.setMalusName(malusName);
		powerUpController.updateMalusLabel();
	}
}