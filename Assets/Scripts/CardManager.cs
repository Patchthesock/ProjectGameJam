using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardManager : MonoBehaviour 
{
	public Button playButton;
	public Button discardButton;
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

	public bool characterSelected = false;
	public GameObject selectedCharacter;

	public bool weaponSelected = false;
	public GameObject selectedWeapon;

	public bool armorSelected = false;
	public GameObject selectedArmor;

	public bool refreshAllCards = true;
	public bool pickToDiscard = false;

	// Use this for initialization
	void Start () 
	{
		//UpdateCards();
	}


	public void CheckCardNumbers ()
	{
		if(!pickToDiscard)
		{
			if(selectedCards.Count == 3)
			{
				playButton.gameObject.SetActive(true);
			}
			else
			{
				playButton.gameObject.SetActive(false);
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
					Debug.Log("new card");
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
		deck.AddRange(selectedCharacter.GetComponent<CharacterOnScreen>().startCards);
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
						c.transform.parent.gameObject.SetActive(false);
					}

				}
			}
			selectedCards.Clear();
			StartDiscard();
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
			}
		}
		selectedCards.Clear();
		gameInfoText.text = "Pick 3 Cards";
		discardButton.gameObject.SetActive(false);
		pickToDiscard = false;
		UpdateCards();
	}

	public void StartDiscard ()
	{
		gameInfoText.text = "Do you want to discard any cards?";
		pickToDiscard = true;
		playButton.gameObject.SetActive(false);
		discardButton.gameObject.SetActive(true);
		discardButtonText.text = "Get Cards";
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{         
			stream.SendNext(cards);
		}
		else
		{         
			this.cards = (List<GameObject>)stream.ReceiveNext();
		}
	}
}
