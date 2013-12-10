using UnityEngine;
using System.Collections;
using System.Collections.Specialized;

public class ServerBogieCar : MonoBehaviour {

	private ServerGameManager serverGameManager;
	private BogieCarMovement bogieCar;

	// Use this for initialization
	void Start () 
	{
	}

	void Awake()
	{
		serverGameManager = (ServerGameManager) gameObject.GetComponent("ServerGameManager");
	}

	public void assignRoles()
	{
		// SECONDO ME QUESTO E' UN ERRORE ( by Dario)
		//Player[] playersArray = new Player[3]; 
		//serverGameManager.getPlayers().CopyTo(playersArray);

		ArrayList playersArray = serverGameManager.getPlayers();

		((Player) playersArray[0]).setRole("Lever1");
		networkView.RPC("assignLever1", ((Player)playersArray[0]).getNetworkPlayer());
		networkView.RPC("blockLever", ((Player)playersArray[0]).getNetworkPlayer(), false);
		Debug.Log("Player "+((Player)playersArray[0]).getNetworkPlayer()+" ha la leva 1");

		((Player)playersArray[1]).setRole("Lever2");
		networkView.RPC("assignLever2", ((Player)playersArray[1]).getNetworkPlayer());
		networkView.RPC("blockLever", ((Player)playersArray[1]).getNetworkPlayer(), true);
		Debug.Log("Player "+((Player)playersArray[1]).getNetworkPlayer()+" ha la leva 2");

		((Player)playersArray[2]).setRole("Steer");
		networkView.RPC("assignSteer", ((Player)playersArray[2]).getNetworkPlayer());
		Debug.Log("Player "+((Player)playersArray[2]).getNetworkPlayer()+" ha lo sterzo");
	}

	public void initializeBogieCar()
	{
		GameObject o = GameObject.Find("BogieCarModel");
		bogieCar = (BogieCarMovement)	o.GetComponent("BogieCarMovement");
	}

	[RPC]
	void assignLever1()
	{
	}
	
	[RPC]
	void assignLever2()
	{
	}

	[RPC]
	void blockLever(bool blocked)
	{
	}

	[RPC]
	void assignSteer()
	{
	}
	
	[RPC]
	void setReady(NetworkPlayer np)
	{
		Player p = serverGameManager.getPlayer(np);
		p.setReady(true);
		Debug.Log("Player: "+np+" ready to play");
	}
	
	[RPC]
	void startGame()
	{
	}

	[RPC]
	void pushLever(string role, float force)
	{

		if(role.Equals("Lever1"))
		{
			networkView.RPC("blockLever", serverGameManager.getPlayerByRole("Lever2").getNetworkPlayer(),false);
		}
		if(role.Equals("Lever2"))
		{
			networkView.RPC("blockLever", serverGameManager.getPlayerByRole("Lever1").getNetworkPlayer(),false);
		}

		bogieCar.LeverDown(force);
	}

	[RPC]
	void rotateSteer(float steerRotation) 
	{
		Debug.Log("Rotate angle recived: "+steerRotation);

		bogieCar.Steer(steerRotation);
	}

	[RPC]
	void brakeOn(string role)
	{
		if(role.Equals("Lever1"))
		{
			bogieCar.BrakeROn();
		}
		else
		{
			if(role.Equals("Lever2"))
			{
				bogieCar.BrakeLOn();
			}
		}
	}
	
	[RPC]
	void brakeOff(string role)
	{
		if(role.Equals("Lever1"))
		{
			bogieCar.BrakeROff();
		}
		else
		{
			if(role.Equals("Lever2"))
			{
				bogieCar.BrakeLOff();
			}
		}
	}
}
