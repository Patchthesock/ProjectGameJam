using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardOnScreen : MonoBehaviour 
{
	public int attack;
	public int defence;
	public int stamina;
	
	public Image selectedImage;
	
	public bool isSelected {get; set;}

	public string cardName;
	
	Manager manager;

	public CardProperties cardProps;

	void Awake ()
	{

	}
	
	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find("CardManager").GetComponent<Manager>();
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
			if(manager.selectedCards.Count < 3)
			{
				manager.selectedCards.Add(cardProps);
				isSelected = true;
				selectedImage.enabled = true;
			}
		}
		else
		{
			manager.selectedCards.Remove(cardProps);
			isSelected = false;
			selectedImage.enabled = false;
		}
		
	}
}
