using UnityEngine;
using System.Collections;

public class ClientGameManager : MonoBehaviour {

	private string role;
	private bool ready = false;
	private bool pause = false;
	
	// Use this for initialization
	void Start () 
	{
		//posizionamento dei menu al centro dello schermo
		GUIMenusClient.menuPositioning();
		GUIMenusClient.controllerPositioning();

		LeverController leverController = 
			(LeverController)GameObject.Find ("Lever").GetComponent("LeverController");
		leverController.setBorder();
	}
	

	// Update is called once per frame
	void Update () 
	{
		if(Network.peerType == NetworkPeerType.Client)
		{
			
			if (role == null)
			{
				
				if (!ready && !pause) 
				{
					GUIMenusClient.readyButton(true);
				}
				else
				{
					GUIMenusClient.readyButton("Ready to play...");
				}
				
			}
			else {
				GUIMenusClient.readyButton(false);
			}
		}
	}

	public void resumeGame()
	{
		networkView.RPC ("clientPause",RPCMode.Server,false);
	}

	public void pauseOn()
	{
		networkView.RPC ("clientPause",RPCMode.Server,true);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		setPause (true);
		GUIMenusClient.readyButton(false);
	}
	
	public string getRole() {
		return role;
	}

	public void setRole(string role)
	{
		this.role = role;
	}


	void OnGUI()
	{
		if(pause)
		{
			if(Network.peerType == NetworkPeerType.Disconnected)
			{
				GUI.Label (new Rect(10,10,200,200),"Attempting to reconnect...");
			}
		}
	}

	[RPC]
	void setPause(bool pause)
	{
		this.pause = pause;

		if(pause)
		{
			if(role == "Lever1" || role == "Lever2")
			{
				GUIMenusClient.showLever(false);
			}
			else
			{
				if(role == "Steer")
				{
					GUIMenusClient.showSteer(false);
				}
			}

			GUIMenusClient.showPauseButton(false);
			GUIMenusClient.pauseMenu(true);
		}
		else
		{
			GUIMenusClient.pauseMenu(false);

			if(role == "Lever1" || role == "Lever2")
			{
				GUIMenusClient.showLever(true);
			}
			else
			{
				if(role == "Steer")
				{
					GUIMenusClient.showSteer(true);
				}
			}
			
			GUIMenusClient.showPauseButton(true);
		}
	}

	[RPC]
	void clientPause(bool p) {
	}

	[RPC]
	public void setReady(NetworkPlayer np)
	{
		this.ready=true;
		networkView.RPC("setReady",RPCMode.Server, Network.player);
	}

	[RPC]
	void reconnect(NetworkPlayer np, string role)
	{	
	}

	[RPC]
	void connectInGame()
	{
		if(role == null)
		{		
			networkView.RPC("reconnect",RPCMode.Server, Network.player, "unknown");
		}
		else
		{
			networkView.RPC("reconnect",RPCMode.Server, Network.player, this.role);
		}
	}
}
