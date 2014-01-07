using UnityEngine;
using System.Collections;

public class AICarScript : MonoBehaviour {

	private ArrayList path;
	private GameObject pathGroup;
	public float maxSteer=15.0f;
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelTraction;
	public int currentPathObject=0; 
	public float distFromPath=5;
	public float maxTorque=50;
	public float currentSpeed;
	public Ray frontSensor;
	public Ray frontLeftSensor;
	public Ray frontRightSensor;
	private bool obstacle=false;
	public float sensorRay=10;
	public int blocked=0;
	public bool retroRequest=false;


	// Use this for initialization
	void Start () {
		GetPath();
		rigidbody.centerOfMass=new Vector3(0,-0.2f,0);
	}

	void GetPath()
	{
		pathGroup=GameObject.Find("Path");
		Transform[] pathObjs=pathGroup.GetComponentsInChildren<Transform>();
		path= new ArrayList();
		
		
		foreach (Transform pathObj in pathObjs)
		{
			if(pathObj!=pathGroup)
				path.Add(pathObj);
		}

		Debug.Log(path.Count);
	}

	// Update is called once per frame
	void FixedUpdate () {

		GetSteer();
		Move ();
		FrontSensor();
		RetroOnCollision();

		currentSpeed= 2*22/7*wheelFR.radius*wheelFR.rpm*60/1000;
		currentSpeed=Mathf.Round(currentSpeed);
	}

	void GetSteer()
	{
		Vector3 steerVector = transform.InverseTransformPoint(new Vector3(((Transform)path[currentPathObject]).position.x,transform.position.y,((Transform)path[currentPathObject]).position.z));
		float newSteer = maxSteer * (-steerVector.z/steerVector.magnitude);

		wheelFL.steerAngle=newSteer;
		wheelFR.steerAngle=newSteer;

		if(steerVector.magnitude<=distFromPath)
		{
			currentPathObject++;
		}
		if(currentPathObject>=path.Count)
		{
			currentPathObject=0;
		}

	}

	void Move()
	{
		if(!retroRequest)
		{
			if(currentSpeed<=70)
				wheelTraction.motorTorque=maxTorque;
			else
				wheelTraction.motorTorque=0;
		}
		else
		{
			Debug.Log("Vai indietro");
			wheelTraction.motorTorque=-20;
		}
	}
	

	void FrontSensor()
	{
		float newSteer=0;
		RaycastHit infoF=new RaycastHit();
		RaycastHit infoFR=new RaycastHit();
		RaycastHit infoFL=new RaycastHit();
		//imposto i raggi dei sensori
		frontSensor=new Ray(new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z),transform.right);
		frontLeftSensor=new Ray(new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z),transform.forward*0.5f+transform.right);
		frontRightSensor=new Ray(new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z),transform.forward*-0.5f+transform.right);
		//disegno i raggi sensori

		Debug.DrawRay(new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z), (transform.forward*-0.5f+transform.right)*sensorRay, Color.red);
		Debug.DrawRay(new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z), (transform.forward*0.5f+transform.right)*sensorRay, Color.red);
		Debug.DrawRay(new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z), (transform.right)*sensorRay, Color.red);

		bool raycastF=Physics.Raycast(frontSensor, out infoF, sensorRay);
		bool raycastFL=Physics.Raycast(frontLeftSensor, out infoFL, sensorRay);
		bool raycastFR=Physics.Raycast(frontRightSensor,out infoFR, sensorRay);

		//ostacolo rilevato dal sensore di fronte

		if(raycastF && (!infoF.collider.name.Equals("CheckPoint") && !infoF.collider.name.Equals("PowerUp") && !infoF.collider.name.Equals("FinishLine")))
		{
			blocked++;
			if(raycastFL)
			{
				if(raycastFR)
				{
					if(infoFL.distance>infoFR.distance)
					{
						//curvo a sinistra ostacolo di fronte più vicino a destra
						wheelFL.steerAngle=-maxSteer;
						wheelFR.steerAngle=-maxSteer;
					}
					if(infoFL.distance<infoFR.distance)
					{
						//curvo a destra ostacolo fronte più vicino a sinistra
						wheelFL.steerAngle=maxSteer;
						wheelFR.steerAngle=maxSteer;
					}
				}
				else
				{
					//curvo a destra ostacolo centro-sinistra
					wheelFL.steerAngle=maxSteer;
					wheelFR.steerAngle=maxSteer;
				}
			}
			else
			{
				if(raycastFR)
				{
					//curvo a sinistra ostacolo centro-destra
					wheelFL.steerAngle=-maxSteer;
					wheelFR.steerAngle=-maxSteer;
				}
				else
				{
					//curvo random a destra o a sinistra perchè l'ostacolo è esattamente di fronte a noi
					if(Random.Range(-1,1)>=0)
					{
						wheelFL.steerAngle=-maxSteer;
						wheelFR.steerAngle=-maxSteer;
					}
					else
					{
						wheelFL.steerAngle=maxSteer;
						wheelFR.steerAngle=maxSteer;
					}
				}
			}
		}
		//ostacolo rilevato solo dai sensori obliqui
		else
		{
			if(raycastFL && (!infoFL.collider.name.Equals("CheckPoint") && !infoFL.collider.name.Equals("PowerUp") && !infoFL.collider.name.Equals("FinishLine")))
			{
				if(raycastFR&& (!infoFR.collider.name.Equals("CheckPoint") && !infoFR.collider.name.Equals("PowerUp") && !infoFR.collider.name.Equals("FinishLine")))
				{
					if(infoFL.distance>infoFR.distance)
					{
						//curvo a sinistra ostacolo più vicino a destra
						blocked++;
						wheelFL.steerAngle=-maxSteer;
						wheelFR.steerAngle=-maxSteer;
					}
					if(infoFL.distance<infoFR.distance)
					{
						//curvo a destra ostacolo più vicino a sinistra
						blocked++;
						wheelFL.steerAngle=maxSteer;
						wheelFR.steerAngle=maxSteer;
					}
				}
				else
				{
					//curvo a destra ostacolo a sinistra
					blocked++;
					wheelFL.steerAngle=maxSteer;
					wheelFR.steerAngle=maxSteer;
				}
			}
			else
			{
				if(raycastFR && (!infoFR.collider.name.Equals("CheckPoint")&& !infoFR.collider.name.Equals("PowerUp") && !infoFR.collider.name.Equals("FinishLine")))
				{
					//curvo a sinsitra ostacolo a destra
					blocked++;
					wheelFL.steerAngle=-maxSteer;
					wheelFR.steerAngle=-maxSteer;
				}
			}
		}
	}

	IEnumerator RetroTorque()
	{
		retroRequest=true;
		yield return new WaitForSeconds(4);
		retroRequest=false;
		blocked=0;
	}

	void RetroOnCollision()
	{
		if(blocked>100)
		{
			StartCoroutine (RetroTorque());
		}
	}

	void OnCollisionEnter(Collision c)
	{
		Debug.Log("Oggetto colpito");
	}
}
