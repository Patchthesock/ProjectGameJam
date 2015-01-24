using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour 
{


	public CardManager manager;
	// Use this for initialization

	public void SelectCharacter ()
	{
		manager.SelectCharacter();
	}
	
	public void SelectWeapon ()
	{
		manager.SelectWeapon();
	}
	
	public void SelectArmor ()
	{
		manager.SelectArmor();
	}
	
	public void PlayCards ()
	{
		manager.PlayCards();
	}
	
	public void DiscardCards ()
	{
		manager.DiscardCards();
	}

	public void PassTurn ()
	{
		manager.PassTurn();
	}
}
