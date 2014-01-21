using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {

	public Material[] controllerMaterials = new Material[3];

	private NetworkView networkView;
	private string role;

	//valori aggiornati ogni qualvolta viene acquisito/utilizzato/rimosso un bonus o un malus tremite RPC dal server
	private bool hasBonus = false;
	private string bonusName;
	private bool hasMalus = false;
	private string malusName;

	// Use this for initialization
	void Start () {
		GameObject client = GameObject.Find ("Client");
		networkView = (NetworkView) client.GetComponent("NetworkView");
	}

	void Update()
	{
	}

	public void setRole(string role)
	{
		this.role = role;
		updateBonusLabel();
		updateMalusLabel();
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
		updateBonusLabel();
	}

	public void setHasMalus(bool hasMalus)
	{
		this.hasMalus = hasMalus;
		updateMalusLabel();
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
			if (hasBonus)
				this.GetComponent("MeshRenderer").renderer.material = controllerMaterials[0];
			else 
				this.GetComponent("MeshRenderer").renderer.material = controllerMaterials[2];
		}
	}

	public void updateMalusLabel()
	{
		if(role == "Lever2")
		{
			if (hasMalus)
				this.GetComponent("MeshRenderer").renderer.material = controllerMaterials[1];
			else 
				this.GetComponent("MeshRenderer").renderer.material = controllerMaterials[2];
		}
	}

	public void setVibration()
	{
		//da scommentare per la consegna
		//Handheld.Vibrate();
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
