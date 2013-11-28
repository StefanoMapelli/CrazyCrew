using UnityEngine;
using System.Collections;

public class BogieCar : MonoBehaviour {

	//Conponenti veicolo

	//ruote
	public WheelCollider WheelFR;
	public WheelCollider WheelFL;
	public WheelCollider WheelRR;
	public WheelCollider WheelRL;

	//valori fisici
	public float decelerationSpeed = 50f;
	public float torqueMax = 50f;

	//Valori sterzo
	public float lowestSteerAtSpeed = 50;
	public float lowSpeedSteerAngle = 10;
	public float highSpeedSteerAngle = 1;

	private float steerPercent = 0;

	//freni
	private bool brakeR =false;
	private bool brakeL =false;

	void BrakeROn()
	{
		brakeR=true;
	}

	void BrakeROff()
	{
		brakeR=false;
	}

	void BrakeLOn()
	{
		brakeL=true;
	}

	void BrakeLOff()
	{
		brakeL=false;
	}

	/*
	 * In caso di freno sinistro e destro premuto aziono il freno del veicolo
	 */
	private void Brake()
	{
		Debug.Log("L:  " + brakeL);
		Debug.Log("R:  " + brakeR);

		if(brakeL && brakeR)
		{
			Debug.Log ("dec" + decelerationSpeed);
			WheelRL.brakeTorque=decelerationSpeed;
			WheelRR.brakeTorque=decelerationSpeed;
		}
	}


	/*
	 * Force percent è compreso tra 0 e 1 
	 * Indica quanto hai abbasato la leva sullo Smartphone
	 * rispetto al massimo possibile
	 * 
	 */
	void LeverDown(float forcePercent)
	{
		WheelRL.motorTorque= forcePercent*torqueMax;
		WheelRR.motorTorque= forcePercent*torqueMax;
	}


	/*
	 * Steer percent deve essere compreso tra -1 e 1
	 * Positivo se lo sterzo è a destra
	 * Negativo se sterzo a sinistra
	 * A seconda di quanto sterzo avrò una percentuale
	 * 
	 */

	void Steering(float steerPercent)
	{
		this.steerPercent = steerPercent;
	}

	void Steer()
	{
		float speedFactor = rigidbody.velocity.magnitude/lowestSteerAtSpeed;
		float currentSteerAngle = Mathf.Lerp(lowSpeedSteerAngle,highSpeedSteerAngle,speedFactor);
		
		currentSteerAngle *= steerPercent;
		
		WheelFL.steerAngle = currentSteerAngle;
		WheelFR.steerAngle = currentSteerAngle;
	}

	// Use this for initialization
	void Start () {

		rigidbody.centerOfMass=new Vector3(0,-0.9f,0);
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.Q) || (Input.GetKey(KeyCode.O)))
		{
			LeverDown(1);

		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			BrakeLOn();
		}
		if(Input.GetKeyDown(KeyCode.L))
		{
			BrakeROn();
		}
		if(Input.GetKeyUp(KeyCode.A))
		{
			BrakeLOff();
		}
		if(Input.GetKeyUp(KeyCode.L))
		{
			BrakeROff();
		}

		//controllo ogni volta se entrambi i freni sono premuti ed in tal caso freno il veicolo
		Brake();
		//update dello sterzo in base all'ultimo angolo di sterzo ricevuto dal client
		Steer();
	}
}
