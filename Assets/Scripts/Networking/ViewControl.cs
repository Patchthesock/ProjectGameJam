using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;
using Fungus;

public class ViewControl : MonoBehaviour {

	public List<string> player1CardsInPlay;
	public List<string> player2CardsInPlay;

	public List<CardProperties> player1SelectedCardProperties;
	public List<CardProperties> player2SelectedCardProperties;

	public List<CardProperties> player1TableCardProperties;
	public List<CardProperties> player2TableCardProperties;

	public GameObject player;
	public GameObject otherPlayer;

	private PhotonView playerView;
	private PhotonView otherPlayerView;

	private Player myPlayer;
	private Player notMyPlayer;

	private FungusScript sequenceScript;

	private CardsInPlay myInPlay;
	private CardsInPlay notMyInPlay;

	private bool hasBeenUpdated = false;
	//private GameObject fungus;
	private GameObject canvas;
	private bool first = true;
	private bool startedCards;

	private bool roundStarted = false;

	void Start () {
		canvas = GameObject.Find ("Canvas");
		canvas.SetActive(false);
		sequenceScript = GameObject.Find("FungusScript").GetComponent<FungusScript>();
	}

	void Update ()
	{
		if(player && otherPlayer)
		{
			// Start fungus and the canvas
			canvas.SetActive(true);

			// Link Player to theyre stat bars
			if(first)
			{
				GameObject.Find ("PlayerTwoHealthBar").GetComponent<HealthUIControl>().player = otherPlayer;
				GameObject.Find ("PlayerTwoStaminaBar").GetComponent<StaminaUIControl>().player = otherPlayer;
				GameObject.Find ("PlayerTwoFooting").GetComponent<FootingUIControl>().player = otherPlayer;
				GameObject.Find ("PlayerTwoTempDef").GetComponent<TempDefUIControl>().player = otherPlayer;

				GameObject.Find ("PlayerOneHealthBar").GetComponent<HealthUIControl>().player = player;
				GameObject.Find ("PlayerOneStaminaBar").GetComponent<StaminaUIControl>().player = player;
				GameObject.Find ("PlayerOneFooting").GetComponent<FootingUIControl>().player = player;
				GameObject.Find ("PlayerOneTempDef").GetComponent<TempDefUIControl>().player = player;

				sequenceScript.ExecuteSequence("Start");

				first = false;
			}

			if(myInPlay.hasCardsOnScreen && notMyInPlay.hasCardsOnScreen && !startedCards)
			{
				Debug.Log("dealing cards");
				startedCards = true;
				sequenceScript.ExecuteSequence("TurnP1C1");
			}

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
						// Set Player Card Values
						player1TableCard.GetComponent<CardProperties>().cardImageTexture = player1TableCardProperties[i].cardImageTexture;
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

						GameObject.Find("FrontP1" + i).renderer.material.mainTexture = player1TableCardProperties[i].cardImageTexture;

						i++;
					}

					i = 0;
					foreach(GameObject player2TableCard in GameObject.FindGameObjectsWithTag("Player2TableCards"))
					{
						// Load Cards into the objects on the table here.
						player2TableCard.GetComponent<CardProperties>().cardImageTexture = player2TableCardProperties[i].cardImageTexture;
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


						GameObject.Find("FrontP2" + i).renderer.material.mainTexture = player2TableCardProperties[i].cardImageTexture;


						i++;
					}
					// Start fungus deal sequence.

				}

				StartCoroutine(PlayCard());
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

	IEnumerator PlayCard ()
	{
		Debug.Log("playing cards");
		int attack;
		CardProperties props;
		yield return new WaitForSeconds(1);
		props = player1SelectedCardProperties[0];

		sequenceScript.ExecuteSequence("PlayP1C" + myInPlay.callOrder[0]);

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
		yield return new WaitForSeconds(5);
		props = player2SelectedCardProperties[0];

		sequenceScript.ExecuteSequence("PlayP2C" + myInPlay.callOrder[0]);

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
		
		yield return new WaitForSeconds(9);
		this.playerView.RPC("ResetTempDefence", PhotonTargets.All);
		//myPlayer.ResetTempDefence();
		props = player1SelectedCardProperties[1];

		sequenceScript.ExecuteSequence("PlayP1C" + myInPlay.callOrder[1]);
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
		yield return new WaitForSeconds(14);
		this.otherPlayerView.RPC("ResetTempDefence", PhotonTargets.All);
		//notMyPlayer.ResetTempDefence();
		props = player2SelectedCardProperties[1];

		sequenceScript.ExecuteSequence("PlayP2C" + myInPlay.callOrder[1]);

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
		
		yield return new WaitForSeconds(19);
		this.playerView.RPC("ResetTempDefence", PhotonTargets.All);
		//myPlayer.ResetTempDefence();
		props = player1SelectedCardProperties[2];

		sequenceScript.ExecuteSequence("PlayP1C" + myInPlay.callOrder[2]);

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
		yield return new WaitForSeconds(23);
		this.otherPlayerView.RPC("ResetTempDefence", PhotonTargets.All);
		//notMyPlayer.ResetTempDefence();
		
		props = player2SelectedCardProperties[2];

		sequenceScript.ExecuteSequence("PlayP2C" + myInPlay.callOrder[2]);

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
		
		player1SelectedCardProperties.Clear();
		player2SelectedCardProperties.Clear();
		//myPlayer.ResetTempDefence();
		//notMyPlayer.ResetTempDefence();
		this.playerView.RPC("ResetTempDefence", PhotonTargets.All);
		this.otherPlayerView.RPC("ResetTempDefence", PhotonTargets.All);
		
		this.playerView.RPC("CardsInPlayEmpty", PhotonTargets.All);
		this.otherPlayerView.RPC("CardsInPlayEmpty", PhotonTargets.All);
		
		roundStarted = false;
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
					this.myPlayer = player.GetComponent<Player>();
					this.myInPlay = player.GetComponent<CardsInPlay>();
					this.playerView = player.GetComponent<PhotonView>();
				}
				else if(myPlayer != this.player)
				{
					this.otherPlayer = myPlayer;
					this.notMyPlayer = player.GetComponent<Player>();
					this.notMyInPlay = otherPlayer.GetComponent<CardsInPlay>();
					this.otherPlayerView = otherPlayer.GetComponent<PhotonView>();
				}

			}
		}
	}
}
