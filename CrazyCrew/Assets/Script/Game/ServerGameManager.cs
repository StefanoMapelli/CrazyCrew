
using UnityEngine;
using System.Collections;
using System.Threading;

public class ServerGameManager : MonoBehaviour {

	private float initialTimeScale;
	
	private bool playing = false;
	private bool pause = false;
	private int numberOfPlayers = 3;
	private ArrayList players = new ArrayList();
	private ServerBogieCar serverBogieCar;
	private PlayerCount playerCount;
	private RaceManager raceManager;

	public ArrayList getPlayers()
	{
		return players;
	}

	// Use this for initialization
	void Start () 
	{
		initialTimeScale = Time.timeScale;
		serverBogieCar = (ServerBogieCar) gameObject.GetComponent("ServerBogieCar");

		playerCount = (PlayerCount) GameObject.Find ("PlayerCountText").GetComponent("PlayerCount");
	}

	// Update is called once per frame
	void Update () 
	{
		if(allPlayersReady() && !playing && players.Count==numberOfPlayers)
		{

			//inizia partita
			Debug.Log("Inizia la partita");
			playing=true;
			
			Application.LoadLevel("bogieCar");

			Debug.Log("Loading level? "+Application.isLoadingLevel);
		}
	}


	void OnGUI()
	{
		if(pause)
		{
			if(!allPlayersConnected())
			{
				GUI.Label (new Rect(10,10,200,200),"Waiting for players reconnection...");
			}
			else
			{
				GUI.Label (new Rect(10,10,200,200),"Game paused");
			}
		}
	}

	public void OnLevelWasLoaded(int level)
	{
		serverBogieCar.initializeBogieCar();
		serverBogieCar.assignRoles();
		raceManager = (RaceManager) GameObject.Find ("RaceManager").GetComponent ("RaceManager");
	}
	
	public void playerConnection(NetworkPlayer np)
	{
		if(!playing)
		{
			if(players.Count < numberOfPlayers)
			{
				players.Add(new Player(np));
				playerCount.incrementNumber();
			}
		}
		else
		{
			if(!allPlayersConnected())
			{
				networkView.RPC("connectInGame",np);
			}
		}
	}
	
	public void playerDisconnection(NetworkPlayer np)
	{
		if(!playing)
		{
			object o = getPlayer(np);
			players.Remove(o);
			Debug.Log("Player: "+np+" disconnesso");
			playerCount.decrementNumber();
		}
		else
		{
			getPlayer(np).setConnected(false);
			pause = true;
			Time.timeScale = 0;
			raceManager.SetPause(true);
			networkView.RPC("setPause", RPCMode.All, pause);
		}
	}

	public Player getPlayer(NetworkPlayer np)
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
	
	public Player getPlayerByRole(string role)
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
	
	private bool allPlayersReady()
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

	private bool allPlayersConnected()
	{
		Player p;
		foreach(object o in players)
		{
			p = (Player) o;
			if(!p.getConnected())
				return false;
		}
		return true;
	}

	[RPC]
	void setReady(NetworkPlayer np)
	{
		Player p = getPlayer(np);
		p.setReady(true);
		Debug.Log("Player: "+np+" ready to play");
	}

	[RPC]
	void setPause(bool pause)
	{
	}

	[RPC]
	void clientPause(bool p)
	{
		raceManager.SetPause(p);
		if (p) {
			networkView.RPC ("setPause",RPCMode.All,true);
			pause = true;
			Time.timeScale = 0;
		}
		else {
			if (allPlayersConnected()) {
				networkView.RPC("setPause",RPCMode.All,false);
				pause = false;
				Time.timeScale = initialTimeScale;
			}
		}
	}

	[RPC]
	void reconnect(NetworkPlayer np, string role)
	{
		Player p = getPlayerByRole(role);

		//caso riconnessione da client che era in partita
		if(p != null)
		{
			if(!p.getConnected())
			{
				p.setConnected(true);
				p.setNetworkPlayer(np);
			}
			else {
				Network.CloseConnection (np,true);
			}
		}
		//caso riconnessione da client mai stato in partita (per esempio: in caso di app riavviata...)
		else
		{
			Player pl;
			foreach(object o in players)
			{
				pl = (Player) o;
				
				if(!pl.getConnected())
				{
					pl.setConnected(true);
					pl.setNetworkPlayer(np);
					networkView.RPC("setPause", np, true);
					networkView.RPC("setReady",np,np);

					//Da estendere nel caso di aggiunta veicoli (in base al veicolo che stiamo guidando cambierà l'implementazione) 
					if(pl.getRole().Equals("Lever1"))
					{
						networkView.RPC("assignLever1",np);
						networkView.RPC("blockLever",np, false);
						networkView.RPC("blockLever",getPlayerByRole("Lever2").getNetworkPlayer(),true);
						return;
					}
					else
					{
						if(pl.getRole().Equals("Lever2"))
						{	
							networkView.RPC("assignLever2",np);
							networkView.RPC("blockLever",np, true);
							networkView.RPC("blockLever",getPlayerByRole("Lever1").getNetworkPlayer(),false);
							return;
						}
						else
						{
							networkView.RPC("assignSteer",np);
							return;
						}
					}
				}
			}
		}
	}

	[RPC]
	void connectInGame()
	{
	}
}
