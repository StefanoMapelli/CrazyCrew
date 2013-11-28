﻿using UnityEngine;
using System.Collections;

public class ServerBogieCarGame : MonoBehaviour {

	private int numberOfPlayers = 3;
	private int counterConnection = 0;
	private int counterReady = 0;
	private bool playing = false;

	private Hashtable players = new Hashtable();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(counterReady==numberOfPlayers && !playing)
		{
			//inizia partita
			Debug.Log("Inizia la partita");
			networkView.RPC("startGame",RPCMode.All);
			playing=true;
		}
	}
	public void PlayerConnection(NetworkPlayer p)
	{
		if(players.Count < numberOfPlayers)
		{
			counterConnection++;
			switch(counterConnection)
			{
			case 1:
				networkView.RPC("assignLever1",p);
				players.Add("Lever1",p);
				Debug.Log("Leva 1 assegnata");
				break;
			case 2:
				networkView.RPC("assignLever2",p);
				players.Add("Lever2",p);		
				Debug.Log("Leva 2 assegnata");
				break;
			case 3:
				networkView.RPC("assignWheel",p);
				players.Add("Wheel",p);
				Debug.Log("Sterzo assegnato");
				break;
			}
		}
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
	void assignWheel()
	{
	}
	
	[RPC]
	void setReady()
	{
		counterReady++;
		Debug.Log("counterReady: "+counterReady);
	}
	
	[RPC]
	void startGame()
	{
	}

	[RPC]
	void pushLever(string role)
	{
		if(role.Equals("Lever1"))
		{
			networkView.RPC("resetLever", players["Lever1"]);
		}
		if(role.Equals("Lever2"))
		{
			networkView.RPC("resetLever", players["Lever2"])
		}

		//chiama metodo della BOGIE CAR
	}
	
	[RPC]
	void resetLever()
	{
	}
}
