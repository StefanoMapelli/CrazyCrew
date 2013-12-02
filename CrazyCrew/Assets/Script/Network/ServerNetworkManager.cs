using UnityEngine;
using System.Collections;
using System;


public class ServerNetworkManager : MonoBehaviour {

	public string masterServerIpAddress;
	private const string typeName = "UniqueGameName";
	private const string gameName = "CrazyCrewServer";
	private ServerGameManager serverGameManager;

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
		serverGameManager = (ServerGameManager) gameObject.GetComponent("ServerGameManager");
	}

	void OnPlayerConnected(NetworkPlayer p)
	{
		serverGameManager.playerConnection(p);
	}

	void OnPlayerDisconnected(NetworkPlayer p)
	{
		serverGameManager.playerDisconnection(p);
	}

	// Use this for initialization
	void Start () {

		DontDestroyOnLoad(gameObject);
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
