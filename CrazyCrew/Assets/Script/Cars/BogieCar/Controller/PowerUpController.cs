using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {

	private NetworkView networkView;
	private string role;
	private TextMesh text;

	//valori aggiornati ogni qualvolta viene acquisito/utilizzato/rimosso un bonus o un malus tremite RPC dal server
	private bool hasBonus = false;
	private string bonusName;
	private bool hasMalus = false;
	private string malusName;

	// Use this for initialization
	void Start () {
		GameObject client = GameObject.Find ("Client");
		networkView = (NetworkView) client.GetComponent("NetworkView");
		text = ((TextMesh)GameObject.Find ("PowerUpLabel").GetComponent("TextMesh"));
	}

	void Update()
	{
	}

	public void setRole(string role)
	{
		this.role = role;
	}

	public void setBonusName(string name)
	{
		bonusName = name;
	}

	public string getBonusName()
	{
		return bonusName;
	}

	public void setHasBonus(bool hasBonus)
	{
		this.hasBonus = hasBonus;
	}

	public void setHasMalus(bool hasMalus)
	{
		this.hasMalus = hasMalus;
	}

	public void setMalusName(string name)
	{
		malusName = name;
	}
	
	public string getMalusName()
	{
		return malusName;
	}

	public void updateBonusLabel()
	{
		if(role == "Lever1")
		{
			if(hasBonus)
			{
				text.text = "Activate "+bonusName;
			}
			else
			{
				text.text = "Activate bonus";
			}
		}
	}

	public void updateMalusLabel()
	{
		if(role == "Lever2")
		{
			if(hasMalus)
			{
				text.text = "Reduce "+malusName;
			}
			else
			{
				text.text = "Reduce malus effects";
			}
		}
	}
	
	void OnMouseDown()
	{
		if(role == "Lever1" && hasBonus)
		{
			networkView.RPC("activateBonus", RPCMode.Server);
			Debug.Log("Sono: "+role+" e ho premuto il PowerUpButton");
		}
		else
		{
			if(role == "Lever2" && hasMalus)
			{
				networkView.RPC("reduceMalus", RPCMode.Server, malusName);
				Debug.Log("Sono: "+role+" e ho premuto il PowerUpButton");
			}
		}
	}
}
