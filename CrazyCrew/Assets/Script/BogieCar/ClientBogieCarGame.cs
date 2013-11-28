using UnityEngine;
using System.Collections;

public class ClientBogieCarGame : MonoBehaviour {

	private bool ready = false;
	private bool playing = false;
	private string role;
	private bool leverUp;
	private float wheelRotation = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if(Network.peerType == NetworkPeerType.Client)
		{
			if (!playing) 
			{
				if (!ready) 
				{
					if (GUI.Button(new Rect(10,10,200,200),"Press to start the game")) 
					{
						ready = true;
						networkView.RPC("setReady",RPCMode.Server);
					}
				}
				else
				{
					GUI.Label (new Rect(10,10,200,200),"Ready to play, waiting for other players...");
				}
			}
			else 
			{
				if((role.Equals("Lever1") || role.Equals("Lever2")) && (leverUp))
				{
					if(GUI.Button(new Rect(100,100,200,200),"Push the Lever"))
					{
						networkView.RPC("pushLever", RPCMode.Server, role);
						leverUp=false;
					}
				}
				if(role.Equals("Wheel"))
				{
					wheelRotation = GUI.HorizontalSlider(new Rect(10, 10, 200, 200), wheelRotation, -1.0f, 1.0f);
					networkView.RPC("rotateWheel", RPCMode.Server, wheelRotation);
				}
			}
		}
	}
	
	[RPC]
	void assignLever1()
	{
		role="Lever1";
		leverUp=true;
	}
	
	[RPC]
	void assignLever2()
	{
		role="Lever2";
		leverUp=false;
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

	[RPC]
	void pushLever(string role)
	{
	}

	[RPC]
	void resetLever()
	{
		leverUp=true;
	}

	[RPC]
	void rotateWheel(float wheelRotation) 
	{ 
	}
}
