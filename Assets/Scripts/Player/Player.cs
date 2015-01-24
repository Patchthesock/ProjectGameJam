using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CardManager))]

public class Player : MonoBehaviour {

	public int maxHealth;
	public int maxStamina;

	public bool isViewing;
	private int health;
	private int stamina;
	private PhotonView pv;

	void Start()
	{
		isViewing = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().isViewing;
		health = maxHealth;
		pv = this.GetComponent<PhotonView>();

		if(!isViewing)
			this.gameObject.GetComponent<CardManager>().enabled = true;
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

	// Damage Health
	public void DamageHealth(int damage)
	{
		health -= damage;
	}

	// Damage Stamina
	public void DamageStamina(int damage)
	{
		stamina -= damage;
	}

	// Increase Stamina
	public void IncreaseStamina(int inc)
	{
		stamina += inc;
	}

	// Photon Serialize
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting && pv.isMine)
		{
			stream.SendNext(health);
			stream.SendNext(stamina);
			stream.SendNext(isViewing);
		}
		else 
		{	
			this.health = (int)stream.ReceiveNext();
			this.stamina = (int)stream.ReceiveNext();
			this.isViewing = (bool)stream.ReceiveNext();
		}
	}
}
