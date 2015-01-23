using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardProperties : MonoBehaviour 
{
	public int attack;
	public int defence;
	public int stamina;

	public Image selectedImage;

	public bool isSelected {get; set;}

	Manager manager;

	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find("Manager").GetComponent<Manager>();
		isSelected = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SelectCard ()
	{
		if(!isSelected)
		{
			if(manager.selectedCards.Count < 3)
			{
				manager.selectedCards.Add(this.gameObject);
				isSelected = true;
				selectedImage.enabled = true;
			}
		}
		else
		{
			manager.selectedCards.Remove(this.gameObject);
			isSelected = false;
			selectedImage.enabled = false;
		}

	}
}
