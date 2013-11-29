using UnityEngine;
using System.Collections;
using System.Collections.Specialized;

public class ServerBogieCarGame : MonoBehaviour {
	
	private bool playing = false;

	private ArrayList players = new ArrayList();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(allPlayerReady() && !playing && players.Count==3)
		{
			//inizia partita
			Debug.Log("Inizia la partita");
			this.assignRoles();
			networkView.RPC("startGame",RPCMode.All);
			playing=true;
		}
	}

	public void playerConnection(NetworkPlayer np)
	{
		if(players.Count < 3)
		{
			players.Add(new Player(np));
		}
	}

	public void playerDisconnection(NetworkPlayer np)
	{
		object o = getPlayer(np);
		players.Remove(o);
		Debug.Log("Player: "+np+" disconnesso");
	}

	public void assignRoles()
	{
		Player[] playersArray = new Player[3]; 
		players.CopyTo(playersArray);

		playersArray[0].setRole("Lever1");
		networkView.RPC("assignLever1", playersArray[0].getNetworkPlayer());
		Debug.Log("Player "+playersArray[0].getNetworkPlayer()+" ha la leva 1");

		playersArray[1].setRole("Lever2");
		networkView.RPC("assignLever2", playersArray[1].getNetworkPlayer());
		Debug.Log("Player "+playersArray[1].getNetworkPlayer()+" ha la leva 2");

		playersArray[2].setRole("Wheel");
		networkView.RPC("assignWheel", playersArray[2].getNetworkPlayer());
		Debug.Log("Player "+playersArray[2].getNetworkPlayer()+" ha lo sterzo");
	}

	private class Player
	{
		NetworkPlayer networkPlayer;
		string role;
		bool ready=false;

		public Player(NetworkPlayer np)
		{
			networkPlayer=np;
		}

		public bool getReady()
		{
			return ready;
		}

		public void setReady(bool ready)
		{
			this.ready=ready;
		}

		public string getRole()
		{
			return role;
		}

		public void setRole(string role)
		{
			this.role=role;
		}

		public NetworkPlayer getNetworkPlayer()
		{
			return networkPlayer;
		}
	}

	private Player getPlayer(NetworkPlayer np)
	{
		Player p;
		foreach(object o in players)
		{
			p = (Player) o;
			if(p.getNetworkPlayer().Equals(np))
			{
				return p;
			}
		}
		return null;
	}

	private Player getPlayerByRole(string role)
	{
		Player p;
		foreach(object o in players)
		{
			p = (Player) o;
			if(p.getRole().Equals(role))
			{
				return p;
			}
		}
		return null;
	}

	private bool allPlayerReady()
	{
	    Player p;
		foreach(object o in players)
		{
			p = (Player) o;
			if(!p.getReady())
				return false;
		}
		return true;
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
	void assignWheel()
	{
	}
	
	[RPC]
	void setReady(NetworkPlayer np)
	{
		Player p = getPlayer(np);
		p.setReady(true);
		Debug.Log("Player: "+np+" ready to play");
	}
	
	[RPC]
	void startGame()
	{
	}

	[RPC]
	void pushLever(string role)
	{

		if(role.Equals("Lever1"))
		{
			networkView.RPC("resetLever", getPlayerByRole("Lever2").getNetworkPlayer());
		}
		if(role.Equals("Lever2"))
		{
			networkView.RPC("resetLever", getPlayerByRole("Lever1").getNetworkPlayer());
		}

		//chiama metodo della BOGIE CAR per l'AZIONE DELL'ABBASSAMENTO LEVA
	}
	
	[RPC]
	void resetLever()
	{
	}

	[RPC]
	void rotateWheel(float wheelRotation) 
	{
		Debug.Log("Rotate angle recived: "+wheelRotation);
		//chiama metodo della BOGIE CAR per lA ROTAZIONE DELLO STERZO
	}
}
