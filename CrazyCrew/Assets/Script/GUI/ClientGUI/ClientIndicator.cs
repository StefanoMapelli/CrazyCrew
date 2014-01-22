using UnityEngine;
using System.Collections;

public class ClientIndicator : MonoBehaviour {

	public Material[] materials = new Material[2];

	public void setConnected(bool connected) {
		if (connected) 
			this.GetComponent("MeshRenderer").renderer.material = materials[0];
		else 
			this.GetComponent("MeshRenderer").renderer.material = materials[1];
	}
}
