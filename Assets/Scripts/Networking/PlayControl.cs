using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayControl : MonoBehaviour {

	public bool isPlaying = false;
	public bool isReady = false;

	public GameObject player = null;
	public GameObject otherPlayer = null;
	private NetworkManager NM;

	private PhotonView pv;

	public List<CardProperties> myCards;
	public List<CardProperties> notMyCards;

	private CardsInPlay myInPlay;
	private CardsInPlay notMyInPlay;
	
	private bool hasCharacters = false;
	
	void Start ()
	{
		NM = this.GetComponent<NetworkManager>();
		pv = GetComponent<PhotonView>();
	}

	void Update ()
	{
		if(NM.startGame && !player)
			FindPlayer();

		if(!otherPlayer)
			FindPlayer();



		if(player && otherPlayer && !hasCharacters)
		{
			hasCharacters = true;
			player.GetComponent<CardManager>().SetPlayer1();
			otherPlayer.GetComponent<CardManager>().SetPlayer2();
		}
	}



	void FindPlayer()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject myPlayer in players)
		{
			if(myPlayer.GetComponent<PhotonView>().isMine)
			{
				this.player = myPlayer;
				this.myInPlay = player.GetComponent<CardsInPlay>();
			}
			else
			{
				this.otherPlayer = myPlayer;
				this.notMyInPlay = otherPlayer.GetComponent<CardsInPlay>();
			}
		}
	}



	void OnPhotonSerialize (PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting && pv.isMine)
		{
			stream.SendNext(this.myInPlay);
			stream.SendNext(this.notMyInPlay);
		}
		else
		{
			this.myInPlay = (CardsInPlay)stream.ReceiveNext();
			this.notMyInPlay = (CardsInPlay)stream.ReceiveNext();
		}
	}
}
