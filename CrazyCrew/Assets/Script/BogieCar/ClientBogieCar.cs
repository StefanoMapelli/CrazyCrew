using UnityEngine;
using System.Collections;

public class ClientBogieCar : MonoBehaviour {

	private bool leverUp;
	public GameObject lever;
	public GameObject leverPlane;
	public GameObject steer;
	private LeverController leverController;

	private ClientGameManager clientGameManager;

	// Use this for initialization
	void Start () {
		leverController = (LeverController) lever.GetComponent("LeverController");
	}

	void Awake()
	{
		clientGameManager = (ClientGameManager) gameObject.GetComponent("ClientGameManager");
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		lever.SetActive(false);
		leverPlane.SetActive(false);
		steer.SetActive(false);
	}

	[RPC]
	void assignLever1()
	{
		lever.SetActive(true);
		leverPlane.SetActive(true);
		clientGameManager.setRole("Lever1"); 
	}

	[RPC]
	void assignLever2()
	{
		lever.SetActive(true);
		leverPlane.SetActive(true);
		clientGameManager.setRole("Lever2");
	}

	[RPC]
	void blockLever(bool blocked)
	{
		leverController.setBlocked(blocked);
	}
	
	[RPC]
	void assignSteer()
	{
		steer.SetActive(true);
		clientGameManager.setRole("Steer");
	}

	[RPC]
	void pushLever(string role, float force)
	{
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
