using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayControl : MonoBehaviour {

	public bool isPlaying = false;
	public bool isReady = false;

	public GameObject player = null;
	public GameObject otherPlayer = null;
	private NetworkManager NM;
	private PhotonView playerView;
	private PhotonView otherPlayerView;
	private PhotonView pv;

	public List<CardProperties> myCards;
	public List<CardProperties> notMyCards;

	private CardsInPlay myInPlay;
	private CardsInPlay notMyInPlay;

	private Player myPlayer;
	private Player notMyPlayer;

	private bool thisPlayerStart = true;

	public bool roundStarted;

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

		if(PhotonNetwork.isMasterClient && player != null && otherPlayer != null)
		{
			if(!roundStarted)
			{
				if(myInPlay.turnEnded && notMyInPlay.turnEnded)
				{
					roundStarted = true;
					RoundControl();
				}
			}
		}


	}

	void RoundControl()
	{
		Debug.Log("round started");
		for(int i = 0; i < myInPlay.cardsInPlay.Count; i++)
		{
			myCards.Add(GameObject.Find(myInPlay.cardsInPlay[i]).GetComponent<CardProperties>());
		}

		for(int i = 0; i < notMyInPlay.cardsInPlay.Count; i++)
		{
			notMyCards.Add(GameObject.Find(notMyInPlay.cardsInPlay[i]).GetComponent<CardProperties>());
		}

		if(thisPlayerStart)
		{
			StartCoroutine(PlayCard());
		}
	}

	IEnumerator PlayCard ()
	{
		int attack;
		CardProperties props;
		yield return new WaitForSeconds(1);
		props = myCards[0];
		attack = props.attack - notMyPlayer.GetTempDefence();
		if(attack < 0)
			attack = 0;

		this.otherPlayerView.RPC("DamageHealth", PhotonTargets.All,(object)attack);
		this.otherPlayerView.RPC("ChangePosition", PhotonTargets.All,(object)props.posture);
		this.playerView.RPC("IncreaseTempDefence", PhotonTargets.All,(object)props.defence);
		this.playerView.RPC("DamageStamina", PhotonTargets.All,(object)props.stamina);
		//notMyPlayer.DamageHealth(attack);
		//notMyPlayer.ChangePosition(props.posture);
		//myPlayer.IncreaseTempDefence(props.defence);
		//myPlayer.DamageStamina(props.stamina);
		yield return new WaitForSeconds(1);
		props = notMyCards[0];
		attack = props.attack - myPlayer.GetTempDefence();
		if(attack < 0)
			attack = 0;

		this.playerView.RPC("DamageHealth", PhotonTargets.All,(object)attack);
		this.playerView.RPC("ChangePosition", PhotonTargets.All,(object)props.posture);
		this.otherPlayerView.RPC("IncreaseTempDefence", PhotonTargets.All,(object)props.defence);
		this.otherPlayerView.RPC("DamageStamina", PhotonTargets.All,(object)props.stamina);
		//myPlayer.DamageHealth(attack);
		//myPlayer.ChangePosition(props.posture);
		//notMyPlayer.IncreaseTempDefence(props.defence);
		//notMyPlayer.DamageStamina(props.stamina);

		yield return new WaitForSeconds(1);
		this.playerView.RPC("ResetTempDefence", PhotonTargets.All);
		//myPlayer.ResetTempDefence();
		props = myCards[1];
		attack = props.attack - notMyPlayer.GetTempDefence();
		if(attack < 0)
			attack = 0;

		this.otherPlayerView.RPC("DamageHealth", PhotonTargets.All,(object)attack);
		this.otherPlayerView.RPC("ChangePosition", PhotonTargets.All,(object)props.posture);
		this.playerView.RPC("IncreaseTempDefence", PhotonTargets.All,(object)props.defence);
		this.playerView.RPC("DamageStamina", PhotonTargets.All,(object)props.stamina);
		//notMyPlayer.DamageHealth(attack);
		//notMyPlayer.ChangePosition(props.posture);
		//myPlayer.IncreaseTempDefence(props.defence);
		//myPlayer.DamageStamina(props.stamina);
		yield return new WaitForSeconds(1);
		this.otherPlayerView.RPC("ResetTempDefence", PhotonTargets.All);
		//notMyPlayer.ResetTempDefence();
		props = notMyCards[1];
		attack = props.attack - myPlayer.GetTempDefence();
		if(attack < 0)
			attack = 0;

		this.playerView.RPC("DamageHealth", PhotonTargets.All,(object)attack);
		this.playerView.RPC("ChangePosition", PhotonTargets.All,(object)props.posture);
		this.otherPlayerView.RPC("IncreaseTempDefence", PhotonTargets.All,(object)props.defence);
		this.otherPlayerView.RPC("DamageStamina", PhotonTargets.All,(object)props.stamina);
		//myPlayer.DamageHealth(attack);
		//myPlayer.ChangePosition(props.posture);
		//notMyPlayer.IncreaseTempDefence(props.defence);
		//notMyPlayer.DamageStamina(props.stamina);

		yield return new WaitForSeconds(1);
		this.playerView.RPC("ResetTempDefence", PhotonTargets.All);
		//myPlayer.ResetTempDefence();
		props = myCards[2];
		attack = props.attack - notMyPlayer.GetTempDefence();
		if(attack < 0)
			attack = 0;


		this.otherPlayerView.RPC("DamageHealth", PhotonTargets.All,(object)attack);
		this.otherPlayerView.RPC("ChangePosition", PhotonTargets.All,(object)props.posture);
		this.playerView.RPC("IncreaseTempDefence", PhotonTargets.All,(object)props.defence);
		this.playerView.RPC("DamageStamina", PhotonTargets.All,(object)props.stamina);
		//notMyPlayer.DamageHealth(attack);
		//notMyPlayer.ChangePosition(props.posture);
		//myPlayer.IncreaseTempDefence(props.defence);
		//myPlayer.DamageStamina(props.stamina);
		yield return new WaitForSeconds(1);
		this.otherPlayerView.RPC("ResetTempDefence", PhotonTargets.All);
		//notMyPlayer.ResetTempDefence();

		props = notMyCards[2];
		attack = props.attack - myPlayer.GetTempDefence();
		if(attack < 0)
			attack = 0;

		this.playerView.RPC("DamageHealth", PhotonTargets.All,(object)attack);
		this.playerView.RPC("ChangePosition", PhotonTargets.All,(object)props.posture);
		this.otherPlayerView.RPC("IncreaseTempDefence", PhotonTargets.All,(object)props.defence);
		this.otherPlayerView.RPC("DamageStamina", PhotonTargets.All,(object)props.stamina);
		//myPlayer.DamageHealth(attack);
		//myPlayer.ChangePosition(props.posture);
		//notMyPlayer.IncreaseTempDefence(props.defence);
		//notMyPlayer.DamageStamina(props.stamina);

		notMyCards.Clear();
		myCards.Clear();
		//myPlayer.ResetTempDefence();
		//notMyPlayer.ResetTempDefence();
		this.playerView.RPC("ResetTempDefence", PhotonTargets.All);
		this.otherPlayerView.RPC("ResetTempDefence", PhotonTargets.All);

		this.playerView.RPC("CardsInPlayEmpty", PhotonTargets.All);
		this.otherPlayerView.RPC("CardsInPlayEmpty", PhotonTargets.All);

		roundStarted = false;
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
				this.myPlayer = player.GetComponent<Player>();
				playerView = player.GetComponent<PhotonView>();
			}
			else
			{
				this.otherPlayer = myPlayer;
				this.notMyInPlay = otherPlayer.GetComponent<CardsInPlay>();
				this.notMyPlayer = otherPlayer.GetComponent<Player>();
				otherPlayerView = otherPlayer.GetComponent<PhotonView>();
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
