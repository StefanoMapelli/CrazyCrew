using UnityEngine;
using System.Collections;


public class LeverController : MonoBehaviour {
	
	private Vector3 screenPoint;
	private Vector3 offset;
	private NetworkView networkView;
	private string role;	
	private bool blocked;
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
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
			new Vector3(screenPoint.x, Input.mousePosition.y, screenPoint.z));
	}
	
	void OnMouseDrag() {
		if (!blocked) {
			Vector3 curScreenPoint = new Vector3(screenPoint.x, Input.mousePosition.y, screenPoint.z);
			
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = curPosition;
			if (transform.position.y > 6.5)
				transform.position = new Vector3(transform.position.x,6.5f,transform.position.z);
			if (transform.position.y < -4.5)
				transform.position = new Vector3(transform.position.x,-4.5f,transform.position.z);
		}
	}
	
	void OnMouseUp() {
		if(!blocked) {
			float force;
			
			force = 1.0f-((transform.position.y+4.5f)/11.0f); //chiedi a Dario
			networkView.RPC ("pushLever",RPCMode.Server,role,force);
			setBlocked(true);
		}
	}
	
	public void setBlocked(bool block) {
		if (text == null)
			text =  (TextMesh) (GameObject.Find("LeverInfo").GetComponent("TextMesh"));
		if (!block) {
			transform.position = new Vector3(transform.position.x,6.5f,transform.position.z);
			text.text = "Pull the lever!";
			text.color = Color.green;
			blocked = false;
		}
		else {
			blocked = true;
			text.text = "Wait...";
			text.color = Color.red;
		}
	}
}
