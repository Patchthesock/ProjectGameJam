using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float maxHealth;

	[HideInInspector]
	public bool isViewing;
	private float health;
	private PhotonView pv;

	void Start()
	{
		isViewing = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().isViewing;
		health = maxHealth;
		pv = this.GetComponent<PhotonView>();
	}

	public float GetHealth()
	{
		return health;
	}
	
	public void DamageHealth(float damage)
	{
		health -= damage;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting && pv.isMine)
		{
			stream.SendNext(health);
		}
		else 
		{	
			this.health = (float)stream.ReceiveNext();
		}
	}
}
