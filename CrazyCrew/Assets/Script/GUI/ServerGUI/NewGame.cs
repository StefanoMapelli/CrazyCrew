using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour {

	private GameObject server;
	public AudioSource buttonSound;

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
		//GUIMenusServer.masterServerError(false);

		server = GameObject.Find("Server");
		ServerNetworkManager serverNetworkManager = (ServerNetworkManager) server.GetComponent("ServerNetworkManager");

		serverNetworkManager.StartServer();
		Camera.main.transform.Translate(new Vector3(520f,58f,0f));
		//GUIMenusServer.mainMenu(false);
		//GUIMenusServer.waitingForPlayersMenu(true);
	}

	void OnMouseUp()
	{
		buttonSound.Play ();
	}
}