using UnityEngine;
using System.Collections;

public class ServerBogieCarGame : MonoBehaviour {

	private int numberOfPlayers = 3;
	private int counterConnection = 0;
	private int counterReady = 0;
	private bool playing = false;

	private ArrayList players = new ArrayList();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(counterReady==numberOfPlayers && !playing)
		{
			//inizia partita
			Debug.Log("Inizia la partita");
			networkView.RPC("startGame",RPCMode.All);
			playing=true;
		}
	}
	public void PlayerConnection(NetworkPlayer p)
	{
		if(players.Count < numberOfPlayers)
		{
			players.Add(p);
			counterConnection++;
			switch(counterConnection)
			{
			case 1:
				networkView.RPC("assignLever1",p);
				Debug.Log("Leva 1 assegnata");
				break;
			case 2:
				networkView.RPC("assignLever2",p);
				Debug.Log("Leva 2 assegnata");
				break;
			case 3:
				networkView.RPC("assignWheel",p);
				Debug.Log("Sterzo assegnato");
				break;
			}
		}
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
	void setReady()
	{
		counterReady++;
		Debug.Log("counterReady: "+counterReady);
	}
	
	[RPC]
	void startGame()
	{
	}
}
