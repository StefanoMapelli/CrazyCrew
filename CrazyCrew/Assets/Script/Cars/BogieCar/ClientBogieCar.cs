using UnityEngine;
using System.Collections;

public class ClientBogieCar : MonoBehaviour {

	private bool leverUp;
	private LeverController leverController;
	private BrakeController brakeController;
	private PowerUpController powerUpController;
	private ClientGameManager clientGameManager;

	private int roleSwitchCounter = 0;

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
		roleSwitchCounter++;

		if(roleSwitchCounter==1)
			gameObject.audio.Play();

		clientGameManager.setRole("Lever1");
		leverController.setRole("Lever1");
		brakeController.setRole("Lever1");
		powerUpController.setRole("Lever1");

		GUIMenusClient.showLever(true);
		GUIMenusClient.showPauseButton(true); 
		GUIMenusClient.showSteer(false);
		GUIMenusClient.showPowerUpController(true);

		GUIMenusClient.controllerPositioning();
	}

	[RPC]
	void assignLever2()
	{
		roleSwitchCounter++;
		
		if(roleSwitchCounter==1)
			gameObject.audio.Play();

		clientGameManager.setRole("Lever2");
		leverController.setRole("Lever2");
		brakeController.setRole("Lever2");
		powerUpController.setRole("Lever2");

		GUIMenusClient.showLever(true);
		GUIMenusClient.showPauseButton(true);
		GUIMenusClient.showSteer(false);
		GUIMenusClient.showPowerUpController(true);

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
		roleSwitchCounter++;
		
		if(roleSwitchCounter==1)
			gameObject.audio.Play();

		GUIMenusClient.showSteer(true);
		GUIMenusClient.showPauseButton(true);
		clientGameManager.setRole("Steer");
		GUIMenusClient.showPowerUpController(false);

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

		if(hasBonus)
			powerUpController.setVibration();

		powerUpController.updateBonusLabel();
	}

	[RPC]
	void hasMalus(bool hasMalus, string malusName)
	{
		powerUpController.setHasMalus(hasMalus);
		powerUpController.setMalusName(malusName);

		if(hasMalus)
			powerUpController.setVibration();

		powerUpController.updateMalusLabel();
	}

	[RPC]
	void slowDownMalus(bool slowDown)
	{
		if(clientGameManager.getRole() == "Lever1" || clientGameManager.getRole() == "Lever2")
		{
			if(slowDown)
			{
				if(!leverController.getBlocked())
				{
					leverController.setBlocked(true);
					leverController.setMalusBlocked(true);
				}
			}
			else
			{
				if(leverController.getMalusBlocked())
				{
					leverController.setBlocked(false);
					leverController.setMalusBlocked(false);
				}
			}
		}
	}
}