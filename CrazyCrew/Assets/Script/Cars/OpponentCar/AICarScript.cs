using UnityEngine;
using System.Collections;
using System;

public class AICarScript : MonoBehaviour {

	private ArrayList path;
	private GameObject pathGroup;
	public string pathName="";
	public float maxSteer=15.0f;

	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelTraction;
	public WheelCollider wheelRR;
	public WheelCollider wheelRL;

	public Transform WheelFLTransform;
	public Transform WheelFRTransform;
	public Transform WheelRLTransform;
	public Transform WheelRRTransform;

	public int currentPathObject=0; 
	public float distFromPath=5;
	public float maxTorque=50;
	public float maxSpeed=70;
	public float currentSpeed;
	public Ray frontSensor;
	public Ray frontLeftSensor;
	public Ray frontRightSensor;
	public float sensorRay=10;
	public int blocked=0;
	public bool retroRequest=false;
	public int numberOfRetroRequest=0;
	public bool raceStarted=false;
	public bool raceFinished=false;
	public bool checkpointPassed=false;
	public float poopFactor=0;

	public TimeSpan finalTime=new TimeSpan(0,0,0);


	// Use this for initialization
	void Start () {
		GetPath();
		rigidbody.centerOfMass=new Vector3(0,-0.2f,0);
		StartCoroutine(SetMaxSpeedOnPath());
	}

	void GetPath()
	{
		pathGroup=GameObject.Find(pathName);
		Transform[] pathObjs=pathGroup.GetComponentsInChildren<Transform>();
		path= new ArrayList();
		
		
		foreach (Transform pathObj in pathObjs)
		{
			if(pathObj!=pathGroup)
				path.Add(pathObj);
		}

		Debug.Log(path.Count);
	}

	public void WheelRotate()
	{
		
		WheelFRTransform.Rotate(0,-wheelFR.rpm/60*360*Time.deltaTime,0);
		WheelRLTransform.Rotate(0,-wheelRL.rpm/60*360*Time.deltaTime,0);
		WheelRRTransform.Rotate(0,-wheelRR.rpm/60*360*Time.deltaTime,0);
		WheelFLTransform.Rotate(0,-wheelRR.rpm/60*360*Time.deltaTime,0);
		WheelFLTransform.localEulerAngles=new Vector3(WheelFLTransform.localEulerAngles.x,wheelFL.steerAngle-WheelFLTransform.localEulerAngles.z+90,WheelFLTransform.localEulerAngles.z);
		WheelFRTransform.localEulerAngles=new Vector3(WheelFLTransform.localEulerAngles.x,wheelFR.steerAngle-WheelFLTransform.localEulerAngles.z+90,WheelFLTransform.localEulerAngles.z);
		
	}

	// Update is called once per frame
	void FixedUpdate () {

		if(raceStarted)
		{
			GetSteer();
			Move ();
			FrontSensor();
			RetroOnCollision();
			WheelRotate();

			currentSpeed= 2*22/7*wheelFR.radius*wheelFR.rpm*60/1000;
			currentSpeed=Mathf.Round(currentSpeed);
		}
	}

	/// <summary>
	/// Sterzo secondo il percorso a punti
	/// </summary>
	void GetSteer()
	{

		if(!retroRequest)
		{
			Vector3 steerVector = transform.InverseTransformPoint(new Vector3(((Transform)path[currentPathObject]).position.x,transform.position.y,((Transform)path[currentPathObject]).position.z));
			float newSteer = maxSteer * (-steerVector.z/steerVector.magnitude);

			wheelFL.steerAngle=newSteer+poopFactor;
			wheelFR.steerAngle=newSteer+poopFactor;

			if(steerVector.magnitude<=distFromPath)
			{
				currentPathObject++;
			}
			if(currentPathObject>=path.Count)
			{
				currentPathObject=0;
			}
		}
		else
		{
			wheelFL.steerAngle=0;
			wheelFR.steerAngle=0;
		}


	}


	/// <summary>
	/// Movimento costante in avanti oppure retro nel caso di collisione continua
	/// </summary>
	void Move()
	{
		if(!raceFinished)
		{
			if(!retroRequest)
			{
				if(currentSpeed<=maxSpeed)
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
	}
	
	/// <summary>
	/// Sterzo in base agli ostacoli rilevati dai sensori
	/// </summary>
	void FrontSensor()
	{
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
		if(!retroRequest)
		{
			if(raycastF && ignoredCollision(infoF))
			{
				incrementBlocked(infoF);
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
						if(UnityEngine.Random.Range(-1,1)>=0)
						{
							wheelFL.steerAngle=-maxSteer;
							wheelFR.steerAngle=-maxSteer;
						}
						else
						{
							//wheelFL.steerAngle=maxSteer;
							//wheelFR.steerAngle=maxSteer;
						}
					}
				}
			}
			//ostacolo rilevato solo dai sensori obliqui
			else
			{
				if(raycastFL && ignoredCollision(infoFL))
				{
					if(raycastFR && ignoredCollision(infoFR))
					{
						if(infoFL.distance>infoFR.distance)
						{
							//curvo a sinistra ostacolo più vicino a destra
							incrementBlocked(infoFL);
							incrementBlocked(infoFR);
							wheelFL.steerAngle=-maxSteer;
							wheelFR.steerAngle=-maxSteer;
						}
						if(infoFL.distance<infoFR.distance)
						{
							//curvo a destra ostacolo più vicino a sinistra
							incrementBlocked(infoFL);
							incrementBlocked(infoFR);
							wheelFL.steerAngle=maxSteer;
							wheelFR.steerAngle=maxSteer;
						}
					}
					else
					{
						//curvo a destra ostacolo a sinistra
						incrementBlocked(infoFL);
						wheelFL.steerAngle=maxSteer;
						wheelFR.steerAngle=maxSteer;
					}
				}
				else
				{
					if(raycastFR && ignoredCollision(infoFR))
					{
						//curvo a sinsitra ostacolo a destra
						incrementBlocked(infoFR);
						wheelFL.steerAngle=-maxSteer;
						wheelFR.steerAngle=-maxSteer;
					}
				}
			}
		}
	}

	IEnumerator SetMaxSpeedOnPath()
	{
		while(!raceFinished)
		{
			if(isPointOnCurve(currentPathObject))
			{
				maxSpeed=UnityEngine.Random.Range(100,130);
				yield return new WaitForSeconds(2);
			}
			else
			{
				maxSpeed=UnityEngine.Random.Range(130,160);
				yield return new WaitForSeconds(2);
			}
		}
	}

	bool isPointOnCurve(int i)
	{
		if(((i>=17) && (i<=29)) || ((i>=33) && (i<=45)) || ((i>=50) && (i<=67)) || ((i>=79) && (i<=81)) || ((i>=98) && (i<=107)) || ((i>=114) && (i<=127)) || ((i>=138) && (i<=141)) || ((i>=144) && (i<=155)) || ((i>=158) && (i<=176)) || ((i>=187) && (i<=200)) || ((i>=204) && (i<=207)) || ((i>=219) && (i<=230)) || ((i>=242) && (i<=249)) || ((i>=256) && (i<=259)) || ((i>=268) && (i<=276)) || ((i>=290) && (i<=308)))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	IEnumerator setRetro()
	{
		retroRequest=true;
		blocked=0;
		yield return new WaitForSeconds(4);
		retroRequest=false;

	}

	void RetroOnCollision()
	{
		if(blocked>150 && currentSpeed<20)
		{
			if(numberOfRetroRequest<3)
			{
				StartCoroutine (setRetro());
				numberOfRetroRequest++;
			}
			else
			{
				RepositioningAICar();
			}
		}
	}

	void RepositioningAICar()
	{
		numberOfRetroRequest=0;
		blocked=0;
		transform.position=((Transform) path[currentPathObject]).position;
	}

	void OnCollisionEnter(Collision c)
	{
		if(c.collider.name.Equals("RocketCollider"))
		{
			StartCoroutine(MissileEffect());
		}

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "FinishLine")
		{
			raceFinished=true;
			wheelTraction.motorTorque=0;
			wheelTraction.brakeTorque=50;
			finalTime=((RaceManager)GameObject.Find("RaceManager").GetComponent ("RaceManager")).getFinalTime();
			Debug.Log(finalTime);
		}
		if(other.name=="Poop")
		{
			StartCoroutine(PoopEffect());
		}
	}

	IEnumerator PoopEffect()
	{
		if(UnityEngine.Random.Range(-1,1)>0)
		{
			poopFactor=10;
		}
		else
		{
			poopFactor=-10;
		}
		yield return new WaitForSeconds(5);
		poopFactor=0;
	}


	/// <summary>
	/// verifica le info sulle collisioni e ignora gli ostacoli che non devono essere evitati
	/// </summary>
	/// <returns><c>true</c>, if collision was ignoreded, <c>false</c> otherwise.</returns>
	/// <param name="info">Info.</param>
	bool ignoredCollision(RaycastHit info)
	{
		if(!info.collider.tag.Equals("CheckPoint")&& 
		   !info.collider.name.Equals("PowerUpObject") && 
		   !info.collider.name.Equals("StartLine")&& 
		   !info.collider.name.Equals("Poop(Clone)") && 
		   !info.collider.name.Equals("FinishLine"))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void incrementBlocked(RaycastHit info)
	{
		if(!info.collider.tag.Equals("Car"))
		{
			blocked++;
		}
	}

	IEnumerator MissileEffect()
	{
		UnityEngine.Object explosion=Resources.Load("Prefab/Explosion");
		GameObject explosionObject= GameObject.Instantiate(explosion, gameObject.transform.position,gameObject.transform.rotation) as GameObject;
		wheelTraction.brakeTorque=2000;
		wheelTraction.motorTorque=0;
		yield return new WaitForSeconds(1);
		Destroy(explosionObject);
		rigidbody.AddForce(new Vector3(0,10,0));
		wheelTraction.brakeTorque=0;
	}

	public void setRaceStarted(bool isStarted)
	{
		raceStarted=isStarted;
	}
	

}
