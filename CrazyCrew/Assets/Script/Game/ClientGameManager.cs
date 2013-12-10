using UnityEngine;
using System.Collections;

public class ClientGameManager : MonoBehaviour {

	private string role;
	private bool ready = false;
	private bool pause = false;
	private Vector3 controlPosition;
	
	// Use this for initialization
	void Start () 
	{
	}
	

	// Update is called once per frame
	void Update () 
	{
		if (role != null) {
			if (Input.GetKeyDown(KeyCode.P))
			{
				if (pause) 
				{
					networkView.RPC ("clientPause",RPCMode.Server,false);

				}
				else 
				{
					networkView.RPC ("clientPause",RPCMode.Server,true);
				}
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
		//ready=false;
		//playing=false;
		setPause (true);
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
		if(Network.peerType == NetworkPeerType.Client)
		{
			if (role == null)
			{
				if (!ready && !pause) 
				{
					if (GUI.Button(new Rect(10,10,200,200),"Press to start the game")) 
					{
						ready = true;
						networkView.RPC("setReady",RPCMode.Server, Network.player);
					}
				}
				else
				{
					GUI.Label (new Rect(10,10,200,200),"Ready to play, waiting for other players...");
				}
			}
		}

		if(pause)
		{
			if(Network.peerType == NetworkPeerType.Disconnected)
			{
				GUI.Label (new Rect(10,10,200,200),"Attempting to reconnect...");
			}
			else
			{
				GUI.Label (new Rect(10,10,200,200),"Game paused");
			}
		}
	}

	[RPC]
	void setPause(bool pause)
	{
		this.pause = pause;

		if(pause)
		{
			controlPosition=Camera.main.transform.position;
			Camera.main.transform.position = new Vector3(100,1, -10);
		}
		else
		{
			Camera.main.transform.position = controlPosition;
			Debug.Log("Sposto camera");
		}
	}

	[RPC]
	void clientPause(bool p) {
	}

	[RPC]
	void setReady(NetworkPlayer np)
	{
		this.ready=true;
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
			Debug.Log("ci sono");
			networkView.RPC("reconnect",RPCMode.Server, Network.player, "unknown");
		}
		else
		{
			networkView.RPC("reconnect",RPCMode.Server, Network.player, this.role);
		}
	}
}
