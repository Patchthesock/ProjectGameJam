﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardOnScreen : MonoBehaviour 
{
	public int attack;
	public int defence;
	public int stamina;
	
	public Image selectedImage;

	public int order;
	
	public bool isSelected;

	public string cardName;

	public CardManager manager;

	public CardProperties cardProps;

	void Awake ()
	{

	}
	
	// Use this for initialization
	void Start () 
	{
		//manager = GameObject.Find("CardManager").GetComponent<CardManager>();
		isSelected = false;
	}

	public void setCard ()
	{
		attack = cardProps.attack;
		defence = cardProps.defence;
		stamina = cardProps.stamina;
		cardName = cardProps.cardName;
		GetComponent<Image>().sprite = cardProps.cardImage;
	}
	
	public void SelectCard ()
	{
		if(!isSelected)
		{
			if(!manager.pickToDiscard)
			{
				if(manager.selectedCards.Count < 3)
				{
					manager.selectedCards.Add(cardProps);
					manager.cardCallOrder.Add(order);
					isSelected = true;
					selectedImage.enabled = true;
				}
			}
			else
			{
				manager.selectedCards.Add(cardProps);
				manager.cardCallOrder.Add(order);
				isSelected = true;
				selectedImage.enabled = true;
			}
		}
		else
		{
			manager.selectedCards.Remove(cardProps);
			manager.cardCallOrder.Remove(order);
			isSelected = false;
			selectedImage.enabled = false;
		}

		manager.CheckCardNumbers();
	}
}
