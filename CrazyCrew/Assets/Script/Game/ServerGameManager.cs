using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class ServerGameManager : MonoBehaviour {

	private float initialTimeScale;
	
	private bool playing = false;
	private bool pause = false;
	private int numberOfPlayers = 3;
	private ArrayList players = new ArrayList();
	private ServerBogieCar serverBogieCar;
	//private PlayerCount playerCount;
	private RaceManager raceManager;

	public AudioSource restart;
	public AudioSource exit;

	private ClientIndicator player1,player2,player3;

	private int level;

	public ArrayList getPlayers()
	{
		return players;
	}

	// Use this for initialization
	void Start () 
	{
		initialTimeScale = Time.timeScale;
		serverBogieCar = (ServerBogieCar) gameObject.GetComponent("ServerBogieCar");

		player1 = (ClientIndicator) GameObject.Find("Player1").GetComponent("ClientIndicator");
		player2 = (ClientIndicator) GameObject.Find("Player2").GetComponent("ClientIndicator");
		player3 = (ClientIndicator) GameObject.Find("Player3").GetComponent("ClientIndicator");
		//playerCount = (PlayerCount) GameObject.Find ("PlayerCountText").GetComponent("PlayerCount");
	}

	// Update is called once per frame
	void Update () 
	{
		if(allPlayersReady() && !playing && players.Count==numberOfPlayers)
		{

			//inizia partita
			Debug.Log("Inizia la partita");
			playing=true;

			switch (level) {
				case 1:
				{
					Application.LoadLevel("bogieCar");
					break;
				}
				case 2: 
				{
					Application.LoadLevel("bogieCar2");
					break;
				}
			}

			Debug.Log("Loading level? "+Application.isLoadingLevel);
		}

		if(pause)
		{
			if(!allPlayersConnected())
			{
				raceManager.setInfoText("Waiting for players reconnection...");
			}
			else
			{
				raceManager.setInfoText("Game paused");
			}
		}
	}

	//resto in attesa dell'evento di fine gara
	
	public void OnLevelWasLoaded(int level)
	{
		if (level == 1 || level == 2) {
			serverBogieCar.initializeBogieCar();
			Debug.Log("Livello ricaricato");
			Time.timeScale = initialTimeScale;
			//serverBogieCar.assignRoles();
			raceManager = (RaceManager) GameObject.Find ("RaceManager").GetComponent ("RaceManager");
			//aggiungo l'evento di fine gara
			raceManager.RaceFinish+=new EventHandler(RaceFinish);
		}
	}

	private void RaceFinish(System.Object sender, EventArgs e)
	{
		Debug.Log ("race finished, sending signal to clients");
		networkView.RPC ("endGame",RPCMode.All);
	}

	public void playerConnection(NetworkPlayer np)
	{
		if(!playing)
		{
			if(players.Count < numberOfPlayers)
			{
				players.Add(new Player(np));
				if (players.Count == 1) {
					player1.setConnected(true);
					player2.setConnected(false);
					player3.setConnected(false);
				}
				else if (players.Count == 2) {
					player1.setConnected(true);
					player2.setConnected(true);
					player3.setConnected(false);
				}
				else if (players.Count == 3) {
					player1.setConnected(true);
					player2.setConnected(true);
					player3.setConnected(true);
				}
				//playerCount.incrementNumber();
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

			if (players.Count == 1) {
				player1.setConnected(true);
				player2.setConnected(false);
				player3.setConnected(false);
			}
			else if (players.Count == 2) {
				player1.setConnected(true);
				player2.setConnected(true);
				player3.setConnected(false);
			}
			else if (players.Count == 0) {
				player1.setConnected(false);
				player2.setConnected(false);
				player3.setConnected(false);
			}
			//playerCount.decrementNumber();
		}
		else
		{
			getPlayer(np).setConnected(false);
			pause = true;
			Time.timeScale = 0;
			raceManager.SetPause(true);
			networkView.RPC("setPause", RPCMode.All, pause);

			//exit from the game if all the players are disconnected
			if (allPlayersDisconnected())
			{
				GameObject server = GameObject.Find ("Server");
				Destroy(server);
				Application.LoadLevel ("Server");
			}
		}
	}

	public RaceManager getRaceManager()
	{
		return raceManager;
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

	private bool allPlayersDisconnected()
	{
		Player p;
		foreach(object o in players)
		{
			p = (Player) o;
			if(p.getConnected())
				return false;
		}
		return true;
	}

	public void setLevel(int level) {
		this.level = level;
	}

	public int getLevel() {
		return level;
	}

	public void disconnectPlayers() {
		foreach (Player p in players) {
			Network.CloseConnection(p.getNetworkPlayer(),true);
		}
		players.Clear();
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
		if (p) {
			raceManager.SetPause(p);
			networkView.RPC ("setPause",RPCMode.All,true);
			pause = true;
			Time.timeScale = 0;
		}
		else {
			if (allPlayersConnected()) {
				raceManager.SetPause(p);
				networkView.RPC("setPause",RPCMode.All,false);
				raceManager.setInfoText("");
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
					networkView.RPC("setReady",np,np);

					//Da estendere nel caso di aggiunta veicoli (in base al veicolo che stiamo guidando cambierà l'implementazione) 
					if(pl.getRole().Equals("Lever1"))
					{
						networkView.RPC("assignLever1",np);
						networkView.RPC("blockLever",np, false);
						networkView.RPC("blockLever",getPlayerByRole("Lever2").getNetworkPlayer(),true);
						networkView.RPC("setPause", np, true);
						return;
					}
					else
					{
						if(pl.getRole().Equals("Lever2"))
						{	
							networkView.RPC("assignLever2",np);
							networkView.RPC("blockLever",np, true);
							networkView.RPC("blockLever",getPlayerByRole("Lever1").getNetworkPlayer(),false);
							networkView.RPC("setPause", np, true);
							return;
						}
						else
						{
							networkView.RPC("assignSteer",np);
							networkView.RPC("setPause", np, true);
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

	[RPC]
	void showEndMenu() {
	}

	[RPC]
	public void exitGame() {
		exit.Play();
		networkView.RPC ("exitGame",RPCMode.Others);
		Network.Disconnect();
		GameObject.Destroy(GameObject.Find("Server"));
		Application.LoadLevel("server");
	}

	[RPC]
	public void restartGame() {
		restart.Play();
		networkView.RPC ("restartGame",RPCMode.Others);
		Debug.Log("restart la gara");
		pause = false;
		networkView.RPC ("hasBonus",RPCMode.Others,false,"");
		networkView.RPC ("hasMalus",RPCMode.Others,false,"");
		Application.LoadLevel("restartLevel");
	}

	[RPC]
	void endGame() {
	}
}
