using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {

	private ClientGameManager clientGameManager;
	public AudioSource buttonSound;
	public Material[] materials = new Material[2];

	// Use this for initialization
	void Start () {

		GameObject client=GameObject.Find("Client");
		clientGameManager=(ClientGameManager)client.GetComponent("ClientGameManager");
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown()
	{
		clientGameManager.pauseOn();

		this.GetComponent("MeshRenderer").renderer.material = materials[1];
	}

	void OnMouseUp()
	{
		buttonSound.Play();
		this.GetComponent("MeshRenderer").renderer.material = materials[0];
	}
}
