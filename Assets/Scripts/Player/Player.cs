using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int maxHealth;
	public int maxStamina;

	[HideInInspector]
	public bool isViewing;
	public int health;
	public int stamina;
	public int position = 0;
	public int tempDefence = 0;

	private PhotonView pv;

	void Start()
	{
		isViewing = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().isViewing;
		health = maxHealth;
		stamina = maxStamina;
		pv = this.GetComponent<PhotonView>();
	}

	// Return Health
	public int GetHealth()
	{
		return health;
	}

	// Return Stamina	
	public int GetStamina ()
	{
		return stamina;
	}

	public int GetTempDefence ()
	{
		return tempDefence;
	}

	public int GetPosition ()
	{
		return position;
	}

	public void ChangePosition(int pos)
	{
		position += pos;
		if(position > 2)
		{
			position = 2;
		}
		else if(position < -2)
		{
			position = -2;
		}
	}

	// Damage Health
	public void DamageHealth(int damage)
	{
		health -= damage;
	}

	// Damage Stamina
	public void DamageStamina(int damage)
	{
		stamina += damage;
	}

	// Increase Stamina
	public void IncreaseStamina(int inc)
	{
		stamina += inc;
	}

	public void IncreaseTempDefence(int inc)
	{
		tempDefence += inc;
	}

	public void ResetTempDefence()
	{
		tempDefence = 0;
	}

	// Photon Serialize
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting && pv.isMine)
		{
			stream.SendNext(health);
			stream.SendNext(stamina);
			stream.SendNext(position);
			stream.SendNext(tempDefence);
			stream.SendNext(isViewing);
		}
		else 
		{	
			this.health = (int)stream.ReceiveNext();
			this.stamina = (int)stream.ReceiveNext();
			this.position = (int)stream.ReceiveNext();
			this.tempDefence = (int)stream.ReceiveNext();
			this.isViewing = (bool)stream.ReceiveNext();
		}
	}
}
