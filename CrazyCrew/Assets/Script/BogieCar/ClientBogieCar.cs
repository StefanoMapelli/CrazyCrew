using UnityEngine;
using System.Collections;

public class ClientBogieCar : MonoBehaviour {

	private bool ready = false;
	private bool playing = false;
	private string role;
	private bool leverUp;
	private float steerRotation = 0.0f;
	public GameObject lever;
	public GameObject leverPlane;
	public GameObject steer;
	private LeverController leverController;

	// Use this for initialization
	void Start () {
		leverController = (LeverController) lever.GetComponent("LeverController");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(role != null)
		{
			if((role.Equals("Lever1") || role.Equals("Lever2")) && Input.GetKeyDown(KeyCode.F))
			{
				networkView.RPC("brakeOn",RPCMode.Server, role);
			}

			if((role.Equals("Lever1") || role.Equals("Lever2")) && Input.GetKeyUp(KeyCode.F))
			{
				networkView.RPC("brakeOff",RPCMode.Server, role);
			}
		}
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
						networkView.RPC("setReady",RPCMode.Server, Network.player);
					}
				}
				else
				{
					GUI.Label (new Rect(10,10,200,200),"Ready to play, waiting for other players...");
				}
			}
		}
	}

	public string getRole() {
		return role;
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		ready=false;
		playing=false;
	}

	[RPC]
	void assignLever1()
	{
		lever.SetActive(true);
		leverPlane.SetActive(true);
		role = "Lever1";
		leverController.setBlocked(false);
	}
	
	[RPC]
	void assignLever2()
	{
		lever.SetActive(true);
		leverPlane.SetActive(true);
		role="Lever2";
		leverController.setBlocked(true);
	}
	
	[RPC]
	void assignSteer()
	{
		steer.SetActive(true);
		role="Steer";
	}
	
	[RPC]
	void setReady(NetworkPlayer np)
	{
	}
	
	[RPC]
	void startGame()
	{
		playing=true;
	}

	[RPC]
	void pushLever(string role, float force)
	{
	}

	[RPC]
	void resetLever()
	{
		leverController.setBlocked(false);
	}

	[RPC]
	void rotateSteer(float steerRotation) 
	{ 
	}

	[RPC]
	void brakeOn(string role)
	{
	}

	[RPC]
	void brakeOff(string role)
	{
	}
}
