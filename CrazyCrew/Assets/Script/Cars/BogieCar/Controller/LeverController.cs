using UnityEngine;
using System.Collections;


public class LeverController : MonoBehaviour {
	
	private Vector3 screenPoint;
	private Vector3 offset;
	private NetworkView networkView;
	private string role;	
	private bool blocked;
	private bool malusBlocked = false;
	private TextMesh text;

	private float sup;
	private float inf;
	private float len;
	
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

	public void setMalusBlocked(bool blocked)
	{
		this.malusBlocked = blocked;
		if(blocked)
		{
			text.text = "Lever failure!!";
			text.color = Color.yellow;
		}
	}

	public bool getMalusBlocked()
	{
		return this.malusBlocked;
	}

	public void setBorder() {
		GameObject leverPlane = GameObject.Find ("LeverPlane");

		sup = leverPlane.transform.position.y + 
			(((MeshRenderer)leverPlane.GetComponent("MeshRenderer")).bounds.size.y)/2f;
		inf = leverPlane.transform.position.y - 
			(((MeshRenderer)leverPlane.GetComponent("MeshRenderer")).bounds.size.y)/2f;	
		len = ((MeshRenderer)leverPlane.GetComponent("MeshRenderer")).bounds.size.y;
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
			if (transform.position.y > sup)
				transform.position = new Vector3(transform.position.x,sup,transform.position.z);
			if (transform.position.y < inf)
				transform.position = new Vector3(transform.position.x,inf,transform.position.z);
		}
	}
	
	void OnMouseUp() {
		if(!blocked) {
			float force;

			force = (sup-(transform.position.y))/len;//chiedi a Muzza
			networkView.RPC ("pushLever",RPCMode.Server,role,force);
			setBlocked(true);
		}
	}
	
	public void setBlocked(bool block) {
		if (text == null)
			text =  (TextMesh) (GameObject.Find("LeverInfo").GetComponent("TextMesh"));
		if (!block) {
			transform.position = new Vector3(transform.position.x,sup,transform.position.z);
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
	
	public bool getBlocked()
	{
		return blocked;
	}
}
