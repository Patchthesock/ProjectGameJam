using UnityEngine;
using System.Collections;

public class PlayControl : MonoBehaviour {

	public bool isPlaying = false;
	public bool isReady = false;

	private GameObject player = null;
	private NetworkManager NM;

	void Start ()
	{
		NM = this.GetComponent<NetworkManager>();
	}

	void Update ()
	{
		if(NM.startGame && !player)
			FindPlayer();
	}

	void RoundControl()
	{

	}

	void FindPlayer()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject myPlayer in players)
		{
			if(myPlayer.GetComponent<PhotonView>().isMine)
			{
				this.player = myPlayer;
				this.player.GetComponent<CardManager>().enabled = true;
			}
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{

		}
		else 
		{	

		}
	}
}
