using UnityEngine;
using System.Collections;

public class ClientGameManager : MonoBehaviour {

	private string role;
	private bool ready = false;
	private bool pause = false;
	
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
			if (role == null) {
				if (!ready) 
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
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 20.0f);
		}
		else
		{
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -10.0f);
		}
	}

	[RPC]
	void clientPause(bool p) {
	}

	[RPC]
	void setReady(NetworkPlayer np)
	{
	}

	/*
	[RPC]
	void startGame()
	{
		playing=true;
	}
	*/

	[RPC]
	void reconnect(NetworkPlayer np, string role)
	{	
	}

	/*
	[RPC]
	void reconnectionGood(string role)
	{
		this.role = role;
		this.ready = true;
	}
	*/

	/*
	[RPC]
	void resumeGame()
	{
		setPause (false);
	}
	*/
}
