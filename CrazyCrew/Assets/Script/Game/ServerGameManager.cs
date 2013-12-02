using UnityEngine;
using System.Collections;
using System.Threading;

public class ServerGameManager : MonoBehaviour {

	private bool playing = false;
	private int numberOfPlayers = 3;
	private ArrayList players = new ArrayList();
	private ServerBogieCar serverBogieCar;

	public ArrayList getPlayers()
	{
		return players;
	}

	// Use this for initialization
	void Start () 
	{
		serverBogieCar = (ServerBogieCar) gameObject.GetComponent("ServerBogieCar");
	}

	// Update is called once per frame
	void Update () 
	{
		if(allPlayerReady() && !playing && players.Count==numberOfPlayers)
		{

			//inizia partita
			Debug.Log("Inizia la partita");
			playing=true;
			
			Application.LoadLevel("bogieCar");

			Debug.Log("Loading level? "+Application.isLoadingLevel);
		}
	}

	public void OnLevelWasLoaded(int level)
	{
		serverBogieCar.initializeBogieCar();
		serverBogieCar.assignRoles();
		networkView.RPC("startGame",RPCMode.All);
	}
	
	public void playerConnection(NetworkPlayer np)
	{
		if(players.Count < numberOfPlayers)
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
}
