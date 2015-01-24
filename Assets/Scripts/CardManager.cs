using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardManager : MonoBehaviour 
{
	public List<GameObject> cards;

	public List<CardProperties> deck;
	public List<CardProperties> playedCards;
	public List<CardProperties> selectedCards;
	public List<CardProperties> currentCards;

	public GameObject characterSelectCanvas;
	public GameObject weaoponSelectCanvas;
	public GameObject armorSelectCanvas;
	public GameObject cardsCanvas;

	public bool characterSelected = false;
	public GameObject selectedCharacter;

	public bool refreshAllCards = true;
	// Use this for initialization
	void Start () 
	{
		//UpdateCards();
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
		deck.AddRange(selectedCharacter.GetComponent<CharacterOnScreen>().startCards);
		characterSelectCanvas.SetActive(false);
		weaoponSelectCanvas.SetActive(true);
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
					playedCards.Add(crd.cardProps);
					currentCards.Remove(crd.cardProps);
					crd.selectedImage.enabled = false;
					c.gameObject.SetActive(false);
					c.transform.parent.gameObject.SetActive(false);
				}
			}
			Debug.Log("send card info");

			selectedCards.Clear();
			UpdateCards();
		}
		else
		{
			Debug.Log("Selected less than 3 cards");
		}
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
