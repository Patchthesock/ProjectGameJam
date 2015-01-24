using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ArmorOnScreen : MonoBehaviour 
{
	public Image selectedImage;
	public List<CardProperties> armorCards;

	public bool isSelected {get; set;}


	CardManager manager;

	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find("CardManager").GetComponent<CardManager>();
		isSelected = false;
	}

	public void SelectArmor ()
	{
		if(!isSelected)
		{
			if(!manager.armorSelected)
			{
				manager.armorSelected = true;
			}

			if(!manager.selectedArmor)
			{
				manager.selectedArmor = this.gameObject;
			}
			else
			{
				manager.selectedArmor.GetComponent<CharacterOnScreen>().isSelected = false;
				manager.selectedArmor.GetComponent<CharacterOnScreen>().selectedImage.enabled = false;
				manager.selectedArmor = this.gameObject;
			}
			isSelected = true;
			selectedImage.enabled = true;
		}
		else
		{
			manager.armorSelected = false;
			isSelected = false;
			selectedImage.enabled = false;
		}
	}
}
