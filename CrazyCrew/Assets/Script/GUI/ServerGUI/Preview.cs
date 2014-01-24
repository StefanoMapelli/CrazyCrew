using UnityEngine;
using System.Collections;

public class Preview : MonoBehaviour {

	private GameObject server;
	public AudioSource buttonSound;
	public Material[] materials = new Material[3];

	// Use this for initialization
	void Start () {
		server = GameObject.Find("Server");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		ServerNetworkManager serverNetworkManager = (ServerNetworkManager) server.GetComponent("ServerNetworkManager");
		serverNetworkManager.StartServer();

		Camera.main.transform.Translate(new Vector3(940f,58f,0f));
	}

	public void setMaterial(int m) {
		this.GetComponent("MeshRenderer").renderer.material = materials[m];
	}

	void OnMouseUp()
	{
		buttonSound.Play ();
	}
}
