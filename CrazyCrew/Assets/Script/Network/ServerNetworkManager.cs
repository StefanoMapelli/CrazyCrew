using UnityEngine;
using System.Collections;
using System;


public class ServerNetworkManager : MonoBehaviour {
	
	private const string typeName = "UniqueGameName";
	private const string gameName = "CrazyCrewServer";
	
	private void StartServer()
	{
	    Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
	    MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnServerInitialized()
	{
	    Debug.Log("Server Initializied");
	}		

	// Use this for initialization
	void Start () {
		MasterServer.ipAddress = "192.168.1.2";
		MasterServer.port = 23466;
		Network.natFacilitatorIP = "192.168.1.2";
		Network.natFacilitatorPort = 50005;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		if(!Network.isServer)
		 if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
	    	 StartServer();
	}
}
