using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{
	public Animator bulle;
	public Text textBulle;

	[System.Serializable]
	public struct Day
	{
		[Multiline]
		public string greeting;
        public Dialogue[] dialogue;
		[Multiline]
		public string bye;
	}

	[System.Serializable]
	public struct Dialogue
	{
		[Multiline]
        public string text;
	}

	[Header("Texts")]
	public Day[] days;


    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.CompareTag("Player"))
		{
			textBulle.text = days[DaysManager.dayNumber].greeting;
			bulle.Play("Pop");
		}
	}

	void OnTriggerExit2D(Collider2D other)
    {
		if (other.CompareTag("Player"))
		{
			bulle.Play("Unpop");
		}
    }

}
