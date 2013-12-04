using UnityEngine;
using System.Collections;

public class BrakeController : MonoBehaviour {
	
	private NetworkView networkView;
	private string role;
	private TextMesh text;
	
	// Use this for initialization
	void Start () {
		GameObject client = GameObject.Find ("Client");
		networkView = (NetworkView) client.GetComponent("NetworkView");
		role = ((ClientBogieCar) client.GetComponent ("ClientBogieCar")).getRole();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown() {
		if (text == null)
			text =  (TextMesh) (GameObject.Find("BrakeLabel").GetComponent("TextMesh"));
		networkView.RPC("brakeOn",RPCMode.Server, role);
		text.text = "Braking...";
	}
	
	void OnMouseUp() {
		networkView.RPC("brakeOff",RPCMode.Server, role);
		text.text = "Press to brake...";
	}
}