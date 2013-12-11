using UnityEngine;
using System.Collections;

public class RefreshList : MonoBehaviour {

	private ClientNetworkManager clientNetworkManager;

	// Use this for initialization
	void Start () {
		clientNetworkManager = (ClientNetworkManager)GameObject.Find("Client").GetComponent("ClientNetworkManager");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		clientNetworkManager.RefreshHostList();
		GUIMenusClient.connectionError(false);
	}
}
