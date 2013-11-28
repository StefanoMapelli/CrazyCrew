﻿using UnityEngine;
using System.Collections;

public class ClientNetworkManager : MonoBehaviour {

	public string masterServerIpAddress="";
	private const string typeName = "UniqueGameName";
	private const string gameName = "CrazyCrewServer";
	private bool ready = false;
	private bool playing = false;
	private HostData[] hostList;
	private string role;
 
	// Use this for initialization
	void Start () 
	{
		MasterServer.ipAddress = masterServerIpAddress;
		MasterServer.port = 23466;
		Network.natFacilitatorIP = masterServerIpAddress;
		Network.natFacilitatorPort = 50005;		
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
	}
	
	private void JoinServer(HostData hostData)
	{
	    Debug.Log("Server Joined1");
		Network.Connect(hostData);
		Debug.Log("Server Joined2");
	}
	
	void OnGUI()
	{
		if (!playing) 
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
			else {
				if (!ready) {
					if (GUI.Button(new Rect(10,10,200,200),"Press to start the game")) {
						ready = true;
						networkView.RPC("setReady",RPCMode.Server);
					}
				}
				else {
					GUI.Label (new Rect(10,10,200,200),"Ready to play, waiting for other players...");
				}
			}
		}
		else {
			GUI.Label (new Rect(20,20,100,100),"PLAYING");
		}
	}

	[RPC]
	void assignLever1()
	{
		role="Lever1";
	}
	
	[RPC]
	void assignLever2()
	{
		role="Lever2";
	}
	
	[RPC]
	void assignWheel()
	{
		role="Wheel";
	}
	
	[RPC]
	void setReady()
	{
	}
	
	[RPC]
	void startGame()
	{
		playing=true;
	}

}
