using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public class ViewControl : MonoBehaviour {

	public List<string> player1CardsInPlay;
	public List<string> player2CardsInPlay;

	public List<CardProperties> player1CardProperties;
	public List<CardProperties> player2CardProperties;

	private GameObject player;
	private GameObject otherPlayer;

	private CardsInPlay myInPlay;
	private CardsInPlay notMyInPlay;

	void Awake () {

	}

	void Update ()
	{
		if(player && otherPlayer)
		{

			for(int i = 0; i < myInPlay.cardsInPlay.Count; i++)
			{
				player1CardProperties.Add(GameObject.Find(myInPlay.cardsInPlay[i]).GetComponent<CardProperties>());
			}


			for(int i = 0; i < notMyInPlay.cardsInPlay.Count; i++)
			{
				player2CardProperties.Add(GameObject.Find(notMyInPlay.cardsInPlay[i]).GetComponent<CardProperties>());
			}

		}
		else
		{
			FindPlayer();
		}
	}

	/*private bool PlayerTurnEnded ( GameObject player )
	{
		if(!player.GetComponent<Player>().isViewing)
		{
			if(player.GetComponent<CardsInPlay>().turnEnded)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}*/

	private List<string> LookForPlayerCards ( GameObject player )
	{
		if(!player.GetComponent<Player>().isViewing)
		{
			if(player.GetComponent<CardsInPlay>().turnEnded)
			{
				return player.GetComponent<CardsInPlay>().cardsInPlay;
			}
		}
		return null;
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
}
