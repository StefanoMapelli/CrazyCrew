using UnityEngine;
using System.Collections;

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


	// Use this for initialization
	void Start () {
		GetPath();
		rigidbody.centerOfMass=new Vector3(0,-0.2f,0);
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

		GetSteer();
		Move ();
		FrontSensor();
		RetroOnCollision();
		WheelRotate();

		currentSpeed= 2*22/7*wheelFR.radius*wheelFR.rpm*60/1000;
		currentSpeed=Mathf.Round(currentSpeed);
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
	
	/// <summary>
	/// Sterzo in base agli ostacoli rilevati dai sensori
	/// </summary>
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
						if(Random.Range(-1,1)>=0)
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
		if(c.collider.name.Equals("Missile(Clone)"))
		{
			Debug.Log("Missile mi ha colpito ");
			StartCoroutine(MissileEffect());
		}
	}


	/// <summary>
	/// verifica le info sulle collisioni e ignora gli ostacoli che non devono essere evitati
	/// </summary>
	/// <returns><c>true</c>, if collision was ignoreded, <c>false</c> otherwise.</returns>
	/// <param name="info">Info.</param>
	bool ignoredCollision(RaycastHit info)
	{
		if(!info.collider.name.Equals("CheckPoint")&& !info.collider.name.Equals("PowerUpObject") && !info.collider.name.Equals("FinishLine"))
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
		wheelTraction.brakeTorque=2000;
		wheelTraction.motorTorque=0;
		yield return new WaitForSeconds(1);
		rigidbody.AddForce(new Vector3(0,10,0));
		wheelTraction.brakeTorque=0;
	}
	

}
