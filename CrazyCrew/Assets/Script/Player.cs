using UnityEngine;
using System.Collections;

public class Player
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
