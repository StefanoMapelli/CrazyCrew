using UnityEngine;
using System.Collections;
using System;


public class ServerNetworkManager : MonoBehaviour {

	public string masterServerIpAddress;
	private const string typeName = "CrazyCrew";
	private string gameName;
	private ServerGameManager serverGameManager;

	public void StartServer()
	{
		gameName = System.Environment.UserName+" "+DateTime.Now.ToShortTimeString();
	    Network.InitializeServer(3, 25000, false);
	    MasterServer.RegisterHost(typeName, gameName);
	}

	void OnFailedToConnectToMasterServer(NetworkConnectionError info) 
	{
		GUIMenusServer.masterServerError(true);
		Camera.main.transform.position = new Vector3(0f,0f,-20f);
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
		/*MasterServer.ipAddress = masterServerIpAddress;
		MasterServer.port = 23466;
		Network.natFacilitatorIP = masterServerIpAddress;
		Network.natFacilitatorPort = 50005;*/
	}

	// Update is called once per frame
	void Update () 
	{
	}
}