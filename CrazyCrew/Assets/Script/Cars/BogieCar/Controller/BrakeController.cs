using UnityEngine;
using System.Collections;

public class BrakeController : MonoBehaviour {
	
	private NetworkView networkView;
	private string role;
	
	// Use this for initialization
	void Start () {
		GameObject client = GameObject.Find ("Client");
		networkView = (NetworkView) client.GetComponent("NetworkView");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setRole(string role)
	{
		this.role = role;
	}
	
	void OnMouseDown() {
		networkView.RPC("brakeOn",RPCMode.Server, role);
	}
	
	void OnMouseUp() {
		networkView.RPC("brakeOff",RPCMode.Server, role);
	}
}