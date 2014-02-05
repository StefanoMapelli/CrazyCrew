using UnityEngine;
using System.Collections;

public class SteerController : MonoBehaviour {
	
	private static float steerRotation = 0f;
	private float i = 0f;
	private float rate = 1f/5f;
	private new NetworkView networkView;

	public Material[] materials = new Material[2];

	// Use this for initialization
	void Start () {
		networkView = (NetworkView) GameObject.Find ("Client").GetComponent("NetworkView");
	}

	void OnMouseDown() {
		i = 0f;
		this.GetComponent("MeshRenderer").renderer.material = materials[1];
	}
	
	void OnMouseDrag() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast (ray, out hit, 100f)) {
			if (hit.collider.gameObject.name == "LeftSteer") {
				i += Time.deltaTime*rate;
				steerRotation = Mathf.Lerp (steerRotation,-1f,i);
				networkView.RPC("rotateSteer", RPCMode.Server, steerRotation);
			}
			else if (hit.collider.gameObject.name == "RightSteer") {
				i += Time.deltaTime*rate;
				steerRotation = Mathf.Lerp (steerRotation,1f,i);
				networkView.RPC("rotateSteer", RPCMode.Server, steerRotation);
			}
		}
	}
	
	void OnMouseUp() {
		steerRotation = 0f;
		networkView.RPC("rotateSteer", RPCMode.Server, steerRotation);
		this.GetComponent("MeshRenderer").renderer.material = materials[0];
	}
}
