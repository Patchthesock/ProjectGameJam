using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthUIControl : MonoBehaviour {

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
				mySlider.maxValue = player.GetComponent<Player>().maxHealth;
				first = false;
			}

			mySlider.value = player.GetComponent<Player>().GetHealth();
		}
	}
}
