using UnityEngine;
using System.Collections;

public class Player
{
	private NetworkPlayer networkPlayer;
	private string role;
	private bool ready=false;
	private bool connected=true;

	public Player(NetworkPlayer np)
	{
		networkPlayer=np;
	}

	public bool getConnected()
	{
		return connected;
	}
	
	public void setConnected(bool connected)
	{
		this.connected=connected;
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

	public void setNetworkPlayer(NetworkPlayer np)
	{
		this.networkPlayer = np;
	}
}
