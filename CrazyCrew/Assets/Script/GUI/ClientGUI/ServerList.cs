using UnityEngine;
using System.Collections;

public class ServerList : MonoBehaviour {

	private HostData[] list;
	private int index = 0;
	private ClientNetworkManager clientNetworkManager;

	// Use this for initialization
	void Start () {
		clientNetworkManager = (ClientNetworkManager)GameObject.Find ("Client").GetComponent("ClientNetworkManager");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		if (list.Length > 0)
		{	
			((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;
			clientNetworkManager.JoinServer(list[index]);
		}
	}

	void OnMouseUp()
	{
		if (list.Length > 0)
		{	
			((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
			gameObject.audio.Play();
		}
	}

	public void setList(HostData[] list) {
		this.list = list;
		index = 0;
		if (list.Length > 0)
			((TextMesh)gameObject.GetComponent("TextMesh")).text = list[index].gameName;
		else
			((TextMesh)gameObject.GetComponent("TextMesh")).text = "No server";
	}

	public void incrementIdex() {
		if (list.Length > 0) {
			if (index == list.Length-1) 
				index = 0;
			else 
				index++;
			((TextMesh)gameObject.GetComponent("TextMesh")).text = list[index].gameName;
		}
		else 
			((TextMesh)gameObject.GetComponent("TextMesh")).text = "No server";
	}

	public void decrementIndex() {
		if (list.Length > 0) {
			if (index > 0)
				index--;
			else 
				index = list.Length - 1;
			((TextMesh)gameObject.GetComponent("TextMesh")).text = list[index].gameName;
		}
		else
		((TextMesh)gameObject.GetComponent("TextMesh")).text = "No server";
	}

}
