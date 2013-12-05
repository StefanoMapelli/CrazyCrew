using UnityEngine;
using System.Collections;

public class SteerController : MonoBehaviour {

	private NetworkView networkView;
	private float steerRotation;

	public float steerSensitivity = 15f;

	// Use this for initialization
	void Start () 
	{
		GameObject client = GameObject.Find ("Client");
		networkView = (NetworkView)client.GetComponent("NetworkView");
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log(gameObject.transform.rotation.eulerAngles.z);

		if(gameObject.transform.rotation.eulerAngles.z <= 90 && gameObject.transform.rotation.eulerAngles.z!=0)
		{
			steerRotation = -gameObject.transform.rotation.eulerAngles.z/90;
		}
		else
			if(gameObject.transform.rotation.eulerAngles.z >= 270 && gameObject.transform.rotation.eulerAngles.z!=0)
		{
			steerRotation = 1 - ((gameObject.transform.rotation.eulerAngles.z - 270)/90);
		}
		else
			if(gameObject.transform.rotation.eulerAngles.z ==0)
				steerRotation=0;

		networkView.RPC("rotateSteer", RPCMode.Server, steerRotation);
		Debug.Log("Inviato :"+steerRotation);
	}

	void OnMouseDrag()
	{

		gameObject.transform.Rotate (0f,0f,-(Input.GetAxis ("Mouse X")*steerSensitivity));

		if(gameObject.transform.rotation.eulerAngles.z <= 270 && gameObject.transform.rotation.eulerAngles.z >= 180)
		{
			gameObject.transform.rotation = Quaternion.Euler(0,0,-90);
		}
		else
			if(gameObject.transform.rotation.eulerAngles.z >= 90 && gameObject.transform.rotation.eulerAngles.z <= 180)
			{
				gameObject.transform.rotation = Quaternion.Euler(0,0,90);
			}
	}
}