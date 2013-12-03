﻿using UnityEngine;
using System.Collections;

public class ClientBogieCar : MonoBehaviour {

	private bool ready = false;
	private bool playing = false;
	private string role;
	private bool leverUp;
	private float steerRotation = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(role != null)
		{
			if((role.Equals("Lever1") || role.Equals("Lever2")) && Input.GetKeyDown(KeyCode.F))
			{
				networkView.RPC("brakeOn",RPCMode.Server, role);
			}

			if((role.Equals("Lever1") || role.Equals("Lever2")) && Input.GetKeyUp(KeyCode.F))
			{
				networkView.RPC("brakeOff",RPCMode.Server, role);
			}
		}
	}

	void OnGUI()
	{
		if(Network.peerType == NetworkPeerType.Client)
		{
			if (!playing) 
			{
				if (!ready) 
				{
					if (GUI.Button(new Rect(10,10,200,200),"Press to start the game")) 
					{
						ready = true;
						networkView.RPC("setReady",RPCMode.Server, Network.player);
					}
				}
				else
				{
					GUI.Label (new Rect(10,10,200,200),"Ready to play, waiting for other players...");
				}
			}
			else 
			{
				if((role.Equals("Lever1") || role.Equals("Lever2")) && (leverUp))
				{
					if(GUI.Button(new Rect(100,100,200,200),"Push the Lever"))
					{
						networkView.RPC("pushLever", RPCMode.Server, role, 1f);
						leverUp=false;
					}
				}
				if(role.Equals("Steer"))
				{
					steerRotation = GUI.HorizontalSlider(new Rect(10, 10, 200, 200), steerRotation, -1.0f, 1.0f);
					networkView.RPC("rotateSteer", RPCMode.Server, steerRotation);
				}
			}
		}
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		ready=false;
		playing=false;
	}

	[RPC]
	void assignLever1()
	{
		role="Lever1";
		leverUp=true;
	}
	
	[RPC]
	void assignLever2()
	{
		role="Lever2";
		leverUp=false;
	}
	
	[RPC]
	void assignSteer()
	{
		role="Steer";
	}
	
	[RPC]
	void setReady(NetworkPlayer np)
	{
	}
	
	[RPC]
	void startGame()
	{
		playing=true;
	}

	[RPC]
	void pushLever(string role, float force)
	{
	}

	[RPC]
	void resetLever()
	{
		leverUp=true;
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
