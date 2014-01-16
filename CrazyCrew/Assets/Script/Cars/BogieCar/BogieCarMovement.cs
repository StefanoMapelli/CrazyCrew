using UnityEngine;
using System.Collections;
using System;

public class BogieCarMovement : MonoBehaviour {

	private ServerBogieCar serverBogieCar;
	
	//Conponenti veicolo

	public int timeTorque = 2;

	//sterzo
	float lowestSteerAtSpeed = 50;
	float lowSpeedSteerAngle = 10;
	float highSpeedSteerAngle = 1;

	//ruote
	public WheelCollider WheelFR;
	public WheelCollider WheelFL;
	public WheelCollider WheelRR;
	public WheelCollider WheelRL;
	public WheelCollider WheelTraction;

	public Transform WheelFLTransform;
	public Transform WheelFRTransform;
	public Transform WheelRLTransform;
	public Transform WheelRRTransform;

	public float WheelTractionRPM;

	public float torqueTraction;
	public float brakeTorque;

	public float currentSpeed;

	//freni
	private bool brakeR =false;
	private bool brakeL =false;

	//valori fisici
	public float decelerationSpeed = -50f;
	public float retroSpeed=-10f;
	public float torqueMax=50f;
	public float topRetroSpeed=-10f;
	public float topSpeed=150f;

	//CheckPoint
	private bool checkPoint = false;
	public GameObject raceManager;


	private bool startRace = true;
	private bool finishRace = false;

	//bonus info
	private float speedMultiplierBonus=2;
	private Bonus bonusAcquired;

	//malus info
	private bool malusActive = false;
	private int reductionCounter = 0;
	private int malusDuration;

	private int malusSteerRotation=0;
	private ArrayList mudSpotsList = new ArrayList();

	public TextMesh infoText;

	public TimeSpan finalTime;

	private float poopFactor=0;

	//Posizione dove fare respawn, si aggiorna all'ultimo checkPoint passato.
	private Vector3 lastPosition = new Vector3();
	private Quaternion lastRotation = new Quaternion();

	//Audio
	public AudioSource dirtBrakeSound;

	public AudioSource collision1;
	public AudioSource collision2;
	public AudioSource collision3;
	public AudioSource collision4;
	public AudioSource collision5;
	public AudioSource collision6;

	public AudioSource mudSound;
	public AudioSource turboSound;
	public AudioSource malusSound;
	public AudioSource bonusSound;

	public AudioSource ackSound;
	public AudioSource leverSound;

	private Animation animation;
	private bool lever1 = true;

	// Use this for initialization
	void Start () 
	{
		rigidbody.centerOfMass=new Vector3(0,-0.2f,0);
		serverBogieCar = (ServerBogieCar) GameObject.Find ("Server").GetComponent("ServerBogieCar");

		GameObject Stain1 = GameObject.Find("Stain1");
		GameObject Stain2 = GameObject.Find("Stain2"); 
		GameObject Stain3 = GameObject.Find("Stain3"); 
		GameObject Stain4 = GameObject.Find("Stain4"); 
		mudSpotsList.Add(Stain1);
		mudSpotsList.Add(Stain2);
		mudSpotsList.Add(Stain3);
		mudSpotsList.Add(Stain4);

		lastPosition = this.transform.position;
		lastRotation = this.transform.rotation;

		animation = (Animation) GameObject.Find ("Avanti Finita 003").GetComponent("Animation");
	}

	public Bonus getBonus()
	{
		return bonusAcquired;
	}

	/*public void setSteerMalusDuration()
	{
	}*/

	// Update is called once per frame
	void Update () {
		if(startRace && !finishRace)
			//Ora comincia la partita.
		{
			torqueTraction = WheelTraction.motorTorque;
			brakeTorque=WheelTraction.brakeTorque;
			currentSpeed= 2*22/7*WheelTraction.radius*WheelTraction.rpm*60/1000;
			currentSpeed=Mathf.Round(currentSpeed);

			//controllo ogni volta se entrambi i freni sono premuti ed in tal caso freno il veicolo
		Brake ();

		WheelRotate();
		}

		if(Input.GetKeyDown (KeyCode.R))
		{
			this.transform.position=lastPosition;
			this.transform.rotation = lastRotation;
		}
	}

	public void WheelRotate()
	{
		WheelTractionRPM = WheelTraction.rpm;

		WheelFRTransform.Rotate(0,-WheelFR.rpm/60*360*Time.deltaTime,0);
		WheelRLTransform.Rotate(0,-WheelRL.rpm/60*360*Time.deltaTime,0);
		WheelRRTransform.Rotate(0,-WheelRR.rpm/60*360*Time.deltaTime,0);
		WheelFLTransform.Rotate(0,-WheelRR.rpm/60*360*Time.deltaTime,0);
		WheelFLTransform.localEulerAngles=new Vector3(WheelFLTransform.localEulerAngles.x,WheelFL.steerAngle-WheelFLTransform.localEulerAngles.z+90,WheelFLTransform.localEulerAngles.z);
		WheelFRTransform.localEulerAngles=new Vector3(WheelFLTransform.localEulerAngles.x,WheelFR.steerAngle-WheelFLTransform.localEulerAngles.z+90,WheelFLTransform.localEulerAngles.z);

	}
	
	public void BrakeROn()
	{
		brakeR=true;
	}
	
	public void BrakeROff()
	{
		brakeR=false;
	}
	
	public void BrakeLOn()
	{
		brakeL=true;
	}
	
	public void BrakeLOff()
	{
		brakeL=false;
	}

	/*
    * In caso di freno sinistro e destro premuto aziono il freno del veicolo
    */
	private void Brake()
	{		
		if(brakeL && brakeR) 
		{
			if(currentSpeed>0)
			{
				//controllo sul tipo di terreno
				dirtBrakeSound.Play();

				WheelTraction.motorTorque = 0;
				WheelTraction.brakeTorque = -decelerationSpeed;

			}
			else if(currentSpeed<=0 && currentSpeed>=topRetroSpeed)
			{
				WheelTraction.brakeTorque = 0;
				WheelTraction.motorTorque = retroSpeed;
			}
			else if(currentSpeed<=0 && currentSpeed<=topRetroSpeed)
			{
				WheelTraction.brakeTorque = 0;
				WheelTraction.motorTorque = 0;
			}
		}
		else 
		{
			WheelTraction.brakeTorque = 0;
		}
	}
	
	/// <summary>
	///  Indica quanto hai abbasato la leva sullo Smartphone
	///  rispetto al massimo possibile
	/// </summary>
	/// <param name="forcePercent">Force percent è compreso tra 0 e 1.</param>
	public void LeverDown(float forcePercent)
	{
		leverSound.Play();
		if (lever1) {
			animation.Play("Lever1");
			lever1 = false;
		}
		else {
			animation.Play("Lever2");
			lever1 = true;
		}
	
		if(currentSpeed<topSpeed)
		{
			WheelTraction.motorTorque += forcePercent*torqueMax;
			StartCoroutine (StopTorque(forcePercent*torqueMax));
		}
		else
		{
			WheelTraction.motorTorque = 0;
		}
	}

	IEnumerator StopTorque(float torque)
	{
		yield return new WaitForSeconds(timeTorque);

		if(WheelTraction.motorTorque > 0)
		{
			if(WheelTraction.motorTorque < torque)
			{
				WheelTraction.motorTorque = 0;
			}
			else
			{
				WheelTraction.motorTorque -= torque;
			}
		}

	}

	public void Steer(float steerPercent)
	{
		if (steerPercent < 0) {
			if (!animation.isPlaying)
				animation.Play ("Left");
			else {
				if (animation.IsPlaying("Left")) {
					animation.Play ("Left");
				}
				else {
					animation.Blend ("Right",0f,0.5f);
					animation.Blend ("Left",1f,0.5f);
				}
			}
		}
		else {
			if (!animation.isPlaying)
				animation.Play ("Right");
			else {
				if (animation.IsPlaying("Right")) {
					animation.Play ("Right");
				}
				else {
					animation.Blend ("Left",0f,0.5f);
					animation.Blend ("Right",1f,0.5f);
				}
			}
		}

		float speedFactor = rigidbody.velocity.magnitude/lowestSteerAtSpeed;
		float currentSteerAngle = Mathf.Lerp(lowSpeedSteerAngle,highSpeedSteerAngle,speedFactor);

		currentSteerAngle = (currentSteerAngle*steerPercent) + malusSteerRotation;

		
		WheelFL.steerAngle = currentSteerAngle+poopFactor;
		WheelFR.steerAngle = currentSteerAngle+poopFactor;
	}
	
	IEnumerator Finish ()
	{
		Debug.Log ("finish");

		if(!finishRace)
		{
			finishRace = true;
			yield return 0;
			rigidbody.isKinematic=true;

			finalTime=((RaceManager)raceManager.GetComponent ("RaceManager")).getFinalTime();
			((RaceManager)raceManager.GetComponent ("RaceManager")).FinishLine ();
		}
	}

	IEnumerator PoopEffect()
	{
		if(UnityEngine.Random.Range(-1,1)>0)
		{
			poopFactor=5;
		}
		else
		{
			poopFactor=-5;
		}
		yield return new WaitForSeconds(5);
		poopFactor=0;
	}

	//Collisione con oggetti in gara
	void OnTriggerEnter(Collider other) 
	{
		if(other.tag=="CheckPoint")
		{
			lastPosition = other.transform.position;
			lastRotation = other.transform.rotation;
		}
		else
		{
			if(other.name=="Poop(Clone)")
			{
				StartCoroutine(PoopEffect());
			}
			else
			{
				if(other.gameObject.name == "BonusObject")
				{
					bonusSound.Play();

					int powerUpId = UnityEngine.Random.Range(1,5);
					//powerUpId=4;
					serverBogieCar.bonusComunication(powerUpId);
					switch(powerUpId)
					{
					case 1:
					{
						bonusAcquired = new BonusMissile();
						break;
					}
				
					case 2:
					{
						bonusAcquired = new BonusPoop();
						break;
					}
					
					case 3:
					{
						StartCoroutine(BonusTime());
						break;
					}
				
					case 4:
					{
						bonusAcquired = new BonusSpeed();
						break;	
					}
					}
				}
				else
				{
					if(other.gameObject.name == "FinishLine")
					{	
						StartCoroutine (Finish());
					}
					else
					{
						if(other.gameObject.name == "MalusObject" && !malusActive)
						{
						malusActive = true;

						int powerUpId = UnityEngine.Random.Range(1,5);

						//powerUpId=3;
						serverBogieCar.malusComunication(powerUpId);
						switch(powerUpId)
						{
						case 1:
						{
							StartCoroutine(MalusMud());
							break;
						}
						
						case 2:
						{
							StartCoroutine(MalusSlowdown());
							break;
						}

						case 3:
						{
							serverBogieCar.assignRoles();
							malusActive = false;
							break;
						}
						
						case 4:
						{
							StartCoroutine(MalusSteerFailure());
							break;	
						}
						}
					}

						}
					}
				}	
			}
		}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("sono in collision enter ramo else, colliso con"+collision.gameObject.name);
		GameObject  obj = collision.gameObject;

		if(currentSpeed <25 || (obj.tag=="Car" && ((AICarScript)obj.GetComponent("AICarScript")).currentSpeed <25))
		{
			collision1.Play();
		}
		//collisione generica
		if((currentSpeed >=25 && currentSpeed < 50) || (obj.tag=="Car" && ((AICarScript)obj.GetComponent("AICarScript")).currentSpeed >= 25 && ((AICarScript)obj.GetComponent("AICarScript")).currentSpeed < 50))
		{
			collision2.Play();
		}
		else 
			{
			if((currentSpeed>=50 && currentSpeed<75) || (obj.tag=="Car" && ((AICarScript)obj.GetComponent("AICarScript")).currentSpeed >= 50 && ((AICarScript)obj.GetComponent("AICarScript")).currentSpeed < 75))
				{
					collision3.Play();
				}
				else
				{
				if((currentSpeed>=75 && currentSpeed<100) || (obj.tag=="Car" && ((AICarScript)obj.GetComponent("AICarScript")).currentSpeed >= 75 && ((AICarScript)obj.GetComponent("AICarScript")).currentSpeed < 100))
					{
						collision4.Play();
					}
					else
					{
					if((currentSpeed>=100 && currentSpeed<125) || (obj.tag=="Car" && ((AICarScript)obj.GetComponent("AICarScript")).currentSpeed >= 100 && ((AICarScript)obj.GetComponent("AICarScript")).currentSpeed < 125))
						{
							collision5.Play();
						}
						else
						{
						if((currentSpeed>=125 && currentSpeed<=150) || (obj.tag=="Car" && ((AICarScript)obj.GetComponent("AICarScript")).currentSpeed >= 125 && ((AICarScript)obj.GetComponent("AICarScript")).currentSpeed < 150))
								{
									collision6.Play();
								}
							}
						}
					}
				}
			}
	
	//GESTIONE POWER-UP
	//BONUS
	
	public void bonusSpeed()
	{
		Debug.Log ("Turbo attivato");
		StartCoroutine(BonusSpeed());
	}
	
	IEnumerator BonusSpeed()
	{
		turboSound.Play();
		infoText.text="TURBOOOOOO";
		WheelTraction.motorTorque += torqueMax*speedMultiplierBonus;
		yield return new WaitForSeconds(0.5f);
		StartCoroutine (StopTorque(torqueMax*speedMultiplierBonus));
		infoText.text="";
	}

	//powerUp: il tempo viene decrementato di un valore tra 5 e 10 secondi
	IEnumerator BonusTime()
	{
		int bonusTime = UnityEngine.Random.Range(5,11);
		infoText.text="BONUS TIME: " +bonusTime+ "sec";
		((RaceManager)raceManager.GetComponent ("RaceManager")).bonusTime(bonusTime);
		yield return new WaitForSeconds(3);
		infoText.text="";
	}


	//MALUS
	public void malusReduction(string malusName)
	{
		reductionCounter++;

		if(reductionCounter % 3 == 0)
		{
			if(malusName != "MUD")
				malusDuration--;

			if(malusName == "SLOWDOWN")
				WheelTraction.motorTorque = torqueMax*0.05f;

			if(malusName == "MUD")
			{
				((MeshRenderer)((GameObject)mudSpotsList[0]).GetComponent("MeshRenderer")).enabled = false;
				mudSpotsList.RemoveAt(0);
			}

			StartCoroutine(MalusReductionNotify());
		}
	}

	IEnumerator MalusReductionNotify()
	{
		ackSound.Play();
		infoText.text=" Malus reduced...GOOD JOB!";
		yield return new WaitForSeconds(2);
		infoText.text="";
	}

	IEnumerator MalusSteerFailure()
	{
		malusSound.Play();

		malusDuration=5;
		malusSteerRotation=6;

		if(UnityEngine.Random.Range(1,2) == 1)
		{
			malusSteerRotation = -malusSteerRotation;
		}
	
		for(int i=0;i<malusDuration;i++)
		{
			infoText.text="STEER FAILURE! Press the button to reduce it!";
			yield return new WaitForSeconds(1);
		}

		serverBogieCar.malusEnded();
		malusDuration=5;
		malusSteerRotation=0;
		infoText.text="";
		malusActive = false;
	}

	IEnumerator MalusSlowdown()
	{
		malusSound.Play();

		malusDuration=5;
		serverBogieCar.slowDown(true);
		WheelTraction.motorTorque = -torqueMax*0.2f;

		for(int i=0;i<malusDuration;i++)
		{
			infoText.text="LEVER FAILURE! Press the button to reduce it!";
			yield return new WaitForSeconds(1);
		}

		serverBogieCar.slowDown(false);
		serverBogieCar.malusEnded();
		malusDuration=5;
		infoText.text="";
		malusActive = false;
	}

	IEnumerator MalusMud()
	{
		mudSound.Play();

		malusDuration=5;

		GameObject Stain1 = GameObject.Find("Stain1");
		GameObject Stain2 = GameObject.Find("Stain2"); 
		GameObject Stain3 = GameObject.Find("Stain3"); 
		GameObject Stain4 = GameObject.Find("Stain4"); 
		((MeshRenderer) Stain1.GetComponent("MeshRenderer")).enabled = true;
		((MeshRenderer) Stain2.GetComponent("MeshRenderer")).enabled = true;
		((MeshRenderer) Stain3.GetComponent("MeshRenderer")).enabled = true;
		((MeshRenderer) Stain4.GetComponent("MeshRenderer")).enabled = true;

		for(int i=0;i<malusDuration;i++)
		{
			infoText.text="MUD ON THE SCREEN! Press the button to clean it!";
			yield return new WaitForSeconds(1);
		}

		//ordine di grandezza crescente
		mudSpotsList.Add(Stain1);
		mudSpotsList.Add(Stain2);
		mudSpotsList.Add(Stain3);
		mudSpotsList.Add(Stain4);
		
		((MeshRenderer) Stain1.GetComponent("MeshRenderer")).enabled = false;
		((MeshRenderer) Stain2.GetComponent("MeshRenderer")).enabled = false;
		((MeshRenderer) Stain3.GetComponent("MeshRenderer")).enabled = false;
		((MeshRenderer) Stain4.GetComponent("MeshRenderer")).enabled = false;

		serverBogieCar.malusEnded();
		malusDuration=5;
		infoText.text="";
		malusActive = false;
	}
}