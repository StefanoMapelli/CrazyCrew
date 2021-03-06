﻿using UnityEngine;
using System.Collections;
using System.Collections.Specialized;

public class ServerBogieCar : MonoBehaviour {

	private ServerGameManager serverGameManager;
	private BogieCarMovement bogieCar;

	// Use this for initialization
	void Start () 
	{
	}

	void Awake()
	{
		serverGameManager = (ServerGameManager) gameObject.GetComponent("ServerGameManager");
	}

	public void assignRoles()
	{
		ArrayList playersArray = serverGameManager.getPlayers();
		ArrayList assigned = new ArrayList();
		int randomIndex;
		int i = 0;

		while (assigned.Count < playersArray.Count) {
			randomIndex = UnityEngine.Random.Range(0,3);

			if (!assigned.Contains(playersArray[randomIndex])) {

				if (i == 0 && ((Player) playersArray[randomIndex]).getRole() != "Steer") {
					assigned.Add(playersArray[randomIndex]);
					((Player) playersArray[randomIndex]).setRole("Steer");
					networkView.RPC("assignSteer", ((Player)playersArray[randomIndex]).getNetworkPlayer());
					i++;
				}
				else if (i == 1 && ((Player) playersArray[randomIndex]).getRole() != "Lever1") {
					assigned.Add(playersArray[randomIndex]);
					((Player)playersArray[randomIndex]).setRole("Lever1");
					networkView.RPC("assignLever1", ((Player)playersArray[randomIndex]).getNetworkPlayer());
					networkView.RPC("blockLever", ((Player)playersArray[randomIndex]).getNetworkPlayer(), false);
					i++;
				}
				else if (i == 2) {
						assigned.Add(playersArray[randomIndex]);
						((Player)playersArray[randomIndex]).setRole("Lever2");
						networkView.RPC("assignLever2", ((Player)playersArray[randomIndex]).getNetworkPlayer());
						networkView.RPC("blockLever", ((Player)playersArray[randomIndex]).getNetworkPlayer(), true);	
						i++;
				}
			}
		}
	}

	public void initializeBogieCar()
	{
		GameObject o = GameObject.Find("_BogieCarModel");
		bogieCar = (BogieCarMovement)	o.GetComponent("BogieCarMovement");
	}

	public void bonusComunication(int powerUpId)
	{
		if(powerUpId <=4)					
		{
			networkView.RPC ("hasBonus",RPCMode.Others, true, "MISSILE");
		}
		
		else if(powerUpId<=6)		
		{
			networkView.RPC ("hasBonus",RPCMode.Others, true, "POOP");
		}		
		else
		{
			networkView.RPC ("hasBonus",RPCMode.Others, true, "SPEED");
		}
	}

	public void malusComunication(float powerUpId)
	{
		if(powerUpId <= 2.5f)
		{
			networkView.RPC ("hasMalus", RPCMode.Others, true, "MUD");
		}
		
		else if(powerUpId <= 4.5f)
		{
			networkView.RPC ("hasMalus",RPCMode.Others, true, "SLOWDOWN");
		}
		else if(powerUpId >8f)
		{
			networkView.RPC ("hasMalus",RPCMode.Others, true, "STEER FAILURE");
		}
	}

	public void malusEnded()
	{
		networkView.RPC ("hasMalus",RPCMode.Others, false, "");
	}

	public void slowDown(bool slowDown)
	{
		networkView.RPC ("slowDownMalus",RPCMode.Others, slowDown);
	}

	[RPC]
	void assignLever1()
	{
	}
	
	[RPC]
	void assignLever2()
	{
	}

	[RPC]
	void blockLever(bool blocked)
	{
	}

	[RPC]
	void assignSteer()
	{
	}
	
	[RPC]
	void setReady(NetworkPlayer np)
	{
		Player p = serverGameManager.getPlayer(np);
		p.setReady(true);
		Debug.Log("Player: "+np+" ready to play");
	}
	
	[RPC]
	void startGame()
	{
	}

	[RPC]
	void pushLever(string role, float force)
	{

		if(role.Equals("Lever1"))
		{
			networkView.RPC("blockLever", serverGameManager.getPlayerByRole("Lever2").getNetworkPlayer(),false);
		}
		if(role.Equals("Lever2"))
		{
			networkView.RPC("blockLever", serverGameManager.getPlayerByRole("Lever1").getNetworkPlayer(),false);
		}

		bogieCar.LeverDown(force);
	}

	[RPC]
	void rotateSteer(float steerRotation) 
	{
		bogieCar.Steer(steerRotation);
	}

	[RPC]
	void brakeOn(string role)
	{
		if(role.Equals("Lever1"))
		{
			bogieCar.BrakeROn();
		}
		else
		{
			if(role.Equals("Lever2"))
			{
				bogieCar.BrakeLOn();
			}
		}
	}
	
	[RPC]
	void brakeOff(string role)
	{
		if(role.Equals("Lever1"))
		{
			bogieCar.BrakeROff();
		}
		else
		{
			if(role.Equals("Lever2"))
			{
				bogieCar.BrakeLOff();
			}
		}
	}

	[RPC]
	void activateBonus()
	{
		//codice che attiva il bonus acquisito sul veicolo
		bogieCar.getBonus().StartEffect();
		networkView.RPC("hasBonus", RPCMode.Others, false, "");
	}

	[RPC]
	void reduceMalus(string malusName)
	{
		bogieCar.malusReduction(malusName);
	}

	//RPC chiamate per aggiornare lo stato del client riguardo la situazione bonus/malus
	[RPC]
	void hasBonus(bool hasBonus, string bonusName)
	{
	}
	
	[RPC]
	void hasMalus(bool hasMalus, string malusName)
	{
	}

	[RPC]
	void slowDownMalus(bool slowDown)
	{
	}
}