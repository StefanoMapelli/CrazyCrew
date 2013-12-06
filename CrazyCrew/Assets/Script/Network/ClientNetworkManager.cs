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
 
	// Use this for initialization
	void Start () 
	{
		MasterServer.ipAddress = masterServerIpAddress;
		MasterServer.port = 23466;
		Network.natFacilitatorIP = masterServerIpAddress;
		Network.natFacilitatorPort = 50005;	

		GameObject client = GameObject.Find ("Client");
		clientGameManager = (ClientGameManager) client.GetComponent("ClientGameManager");
	}

	private void RefreshHostList()
	{
	    MasterServer.RequestHostList(typeName);
	}
	 
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
	    if (msEvent == MasterServerEvent.HostListReceived)
	        hostList = MasterServer.PollHostList();
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
	
	private void JoinServer(HostData hostData)
	{
	    Debug.Log("Server Joined1");
		Network.Connect(hostData);
		myServer = hostData;
		Debug.Log("Server Joined2");
	}
	
	void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Disconnected && myServer == null) 
		{	 
	        if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
	           	RefreshHostList();
	
        	if (hostList != null)	        	
			{
	            for (int i = 0; i < hostList.Length; i++)
	           	{
	               	if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
	                   	JoinServer(hostList[i]);
	           	}
	       	}
		}
	}

	private IEnumerator tryReconnect() {
		while (Network.peerType == NetworkPeerType.Disconnected) {
			Debug.Log ("I'm disconnected, trying to reconnect to server...");
			Network.Connect (myServer);
			yield return new WaitForSeconds(5);
		}
		networkView.RPC ("reconnect",RPCMode.Server,Network.player,clientGameManager.getRole ());
		reconnectRunning = false;

	}
}
