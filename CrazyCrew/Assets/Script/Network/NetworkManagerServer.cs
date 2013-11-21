using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class NetworkManagerServer : MonoBehaviour {
	
	public int numberOfPlayers = 3;
	public GameObject controllerPrefab;
	
	private int playerCount = 0;
	// Use this for initialization
	void Start () {
		Network.InitializeServer(numberOfPlayers,25001,false);
		StartCoroutine("sendMessage");
	}
	
	void OnServerInitialized()
	{
		Network.Instantiate(controllerPrefab,Vector3.zero,Quaternion.identity,0);
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		playerCount++;
		
		if (playerCount == numberOfPlayers) {
			
		
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	IEnumerator sendMessage() { 
		Debug.Log ("started");
		while (playerCount < numberOfPlayers) {
			Debug.Log ("sending new message to clients...");
    		Socket sock = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp); 
    		IPEndPoint iep1 = new IPEndPoint(IPAddress.Broadcast, 9050); 
    		byte[]   data = Encoding.ASCII.GetBytes("CrazyCrewServer: HELLO");    
        	sock.SetSocketOption(SocketOptionLevel.Socket, 
            	SocketOptionName.Broadcast, 1);  
    		sock.SendTo(data, iep1);
			yield return new WaitForSeconds(2f);
		}
	}
	
	void OnGUI() {
		GUI.Label(new Rect(10,10,100,100),"Number of players connected: "+playerCount);
	}
}
