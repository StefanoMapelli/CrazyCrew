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
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if(!finishMenu)
		{
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
		}
	
	}

	void FixedUpdate()
	{
		if(!finishMenu)
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
		}
		
	}

	public void cameraOnFinishMenu()
	{
		finishMenu=true;
		transform.position=new Vector3(13.48057f,-13.1f,112.817f);
		transform.rotation=new Quaternion(0,0,0,0);

	}

	public void restartCamera()
	{
		finishMenu=false;
	}
}
