using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour {

	private GameObject server;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnMouseDown()
	{
		GUIMenus.masterServerError(false);

		server = GameObject.Find("Server");
		ServerNetworkManager serverNetworkManager = (ServerNetworkManager) server.GetComponent("ServerNetworkManager");

		serverNetworkManager.StartServer();
		GUIMenus.mainMenu(false);
		GUIMenus.waitingForPlayersMenu(true);
	}
}