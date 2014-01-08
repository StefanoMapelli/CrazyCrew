using UnityEngine;
using System.Collections;

public class SteerController : MonoBehaviour {
	
	private static float steerRotation = 0f;
	private float i = 0f;
	private float rate = 1f/5f;
	private NetworkView networkView;

	// Use this for initialization
	void Start () {
		networkView = (NetworkView) GameObject.Find ("Client").GetComponent("NetworkView");
	}

	void OnMouseDown() {
		i = 0f;
	}
	
	void OnMouseDrag() {
		i += Time.deltaTime*rate;
		if (gameObject.name == "LeftSteer") {
			steerRotation = Mathf.Lerp (steerRotation,-1f,i);
		}
		else if (gameObject.name == "RightSteer") {
			steerRotation = Mathf.Lerp (steerRotation,1f,i);
		}
		networkView.RPC("rotateSteer", RPCMode.Server, steerRotation);
	}
	
	void OnMouseUp() {
		steerRotation = 0f;
		networkView.RPC("rotateSteer", RPCMode.Server, steerRotation);
	}
}
