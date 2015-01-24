using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterOnScreen : MonoBehaviour 
{
	public Image selectedImage;
	public List<CardProperties> startCards;

	public bool isSelected {get; set;}


	CardManager manager;

	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find("CardManager").GetComponent<CardManager>();
		isSelected = false;
	}

	public void SelectCharacter ()
	{
		if(!isSelected)
		{
			if(!manager.characterSelected)
			{
				manager.characterSelected = true;
			}

			if(!manager.selectedCharacter)
			{
				manager.selectedCharacter = this.gameObject;
			}
			else
			{
				manager.selectedCharacter.GetComponent<CharacterOnScreen>().isSelected = false;
				manager.selectedCharacter.GetComponent<CharacterOnScreen>().selectedImage.enabled = false;
				manager.selectedCharacter = this.gameObject;
			}
			isSelected = true;
			selectedImage.enabled = true;
		}
		else
		{
			manager.characterSelected = false;
			isSelected = false;
			selectedImage.enabled = false;
		}
	}
}
