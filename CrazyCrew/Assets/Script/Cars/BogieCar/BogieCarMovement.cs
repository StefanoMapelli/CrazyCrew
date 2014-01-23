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
	private float retroSpeed=-40f;
	public float torqueMax=50f;
	private float topRetroSpeed=-45f;
	public float topSpeed=150f;

	//CheckPoint
	private bool checkPoint = false;
	public GameObject raceManager;


	private bool startRace = true;
	private bool finishRace = false;

	//bonus info
	public GameObject bonusSlot; 
	public Material[] bonusMalerials= new Material[4];
	private float speedMultiplierBonus=3;
	private Bonus bonusAcquired;
	private ParticleSystem turbo;

	//malus info
	private bool malusActive = false;
	private int reductionCounter = 0;
	private int malusDuration;
	private ParticleSystem malusSmoke;

	private int malusSteerRotation=0;
	private ArrayList mudSpotsList = new ArrayList();
	
	public TextMesh bonusText;
	public TextMesh speedText;

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

	// Use this for initialization
	void Start () 
	{
		rigidbody.centerOfMass=new Vector3(0,-0.2f,0);
		serverBogieCar = (ServerBogieCar) GameObject.Find ("Server").GetComponent("ServerBogieCar");

		GameObject Stain1 = GameObject.Find("Stain1");
		GameObject Stain2 = GameObject.Find("Stain2"); 
		GameObject Stain3 = GameObject.Find("Stain3"); 
		GameObject Stain4 = GameObject.Find("Stain4");
		malusSmoke = ((ParticleSystem)GameObject.Find ("malusSmoke").GetComponent("ParticleSystem"));
		turbo = ((ParticleSystem)GameObject.Find ("turbo").GetComponent("ParticleSystem"));
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
			this.transform.position=new Vector3(lastPosition.x,-0.1556971f,lastPosition.z);
			this.transform.rotation = lastRotation;
		}

		if(currentSpeed > 0)
		{
			speedText.text = currentSpeed.ToString();
		}
		else
			speedText.text="0";
	}

	public void WheelRotate()
	{
		WheelTractionRPM = WheelTraction.rpm;
		//(0,0,-WheelFR.rpm/60*360*Time.deltaTime)
		WheelFRTransform.Rotate(0,0,-WheelFR.rpm/60*360*Time.deltaTime,Space.Self);
		WheelRLTransform.Rotate(0,0,WheelRL.rpm/60*360*Time.deltaTime,Space.Self);
		WheelRRTransform.Rotate(0,0,-WheelRR.rpm/60*360*Time.deltaTime,Space.Self);
		WheelFLTransform.Rotate(0,0,WheelFL.rpm/60*360*Time.deltaTime,Space.Self);

		//TODO: visualizzare sterzata
		//WheelFLTransform.localEulerAngles=new Vector3(WheelFLTransform.localEulerAngles.x,WheelFLTransform.localEulerAngles.y-WheelFL.steerAngle,WheelFLTransform.localEulerAngles.z);
		//WheelFRTransform.localEulerAngles=new Vector3(WheelFRTransform.localEulerAngles.x-WheelFR.steerAngle,WheelFRTransform.localEulerAngles.y,WheelFRTransform.localEulerAngles.z);

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
				if(!dirtBrakeSound.isPlaying)
				{
					dirtBrakeSound.Play();
				}

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

		if (!animation.isPlaying) {
			animation.Play ("Lever");
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
		}
		else {
			if (!animation.isPlaying)
				animation.Play ("Right");
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
			if(other.gameObject.name=="CheckPointYNoFreeze")
			{
				rigidbody.constraints=RigidbodyConstraints.None;
			}
			if(other.gameObject.name=="CheckPointYFreeze")
			{
				rigidbody.constraints=RigidbodyConstraints.FreezePositionY;
			}
		}
		else
		{
			if(other.name=="Poop(Clone)")
			{
				StartCoroutine(PoopEffect());
			}
			else
			{
				if(other.gameObject.tag == "Bonus")
				{
					bonusSound.Play();

					int powerUpId = UnityEngine.Random.Range(1,10);
					serverBogieCar.bonusComunication(powerUpId);
					
					if(powerUpId <=4)					
					{
						bonusText.text="MISSILE taken!";
						bonusSlot.renderer.material = bonusMalerials[3];
						bonusAcquired = new BonusMissile();
						StartCoroutine (Waiting());
					}
				
					else if(powerUpId<=6)		
					{
						bonusText.text="POOP taken!";
						bonusSlot.renderer.material = bonusMalerials[2];
						bonusAcquired = new BonusPoop();
						StartCoroutine (Waiting());
					}
					
					else
					{
						bonusText.text="TURBO taken!";
						bonusSlot.renderer.material = bonusMalerials[1];
						bonusAcquired = new BonusSpeed();
						StartCoroutine (Waiting());
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
						if(other.gameObject.tag == "Malus" && !malusActive)
						{
							malusActive = true;

							float powerUpId = UnityEngine.Random.Range(1f,10f);

							//powerUpId=1;
							serverBogieCar.malusComunication(powerUpId);
							if(powerUpId <= 2.5f)
							{
								StartCoroutine(MalusMud());
							}
							
							else if(powerUpId <= 4.5f)
							{
								StartCoroutine(MalusSlowdown());
							}
							else if(powerUpId <= 8f)
							{
								bonusText.text="SWITCH!!!";
								malusSound.Play();
								serverBogieCar.assignRoles();
								malusActive = false;
								StartCoroutine (Waiting());
							}
							else
							{
								StartCoroutine(MalusSteerFailure());
							}
						
						}
					}
				}
			}
		}
	}

	IEnumerator Waiting()
	{
		yield return new WaitForSeconds(4);
		bonusText.text="";
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag != "Terrain")
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
		turbo.Play();
		bonusText.text="TURBO!!";
		WheelTraction.motorTorque += torqueMax*speedMultiplierBonus;
		yield return new WaitForSeconds(0.5f);
		StartCoroutine (StopTorque(torqueMax*speedMultiplierBonus));
		bonusSlot.GetComponent("MeshRenderer").renderer.material = bonusMalerials[0];
		bonusText.text="";
	}

	//powerUp: il tempo viene decrementato di un valore tra 5 e 10 secondi
	IEnumerator BonusTime()
	{
		int bonusTime = UnityEngine.Random.Range(5,11);
		bonusText.text="BONUS TIME: " +bonusTime+ "sec";
		((RaceManager)raceManager.GetComponent ("RaceManager")).bonusTime(bonusTime);
		yield return new WaitForSeconds(3);
		bonusText.text="";
	}

	public void resetPlank()
	{
		bonusSlot.GetComponent("MeshRenderer").renderer.material = bonusMalerials[0];
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
			{
				WheelTraction.motorTorque = torqueMax*0.05f;
				malusSmoke.emissionRate -= 5;
			}

			if(malusName == "STEER FAILURE")
				malusSmoke.emissionRate -= 5;

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
		bonusText.text="Malus reduced!";
		yield return new WaitForSeconds(2);
		bonusText.text="";
	}

	IEnumerator MalusSteerFailure()
	{
		malusSound.Play();
		malusSmoke.Play();

		malusDuration=5;
		malusSteerRotation=6;

		if(UnityEngine.Random.Range(1,2) == 1)
		{
			malusSteerRotation = -malusSteerRotation;
		}
	
		for(int i=0;i<malusDuration;i++)
		{
			bonusText.text="STEER FAILURE!\nTap the RED button!";
			yield return new WaitForSeconds(1);
		}

		serverBogieCar.malusEnded();
		malusSmoke.Stop();
		malusSmoke.emissionRate=15;
		malusDuration=5;
		malusSteerRotation=0;
		bonusText.text="";
		malusActive = false;
	}

	IEnumerator MalusSlowdown()
	{
		malusSound.Play();
		malusSmoke.Play();

		malusDuration=5;
		serverBogieCar.slowDown(true);
		WheelTraction.motorTorque = -torqueMax*0.2f;

		for(int i=0;i<malusDuration;i++)
		{
			bonusText.text="LEVER FAILURE!\nTap the RED button!";
			yield return new WaitForSeconds(1);
		}

		serverBogieCar.slowDown(false);
		serverBogieCar.malusEnded();
		malusSmoke.Stop();
		malusSmoke.emissionRate=15;
		malusDuration=5;
		bonusText.text="";
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
			bonusText.text="MUD!\nTap the RED button!";
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
		bonusText.text="";
		malusActive = false;
	}
}