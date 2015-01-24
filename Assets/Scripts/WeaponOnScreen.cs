using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponOnScreen : MonoBehaviour 
{
	public Image selectedImage;
	public List<CardProperties> weaponCards;

	public bool isSelected {get; set;}


	CardManager manager;

	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find("CardManager").GetComponent<CardManager>();
		isSelected = false;
	}

	public void SelectWeapon ()
	{
		if(!isSelected)
		{
			if(!manager.weaponSelected)
			{
				manager.weaponSelected = true;
			}

			if(!manager.selectedWeapon)
			{
				manager.selectedWeapon = this.gameObject;
			}
			else
			{
				manager.selectedWeapon.GetComponent<CharacterOnScreen>().isSelected = false;
				manager.selectedWeapon.GetComponent<CharacterOnScreen>().selectedImage.enabled = false;
				manager.selectedWeapon = this.gameObject;
			}
			isSelected = true;
			selectedImage.enabled = true;
		}
		else
		{
			manager.weaponSelected = false;
			isSelected = false;
			selectedImage.enabled = false;
		}
	}
}
