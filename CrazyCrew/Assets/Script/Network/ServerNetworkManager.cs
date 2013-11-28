﻿using UnityEngine;
using System.Collections;
using System;


public class ServerNetworkManager : MonoBehaviour {

	public string masterServerIpAddress="";
	private const string typeName = "UniqueGameName";
	private const string gameName = "CrazyCrewServer";
	private ServerBogieCarGame sbcg;

	private void StartServer()
	{
	    Network.InitializeServer(4, 25000, false);
	    MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnServerInitialized()
	{
	    Debug.Log("Server Initializied");
	}

	void Awake()
	{
		sbcg = (ServerBogieCarGame) gameObject.GetComponent("ServerBogieCarGame");
	}

	void OnPlayerConnected(NetworkPlayer p)
	{
		sbcg.PlayerConnection(p);
	}

	// Use this for initialization
	void Start () {
		MasterServer.ipAddress = masterServerIpAddress;
		MasterServer.port = 23466;
		Network.natFacilitatorIP = masterServerIpAddress;
		Network.natFacilitatorPort = 50005;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnGUI()
	{
		if(!Network.isServer)
		 if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
	    	 StartServer();
	}
}
