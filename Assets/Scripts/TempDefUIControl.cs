using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TempDefUIControl : MonoBehaviour {

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
				mySlider.maxValue = player.GetComponent<Player>().maxTempDef;
				first = false;
			}

			mySlider.value = player.GetComponent<Player>().GetTempDefence();
		}
	}
}
