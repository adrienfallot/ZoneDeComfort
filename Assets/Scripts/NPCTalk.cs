using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{
	public Animator bulle;
	public Text textBulle;
	public Animator NPCAnimator;

	private bool isTalking = false;
	private bool bye = false;
	private bool bullePop = false;

	private int numberText = 0;
	private int numberDialogue = 0;

	public static List<NPCTalk> NPC;

	[System.Serializable]
	public struct Day
	{
		public bool hasACroissant;

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

				textBulle.text = days[numberDialogue].greeting;
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

	void Awake()
	{
		if(NPC == null){
			NPC = new List<NPCTalk>();
		}

		NPC.Add(this);

		NPCAnimator.SetBool("Walking", days[numberDialogue].hasACroissant); //croissant
	}

	// Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Interact") && isTalking)
		{
			Day today = days[numberDialogue];

			if (!bye)
			{
				if (today.dialogue.Length == 0)
				{
					textBulle.text = today.greeting;
				}
				else
				{
					textBulle.text = today.dialogue[numberText];
					numberText ++;
					bye = today.dialogue.Length == numberText;
				}
			}
			else
			{
				if (today.hasACroissant){
					today.hasACroissant = false;
					NPCAnimator.SetBool("Walking", false); //croissant
					PlayerController.croissant = true;
				}

				textBulle.text = today.bye;

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

		if (bye)
		{
			numberDialogue++;
		}

		isTalking = false;
		bullePop = false;
		bye = false;
		numberText = 0;
	}

}
