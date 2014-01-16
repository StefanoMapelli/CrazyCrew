using UnityEngine;
using System.Collections;
using System;

public class CarCamera : MonoBehaviour {

	public Transform car;
	public float distance=3f;
	public float height=1.4f;
	public float rotationDamping=3.0f;
	public float heightDamping= 2.0f;
	public float zoomRacio =0.5f;
	private Vector3 rotationVector;
	public float defaultFOV = 60;
	private bool finishMenu=false;
	
	void Start () {
		//car=GameObject.Find("_BogieCarModel").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if(!finishMenu)
		{/*
			float wantedAngle = rotationVector.y;
			float wantedHeight = car.position.y+height;
			float myAngle=transform.eulerAngles.y;
			float myHeight=transform.position.y;
			
			myAngle = Mathf.LerpAngle(myAngle,wantedAngle,rotationDamping*Time.deltaTime);
			myHeight = Mathf.Lerp(myHeight,wantedHeight,heightDamping*Time.deltaTime);
			var currentRotation = Quaternion.Euler(0,myAngle,0);
			transform.position=car.position;
			transform.position-=currentRotation*Vector3.forward*distance;
			transform.position=new Vector3(transform.position.x,myHeight,transform.position.z);
			transform.LookAt(car);

			*/
			transform.position=new Vector3(car.position.x,car.position.y,car.position.z)-6*car.right + 3*car.up;
			transform.LookAt(new Vector3(car.transform.position.x,car.transform.position.y+2,car.transform.position.z));

		}
	
	}

	void FixedUpdate()
	{
		/*if(!finishMenu)
		{
			Vector3 localVelocity = car.InverseTransformDirection(car.rigidbody.velocity);
			if(localVelocity.x<-0.5f){
				rotationVector.y=car.eulerAngles.y-90;	
			}
			else {
				
				rotationVector.y=car.eulerAngles.y+90;
			}
			
			float acc= car.rigidbody.velocity.magnitude;
			camera.fieldOfView = defaultFOV+acc*zoomRacio;
		}*/
	}

	public void cameraOnFinishMenu()
	{
		finishMenu=true;
		//transform.position=new Vector3(14.4843f,-13.1f,80f);
		//transform.rotation=new Quaternion(0,0,0,0);
		Camera cam = (Camera) GameObject.Find ("Camera1").GetComponent("Camera");
		cam.enabled = true;

		((Camera)this.GetComponent("Camera")).enabled = false;

	}

	public void restartCamera()
	{
		finishMenu=false;
	}
}
