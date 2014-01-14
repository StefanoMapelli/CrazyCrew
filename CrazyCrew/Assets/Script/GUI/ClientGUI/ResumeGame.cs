using UnityEngine;
using System.Collections;

public class ResumeGame : MonoBehaviour {

	private GameObject client;
	private ClientGameManager clientGameManager;
	public AudioSource buttonSound;


	// Use this for initialization
	void Start () {

		client=GameObject.Find("Client");
		clientGameManager=(ClientGameManager)client.GetComponent("ClientGameManager");
		}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;
		clientGameManager.resumeGame();
	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
		buttonSound.Play();
	}
}
