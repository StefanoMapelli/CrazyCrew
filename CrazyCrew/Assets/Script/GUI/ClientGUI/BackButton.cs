using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {

	private ServerList serverList;
	public AudioSource shiftSound;

	public Material[] materials = new Material[2];

	// Use this for initialization
	void Start () {
		serverList = (ServerList) GameObject.Find ("ServerList").GetComponent("ServerList");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown() {
		this.GetComponent("MeshRenderer").renderer.material = materials[1];
		serverList.decrementIndex();
	}

	void OnMouseUp()
	{
		this.GetComponent("MeshRenderer").renderer.material = materials[0];
		shiftSound.Play();
	}
}
