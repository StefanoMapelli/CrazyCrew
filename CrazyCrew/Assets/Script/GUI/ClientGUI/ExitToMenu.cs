using UnityEngine;
using System.Collections;

public class ExitToMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		Debug.Log("Ho schiacciato");

		GameObject.Destroy(GameObject.Find("Client"));
		Network.Disconnect();
		Application.LoadLevel("client");

	}
}
