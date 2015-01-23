using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Manager : MonoBehaviour 
{
	public List <GameObject> cards;

	public List <CardProperties> deck;
	public List <CardProperties> playedCards;
	public List<CardProperties> selectedCards; // {get; set;}

	// Use this for initialization
	void Start () 
	{
		UpdateCards();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void UpdateCards ()
	{
		foreach(var c in cards)
		{
			int cardToUse = Random.Range(0, deck.Count);
			c.GetComponent<CardOnScreen>().cardProps = deck[cardToUse];
			c.GetComponent<CardOnScreen>().setCard();
		}
	}

	public void PlayCards ()
	{
		if(selectedCards.Count == 3)
		{
			foreach(var c in cards)
			{
				if(c.GetComponent<CardOnScreen>().isSelected)
				{
					c.gameObject.SetActive(false);
					c.transform.parent.gameObject.SetActive(false);
				}
			}
			Debug.Log("send card info");
		}
		else
		{
			Debug.Log("Selected less than 3 cards");
		}
	}
}
