using UnityEngine;
using System.Collections;

public class ClientNetworkManager : MonoBehaviour {
	
		
	private const string typeName = "UniqueGameName";
	private const string gameName = "CrazyCrewServer";
	
	private HostData[] hostList;
 
	private void RefreshHostList()
	{
	    MasterServer.RequestHostList(typeName);
	}
	 
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
	    if (msEvent == MasterServerEvent.HostListReceived)
	        hostList = MasterServer.PollHostList();
	}
	
	// Use this for initialization
	void Start () 
	{
		MasterServer.ipAddress = "192.168.1.4";
		MasterServer.port = 23466;
		Network.natFacilitatorIP = "192.168.1.4";
		Network.natFacilitatorPort = 50005;		
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	private void JoinServer(HostData hostData)
	{
	    Debug.Log("Server Joined1");
		Network.Connect(hostData);
		Debug.Log("Server Joined2");
	}
	 
	void OnConnectedToServer()
	{
	    Debug.Log("Server Joined3");
	}
	
	void OnGUI()
	{
	    if (!Network.isClient)
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
}
