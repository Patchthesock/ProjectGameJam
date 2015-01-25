using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StaminaUIControl : MonoBehaviour {

	public GameObject player; 
	
	private Slider mySlider;
	private bool first = true;

	void Start () {
		mySlider = this.GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(player)
		{
			if(first)
			{
				mySlider.maxValue = player.GetComponent<Player>().maxStamina;
				first = false;
			}
			mySlider.value = player.GetComponent<Player>().GetStamina();
		}
	}
}
