using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{
    public Animator bulle;
    public Text textBulle;
    public Animator NPCAnimator;
    public Font NormalFont, IncomfortFont;

    public ComfortZone[] comfortZones;

    private bool isTalking = false;
    [HideInInspector]
    public bool bye = false;
    private bool bullePop = false;

    [HideInInspector]
    public int numberText = 0;
    //[HideInInspector]
    public int numberDialogue = 0;

    public static List<NPCTalk> NPC;

    public bool[] hasACroissant = new bool[7];

    private PlayerController ThePlayer;

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
            if (!bye)
            {
                if (!ThePlayer.isDiscomfort)
                    AkSoundEngine.PostEvent("Env_Greet_Nice", this.gameObject); //TODO: comfort/discomfort
                if (ThePlayer.isDiscomfort)
                    AkSoundEngine.PostEvent("Env_Greet_Mean", this.gameObject);

                textBulle.text = " " + days[numberDialogue].greeting + " ";
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

            if (!bye)
            {
                numberText = 0;
            }
        }
    }

    void SetFont()
    {
        //Si je suis en zone de confort
        if (!ThePlayer.isDiscomfort)
        {
            textBulle.font = NormalFont;
        }
        else //Si je suis en zone d'inconfort
        {
            textBulle.font = IncomfortFont;
        }
    }

    void Awake()
    {
        ThePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (NPC == null)
        {
            NPC = new List<NPCTalk>();
        }

        NPC.Add(this);

        NPCAnimator.SetBool("Walking", hasACroissant[numberDialogue]); //croissant
    }

    // Update is called once per frame
    void Update()
    {
        SetFont();

        if (Input.GetButtonDown("Interact") && isTalking)
        {
            Day today = days[numberDialogue];

            if (!bye)
            {
                if (today.dialogue.Length == 0)
                {
                    textBulle.text = " " + today.greeting + " ";
                }
                else
                {
                    textBulle.text = " " + today.dialogue[numberText] + " ";
                    numberText++;
                    bye = today.dialogue.Length == numberText;
                }
            }
            else
            {
                if (hasACroissant[numberDialogue])
                {
                    hasACroissant[numberDialogue] = false;
                    NPCAnimator.SetBool("Walking", false); //croissant
                    PlayerController.croissant = true;
                    AkSoundEngine.PostEvent("P_Success", gameObject);
                }

                textBulle.text = " " + today.bye + " ";

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

        //Si j'ai parlé totalement à la personne
        if (bye)
        {
            //Je m'assure de pas dépasser le nombre de jour total du perso
            if (numberDialogue != days.Length - 1)
                numberDialogue++;

            if (hasACroissant[numberDialogue])
                NPCAnimator.SetBool("Walking", true); //croissant

            //J'augmente la taille de la zone de confort
            foreach (var comfortZone in comfortZones)
            {
                comfortZone.Expend();
            }
        }

        isTalking = false;
        bullePop = false;
        bye = false;
        numberText = 0;
    }

}
