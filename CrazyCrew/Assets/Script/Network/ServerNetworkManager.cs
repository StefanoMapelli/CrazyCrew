using UnityEngine;
using System.Collections;
using System;


public class ServerNetworkManager : MonoBehaviour {

	public string masterServerIpAddress;
	private const string typeName = "UniqueGameName";
	private string gameName;
	private ServerGameManager serverGameManager;

	public void StartServer()
	{
		gameName = DateTime.Now.ToString();
	    Network.InitializeServer(3, 25000, false);
	    MasterServer.RegisterHost(typeName, gameName);
	}

	void OnFailedToConnectToMasterServer(NetworkConnectionError info) 
	{
		GUIMenus.waitingForPlayersMenu(false);
		GUIMenus.mainMenu(true);
		GUIMenus.masterServerError(true);
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

		DontDestroyOnLoad(gameObject);/*
		MasterServer.ipAddress = masterServerIpAddress;
		MasterServer.port = 23466;
		Network.natFacilitatorIP = masterServerIpAddress;
		Network.natFacilitatorPort = 50005;*/
	}

	// Update is called once per frame
	void Update () 
	{
	}
}