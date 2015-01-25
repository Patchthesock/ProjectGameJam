using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class CardProperties : MonoBehaviour
{
	public Sprite cardImage;
	public Texture cardImageTexture;

	public string cardName;

	public bool defenceCard;
	public bool utilityCard;
	public int attack;
	public int defence;
	public int stamina;
	public int posture;
	public int diceValue;
	public int defenceValue;
	public int AttackBonus;
}
