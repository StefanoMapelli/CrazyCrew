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
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;

		GUIMenusServer.masterServerError(false);

		server = GameObject.Find("Server");
		ServerNetworkManager serverNetworkManager = (ServerNetworkManager) server.GetComponent("ServerNetworkManager");

		serverNetworkManager.StartServer();
		GUIMenusServer.mainMenu(false);
		GUIMenusServer.waitingForPlayersMenu(true);
	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
	}
}