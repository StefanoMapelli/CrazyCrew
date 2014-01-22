using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour {

	private GameObject server;
	public AudioSource buttonSound;

	// Use this for initialization
	void Start () 
	{
		server = GameObject.Find("Server");
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnMouseDown()
	{
		ServerNetworkManager serverNetworkManager = (ServerNetworkManager) server.GetComponent("ServerNetworkManager");
		serverNetworkManager.StartServer();

		Camera.main.transform.Translate(new Vector3(-420f,0f,0f));
		GUIMenusServer.masterServerError(false);
		//GUIMenusServer.mainMenu(false);
		//GUIMenusServer.waitingForPlayersMenu(true);
	}

	void OnMouseUp()
	{
		buttonSound.Play ();
	}
}