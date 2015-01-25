using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public class ViewControl : MonoBehaviour {

	public List<string> player1CardsInPlay;
	public List<string> player2CardsInPlay;

	public List<CardProperties> player1SelectedCardProperties;
	public List<CardProperties> player2SelectedCardProperties;

	public List<CardProperties> player1TableCardProperties;
	public List<CardProperties> player2TableCardProperties;

	public GameObject player;
	public GameObject otherPlayer;

	private CardsInPlay myInPlay;
	private CardsInPlay notMyInPlay;

	private bool hasBeenUpdated = false;
	private GameObject fungus;

	void Awake () {
		fungus = GameObject.Find ("FungusScript");
		fungus.SetActive(false);
	}

	void Update ()
	{
		if(player && otherPlayer)
		{
			fungus.SetActive(true);
			if(myInPlay.turnEnded && notMyInPlay.turnEnded && !hasBeenUpdated)
			{
				hasBeenUpdated = true;
				for(int i = 0; i < myInPlay.cardsInPlay.Count; i++)
				{
					player1SelectedCardProperties.Add(GameObject.Find(myInPlay.cardsInPlay[i]).GetComponent<CardProperties>());
				}
				
				for(int i = 0; i < notMyInPlay.cardsInPlay.Count; i++)
				{
					player2SelectedCardProperties.Add(GameObject.Find(notMyInPlay.cardsInPlay[i]).GetComponent<CardProperties>());
				}

				for(int i = 0; i< myInPlay.cardsOnTable.Count; i++)
				{
					player1TableCardProperties.Add(GameObject.Find(myInPlay.cardsOnTable[i]).GetComponent<CardProperties>());
				}

				for(int i = 0; i < notMyInPlay.cardsOnTable.Count; i++)
				{
					player2TableCardProperties.Add(GameObject.Find(notMyInPlay.cardsOnTable[i]).GetComponent<CardProperties>());
				}


				// If both players have selected there table cards.
				if(player1TableCardProperties.Count >= 4 && player2TableCardProperties.Count >= 4)
				{
					// Load Cards into the objects on the table here.
					// NB Images not loaded yet.

					int i = 0;
					foreach(GameObject player1TableCard in GameObject.FindGameObjectsWithTag("Player1TableCards"))
					{

						player1TableCard.GetComponent<CardProperties>().cardName = player1TableCardProperties[i].cardName;
						player1TableCard.GetComponent<CardProperties>().defenceCard = player1TableCardProperties[i].defenceCard;
						player1TableCard.GetComponent<CardProperties>().utilityCard = player1TableCardProperties[i].utilityCard;
						player1TableCard.GetComponent<CardProperties>().attack = player1TableCardProperties[i].attack;
						player1TableCard.GetComponent<CardProperties>().defence = player1TableCardProperties[i].defence;
						player1TableCard.GetComponent<CardProperties>().stamina = player1TableCardProperties[i].stamina;
						player1TableCard.GetComponent<CardProperties>().posture = player1TableCardProperties[i].posture;
						player1TableCard.GetComponent<CardProperties>().diceValue = player1TableCardProperties[i].diceValue;
						player1TableCard.GetComponent<CardProperties>().defenceValue = player1TableCardProperties[i].defenceValue;
						player1TableCard.GetComponent<CardProperties>().AttackBonus = player1TableCardProperties[i].AttackBonus;

						i++;
					}

					i = 0;
					foreach(GameObject player2TableCard in GameObject.FindGameObjectsWithTag("Player2TableCards"))
					{
						// Load Cards into the objects on the table here.
						player2TableCard.GetComponent<CardProperties>().cardName = player2TableCardProperties[i].cardName;
						player2TableCard.GetComponent<CardProperties>().defenceCard = player2TableCardProperties[i].defenceCard;
						player2TableCard.GetComponent<CardProperties>().utilityCard = player2TableCardProperties[i].utilityCard;
						player2TableCard.GetComponent<CardProperties>().attack = player2TableCardProperties[i].attack;
						player2TableCard.GetComponent<CardProperties>().defence = player2TableCardProperties[i].defence;
						player2TableCard.GetComponent<CardProperties>().stamina = player2TableCardProperties[i].stamina;
						player2TableCard.GetComponent<CardProperties>().posture = player2TableCardProperties[i].posture;
						player2TableCard.GetComponent<CardProperties>().diceValue = player2TableCardProperties[i].diceValue;
						player2TableCard.GetComponent<CardProperties>().defenceValue = player2TableCardProperties[i].defenceValue;
						player2TableCard.GetComponent<CardProperties>().AttackBonus = player2TableCardProperties[i].AttackBonus;

						i++;
					}
					// Start fungus deal sequence.

				}
			}
			else if(!myInPlay.turnEnded && !notMyInPlay.turnEnded)
			{
				hasBeenUpdated = false;
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
			if(myPlayer.GetComponent<CardManager>().enabled == true)
			{
				if(!this.player)
				{
					this.player = myPlayer;
					this.myInPlay = player.GetComponent<CardsInPlay>();
					GameObject.Find("PlayerOneHealthBar").GetComponent<HealthUIControl>().player = myPlayer;
				}
				else if(myPlayer != this.player)
				{
					this.otherPlayer = myPlayer;
					this.notMyInPlay = otherPlayer.GetComponent<CardsInPlay>();
					GameObject.Find("PlayerTwoHealthBar").GetComponent<HealthUIControl>().player = myPlayer;
				}

			}
		}
	}
}
