using UnityEngine;
using System.Collections;

public class ClientNetworkManager : MonoBehaviour {

	public string masterServerIpAddress;
	private const string typeName = "UniqueGameName";
	private const string gameName = "CrazyCrewServer";
	private HostData[] hostList;

	private HostData myServer = null;
	private bool reconnectRunning = false;
	private ClientGameManager clientGameManager;

	private bool listArrived = false;

	private ServerList serverList;
 
	// Use this for initialization
	void Start () 
	{/*
		MasterServer.ipAddress = masterServerIpAddress;
		MasterServer.port = 23466;
		Network.natFacilitatorIP = masterServerIpAddress;
		Network.natFacilitatorPort = 50005;	*/

		GameObject client = GameObject.Find ("Client");
		clientGameManager = (ClientGameManager) client.GetComponent("ClientGameManager");
		serverList = (ServerList) GameObject.Find ("ServerList").GetComponent("ServerList");
	}

	public void RefreshHostList()
	{
	    MasterServer.RequestHostList(typeName);
		GUIMenusClient.showServerList(true);
	}
	 
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
	    if (msEvent == MasterServerEvent.HostListReceived) {
	        hostList = MasterServer.PollHostList();
			serverList.setList(hostList);
			if (myServer != null)
				listArrived = true;
		}
	}

	void OnFailedToConnectToMasterServer(NetworkConnectionError e) {
		GUIMenusClient.connectionError(true);
		GUIMenusClient.mainMenu(true);
	}

	void OnFailedToConnect(NetworkConnectionError e) {
		GUIMenusClient.connectionError(true);
		GUIMenusClient.mainMenu(true);
	}

	void OnConnectedToServer() {
		GUIMenusClient.connectionError(false);
		GUIMenusClient.mainMenu(false);
	}

	// Update is called once per frame
	void Update () 
	{
		if (Network.peerType == NetworkPeerType.Disconnected && clientGameManager.getRole() != null) {
			if (!reconnectRunning) {
				StartCoroutine("tryReconnect");
				reconnectRunning = true;
			}
		}
	}
	
	public void JoinServer(HostData hostData)
	{
	    Debug.Log("Server Joined1");
		Network.Connect(hostData);
		myServer = hostData;
		GUIMenusClient.showServerList(false);
		Debug.Log("Server Joined2");
	}
	
	void OnGUI()
	{
		/*
		if (Network.peerType == NetworkPeerType.Disconnected && myServer == null) 
		{	 
	        //if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
	           	//RefreshHostList();
	
        	if (hostList != null)	        	
			{
	            for (int i = 0; i < hostList.Length; i++)
	           	{
					if (GUI.Button(new Rect(0+(160*i), (Screen.height/10)*9, 150, 50), hostList[i].gameName))
	                   	JoinServer(hostList[i]);
	           	}
	       	}
		}
		*/
	}

	private IEnumerator tryReconnect() {
		while (!listArrived) {
			Debug.Log ("list requested, now waiting...");
			MasterServer.RequestHostList(typeName);
			yield return new WaitForSeconds(10);
		}

		bool found = false;
		for (int i = 0; i < hostList.Length; i++) {
			if (hostList[i].gameName == myServer.gameName) {
				found = true;
				break;
			}
		}

		if (found) {
			while (Network.peerType == NetworkPeerType.Disconnected) {
				Network.Connect (myServer);
				yield return new WaitForSeconds(10);
			}
		}
		else {
			Application.LoadLevel ("client");
		}
		reconnectRunning = false;
		listArrived = false;
	}
}