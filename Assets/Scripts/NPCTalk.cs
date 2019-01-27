using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{
	public Animator bulle;
	public Text textBulle;

	private bool isTalking = false;
	private bool bye = false;
	private bool bullePop = false;

	private int numberText = 0;

	[System.Serializable]
	public struct Day
	{
		[Multiline]
		public string greeting;
		[Multiline]
        public string[] dialogue;
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
			if(!bye)
			{
				AkSoundEngine.PostEvent("Env_Greet_Nice", this.gameObject); //TODO: comfort/discomfort

				textBulle.text = days[DaysManager.dayNumber].greeting;
				bulle.Play("Pop");
				bullePop = true;
			}
			
			isTalking = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
    {
		if (other.CompareTag("Player"))
		{
			bulle.Play("Unpop");

			isTalking = false;
			bullePop = false;

			if(!bye)
			{
				numberText = 0;
			}
		}
    }

	 // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Interact") && isTalking)
		{
			if (!bye)
			{
				textBulle.text = days[DaysManager.dayNumber].dialogue[numberText];
				numberText ++;
				bye = days[DaysManager.dayNumber].dialogue.Length == numberText;
			}
			else
			{
				textBulle.text = days[DaysManager.dayNumber].bye;

				if (!bullePop)
				{
					bulle.Play("Pop");
					bullePop = true;
				}
			}
		}
	}

	public void FinishDay()
	{
		bulle.Play("Unpop");

		isTalking = false;
		bullePop = false;
		bye = false;
		numberText = 0;
	}

}
