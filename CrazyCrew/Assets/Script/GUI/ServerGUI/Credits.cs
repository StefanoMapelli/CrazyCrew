using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	private CreditAnimation creditAnimation;

	void Start() {
		creditAnimation = (CreditAnimation) GameObject.Find ("Credits").GetComponent("CreditAnimation");
	}

	void OnMouseDown() {
		Camera.main.transform.position = new Vector3(-850f,0f,-20f);
		creditAnimation.setStart(true);
	}
}
