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
		ArrayList playersArray = serverGameManager.getPlayers();
		ArrayList assigned = new ArrayList();
		int randomIndex;
		int i = 0;

		while (assigned.Count < playersArray.Count) {
			randomIndex = UnityEngine.Random.Range(0,3);

			if (!assigned.Contains(playersArray[randomIndex])) {
				assigned.Add(playersArray[randomIndex]);

				if (i == 0) {
					((Player) playersArray[randomIndex]).setRole("Lever1");
					networkView.RPC("assignLever1", ((Player)playersArray[randomIndex]).getNetworkPlayer());
					networkView.RPC("blockLever", ((Player)playersArray[randomIndex]).getNetworkPlayer(), false);
				}
				else if (i == 1) {
					((Player)playersArray[randomIndex]).setRole("Lever2");
					networkView.RPC("assignLever2", ((Player)playersArray[randomIndex]).getNetworkPlayer());
					networkView.RPC("blockLever", ((Player)playersArray[randomIndex]).getNetworkPlayer(), true);
				}
				else {
					((Player)playersArray[randomIndex]).setRole("Steer");
					networkView.RPC("assignSteer", ((Player)playersArray[randomIndex]).getNetworkPlayer());
				}
				i++;
			}
		}
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
