using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int maxHealth;
	public int maxStamina;
	
	public bool isViewing;
	public int health;
	public int stamina;
	public int position = 0;
	public int tempDefence = 0;

	private PhotonView pv;

	void Awake ()
	{
		isViewing = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().isViewing;
		pv = this.GetComponent<PhotonView>();
		if(isViewing && pv.isMine)
			this.gameObject.GetComponent<CardManager>().enabled = false;
	}

	void Start()
	{




		health = maxHealth;
		stamina = maxStamina;
	}

	// Return Health
	[RPC]
	public int GetHealth()
	{
		return health;
	}

	// Return Stamina
	[RPC]
	public int GetStamina ()
	{
		return stamina;
	}

	[RPC]
	public int GetTempDefence ()
	{
		return tempDefence;
	}
	[RPC]

	public int GetPosition ()
	{
		return position;
	}

	[RPC]
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
	[RPC]
	public void DamageHealth(int damage)
	{
		health -= damage;
	}

	// Damage Stamina
	[RPC]
	public void DamageStamina(int damage)
	{
		stamina += damage;
	}

	// Increase Stamina
	[RPC]
	public void IncreaseStamina(int inc)
	{
		stamina += inc;
	}

	[RPC]
	public void IncreaseTempDefence(int inc)
	{
		tempDefence += inc;
	}

	[RPC]
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
