using UnityEngine;
using System.Collections;

public class CreditAnimation : MonoBehaviour {

	private bool start = false;
	private float vel = 25f;
	private Vector3 startingPosition;

	public void setStart(bool start) {
		this.start = start;
		if (!start)
			transform.position = startingPosition;
	}

	private void playAnimation() {
		transform.position = new Vector3(transform.position.x,transform.position.y+(Time.deltaTime*vel),transform.position.z);
		if (transform.position.y > 450)
			transform.position = startingPosition;
	}

	void Start() {
		startingPosition = transform.position;
	}

	void Update() {
		if (start)
			playAnimation();
	}

	void OnMouseDown() {
		CreditAnimation creditAnimation = (CreditAnimation)GameObject.Find ("Credits").GetComponent("CreditAnimation");

		creditAnimation.setStart(false);
		Camera.main.transform.position = new Vector3(0f,0f,-20f);
	}
}
