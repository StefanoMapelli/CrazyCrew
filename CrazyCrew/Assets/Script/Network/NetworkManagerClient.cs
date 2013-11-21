using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class NetworkManagerClient : MonoBehaviour {
	
	private UdpClient udpClient;
	private int serverPort;
	private string serverIpAddress;
	private bool messageReceived=false;

	// Use this for initialization
	void Start () {
		
		udpClient = new UdpClient(9051);
		System.Threading.Thread connection_thread = new System.Threading.Thread(WaitForServer);
		connection_thread.Start ();
	}
	
	void WaitForServer()
	{
		//IPEndPoint object will allow us to read datagrams sent from any source on port 9050.
		IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 9051);
		
		// Receive message from server and control if it's the CrazyCrew message.
		while(!messageReceived)
		{
			Debug.Log("attesa dati:");
			Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint); 
			string returnData = Encoding.ASCII.GetString(receiveBytes);
			Debug.Log("dati:"+returnData);
			if(returnData.CompareTo("CrazyCrewServer: HELLO")==0)
			{
				messageReceived=true;
			}
		}
		
		serverIpAddress=RemoteIpEndPoint.Address.ToString();
		serverPort= RemoteIpEndPoint.Port;
		
		Debug.Log(serverIpAddress + serverPort);
		
		udpClient.Close();
	}
	
	void OnGUI()
	{
		if(messageReceived)
		{
			if(GUI.Button(new Rect(100,100,100,100),"Connect To server"))
			{
				Network.Connect(serverIpAddress,25001);
				
			}
		}
		else
			GUI.Label(new Rect(200,100,100,100),"not connected");
		
		if(Network.isClient){
			GUI.Label(new Rect(100,200,100,100),"connect to server with network");
			
			if(GUI.Button(new Rect(200,200,100,100),"Brake")){
				//Ho premuto il freno dx
				
				GameObject controllerBogie = GameObject.Find ("ControllerBogieCar");
				Debug.Log(controllerBogie.ToString());
				//controllerBogie.networkView.RPC ("pressRightBrake",RPCMode.Server,null);
			}
		}
			
	}
	
	void OnConnectedToServer()
	{
		//Devo ricevere il messaggio di quale controller visualizzare
		//Application.LoadLevel ("BogieRight");
	}
	
	void OnDisconnectedFromServer()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
