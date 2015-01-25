using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardManager : MonoBehaviour 
{
	public GameObject playButton;
	public GameObject discardButton;
	public GameObject passButton;
	public GameObject waitText;
	public Text discardButtonText;
	public Text gameInfoText;
	public List<GameObject> cards;
	public List<CardProperties> deck;
	public List<CardProperties> playedCards;
	public List<CardProperties> selectedCards;
	public List<CardProperties> currentCards;

	public GameObject characterSelectCanvas;
	public GameObject weaponSelectCanvas;
	public GameObject armorSelectCanvas;
	public GameObject cardsCanvas;

	public GameObject character1;
	public GameObject character2;

	public bool characterSelected = false;
	public GameObject selectedCharacter;

	public bool weaponSelected = false;
	public GameObject selectedWeapon;

	public bool armorSelected = false;
	public GameObject selectedArmor;

	public bool refreshAllCards = true;
	public bool pickToDiscard = false;
	public bool hasToWait = false;

	private PhotonView pv;
	private CardsInPlay crdPly;
	
	// Use this for initialization
	void Start () 
	{
		pv = this.GetComponent<PhotonView>();
		crdPly = GetComponent<CardsInPlay>();

		if(pv.isMine)
		{
			cards.Add(GameObject.Find("Card1"));
			cards.Add(GameObject.Find("Card2"));
			cards.Add(GameObject.Find("Card3"));
			cards.Add(GameObject.Find("Card4"));
			cards.Add(GameObject.Find("Card5"));

			character1 = GameObject.Find("Char1");
			character2 = GameObject.Find("Char2");

			playButton = GameObject.Find("PlayButton");
			discardButton = GameObject.Find("DiscardButton");
			passButton = GameObject.Find("PassButton");
			waitText = GameObject.Find("WaitingText");
			characterSelectCanvas = GameObject.Find("CharacterSelect");
			weaponSelectCanvas = GameObject.Find("WeaponSelect");
			armorSelectCanvas = GameObject.Find("ArmorSelect");
			cardsCanvas = GameObject.Find("CardsSelect");
			gameInfoText = GameObject.Find("GameInfo").GetComponent<Text>();
			discardButtonText = discardButton.GetComponentInChildren<Text>();

			CharacterOnScreen[] charObj = FindObjectsOfType(typeof(CharacterOnScreen)) as CharacterOnScreen[];
			foreach(var c in charObj)
			{
				c.manager = this;
			}

			WeaponOnScreen[] weaponObj = FindObjectsOfType(typeof(WeaponOnScreen)) as WeaponOnScreen[];
			foreach(var c in weaponObj)
			{
				c.manager = this;
			}

			ArmorOnScreen[] armorObj = FindObjectsOfType(typeof(ArmorOnScreen)) as ArmorOnScreen[];
			foreach(var c in armorObj)
			{
				c.manager = this;
			}

			CardOnScreen[] cardObj = FindObjectsOfType(typeof(CardOnScreen)) as CardOnScreen[];
			foreach(var c in cardObj)
			{
				c.manager = this;
			}

			UIButtons uibutton = FindObjectOfType(typeof(UIButtons)) as UIButtons;
			uibutton.manager = this;

			playButton.SetActive(false);
			discardButton.SetActive(false);
			cardsCanvas.SetActive(false);
			weaponSelectCanvas.SetActive(false);
			armorSelectCanvas.SetActive(false);
			pickToDiscard = false;
			waitText.SetActive(false);
		}
	}

	public void SetPlayer1 ()
	{
		StartCoroutine(SetChar1());
	}

	public void SetPlayer2 ()
	{
		StartCoroutine(SetChar2());
	}

	IEnumerator SetChar1 ()
	{
		Debug.Log("Set character 1");
		yield return new WaitForSeconds(0.1f);
		if(pv.isMine)
		{
			characterSelected  = true;
			character2.SetActive(false);
			deck.AddRange(character1.GetComponent<CharacterOnScreen>().startCards);
		}
	}

	IEnumerator SetChar2 ()
	{
		Debug.Log("Set character 2");
		yield return new WaitForSeconds(0.1f);
		if(pv.isMine)
		{
			characterSelected = true;
			character1.SetActive(false);
			deck.AddRange(character2.GetComponent<CharacterOnScreen>().startCards);
		}
	}

	void Update ()
	{
		if(pv.isMine)
		{
			if(crdPly)
			{
				if(crdPly.turnEnded && !hasToWait)
				{
					hasToWait = true;
					cardsCanvas.GetComponent<GraphicRaycaster>().enabled = false;
					waitText.SetActive(true);
					passButton.SetActive(false);
					playButton.SetActive(false);

				}
				else if(!crdPly.turnEnded && hasToWait)
				{
					hasToWait = false;
					cardsCanvas.GetComponent<GraphicRaycaster>().enabled = true;
					waitText.SetActive(false);
					StartDiscard();
				}
			}
		}
	}


	public void CheckCardNumbers ()
	{
		if(!pickToDiscard)
		{
			if(selectedCards.Count == 3)
			{
				playButton.SetActive(true);
			}
			else
			{
				playButton.SetActive(false);
			}
		}
		else
		{
			if(selectedCards.Count > 0)
			{
				discardButtonText.text = "Discard";
			}
			else
			{
				discardButtonText.text = "Get Cards";
			}
		}
	}

	public void UpdateCards ()
	{
		foreach(var c in cards)
		{

			var cardOnScreen = c.GetComponent<CardOnScreen>();
			if(refreshAllCards)
			{
				if(deck.Count < 5)
				{
					deck.AddRange(playedCards);
					playedCards.Clear();
				}
				int cardToUse = Random.Range(0, deck.Count);
				cardOnScreen.cardProps = deck[cardToUse];
				currentCards.Add(deck[cardToUse]);
				deck.RemoveAt(cardToUse);
				cardOnScreen.setCard();
			}
			else
			{
				if(deck.Count < 3)
				{
					deck.AddRange(playedCards);
					playedCards.Clear();
				}
				if(cardOnScreen.isSelected)
				{
					cardOnScreen.isSelected = false;
					int cardToUse = Random.Range(0, deck.Count);
					cardOnScreen.cardProps = deck[cardToUse];
					currentCards.Add(deck[cardToUse]);
					deck.RemoveAt(cardToUse);
					cardOnScreen.setCard();
					c.gameObject.SetActive(true);
					c.transform.parent.gameObject.SetActive(true);
				}
			}
		}
		refreshAllCards = false;
	}

	public void SelectCharacter ()
	{
		characterSelectCanvas.SetActive(false);
		weaponSelectCanvas.SetActive(true);

	}

	public void SelectWeapon ()
	{
		deck.AddRange(selectedWeapon.GetComponent<WeaponOnScreen>().weaponCards);
		weaponSelectCanvas.SetActive(false);
		armorSelectCanvas.SetActive(true);
	}

	public void SelectArmor ()
	{
		deck.AddRange(selectedArmor.GetComponent<ArmorOnScreen>().armorCards);
		armorSelectCanvas.SetActive(false);
		cardsCanvas.SetActive(true);
		UpdateCards();
	}

	public void PlayCards ()
	{
		if(selectedCards.Count == 3)
		{
			foreach(var c in cards)
			{
				var crd = c.GetComponent<CardOnScreen>();
				if(crd.isSelected)
				{
					if(crd.cardProps.defenceCard)
					{
						crd.GetComponent<Button>().interactable = false;
						crd.isSelected = false;
						crd.selectedImage.enabled = false;
					}
					else
					{
						//playedCards.Add(crd.cardProps);
						//currentCards.Remove(crd.cardProps);
						crd.selectedImage.enabled = false;
						c.gameObject.SetActive(false);

						//c.transform.parent.gameObject.SetActive(false);
					}

					// set active cards
					crdPly.cardsInPlay.Add(crd.cardProps.name);
				}

				crdPly.cardsOnTable.Add (crd.cardProps.name);
			}

			// End Turn.
			crdPly.turnEnded = true;
			selectedCards.Clear();
			//StartDiscard();
		}
		else
		{
			Debug.Log("Selected less than 3 cards");
		}
	}

	public void DiscardCards ()
	{
		foreach(var c in cards)
		{
			var crd = c.GetComponent<CardOnScreen>();
			if(crd.isSelected)
			{
				playedCards.Add(crd.cardProps);
				currentCards.Remove(crd.cardProps);
				crd.selectedImage.enabled = false;
				c.gameObject.SetActive(false);
				c.transform.parent.gameObject.SetActive(false);

				// Increase Stamina by 3 for each card discarded.
				pv.RPC("IncreaseStamina", PhotonTargets.All, (object)3);
			}
		}
		selectedCards.Clear();
		gameInfoText.text = "Pick 3 Cards";
		discardButton.SetActive(false);
		pickToDiscard = false;
		passButton.SetActive(true); 

		UpdateCards();
	}

	public void StartDiscard ()
	{
		gameInfoText.text = "Do you want to discard any cards?";
		pickToDiscard = true;
		playButton.SetActive(false);
		discardButton.SetActive(true);
		discardButtonText.text = "Get Cards";
	}

	public void PassTurn ()
	{

	}
}
