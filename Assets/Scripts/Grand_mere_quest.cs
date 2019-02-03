using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grand_mere_quest : MonoBehaviour
{

    public int DayQuest;
    public Transform TP_GrandMere;
    public RectTransform Bulle;

    private NPCTalk GrandMereTalk;
    private bool oncepackage;

    private void Start()
    {
        GrandMereTalk = GetComponent<NPCTalk>();
    }

    // Update is called once per frame
    void Update()
    {
        //Si c'est le bon jour et que j'ai commencé à parler à la grand-mère
        if (GrandMereTalk.numberDialogue + 1 == DayQuest && GrandMereTalk.numberText >= 1)
        {
            //Active la quête
            PlayerController.hasQuest = true;
        }

        //Une fois que le joueur a le paquet, je TP la grand-mère dans sa maison etje la fais passer au dialogue du jour suivant
        if (PlayerController.hasPackage && !oncepackage)
        {
            transform.position = TP_GrandMere.position;
            Bulle.localPosition = Vector3.up * 3;
            Bulle.localScale = Vector3.one * 0.008f;
            GrandMereTalk.numberDialogue += 2;
            GrandMereTalk.numberText = 0;
            GrandMereTalk.bye = false;
            oncepackage = true;
        }

        //Si j'ai la quête mais pas le paquet, je boucle le jour suivant la quête et j'affiche pas les dialogues de remerciement
        if (GrandMereTalk.numberDialogue + 1 == DayQuest + 1)
        {
            //Si le joueur a le paquet, je passe au jour suivant
            if (PlayerController.hasPackage)
            {
                GrandMereTalk.numberDialogue++;
                GrandMereTalk.numberText = 0;
                GrandMereTalk.bye = false;
                oncepackage = true;
            }
            else //S'il l'a pas je fait boucler le jour
                if (GrandMereTalk.numberDialogue + 1 == DayQuest + 2)
            {
                GrandMereTalk.numberDialogue = DayQuest;
            }
        }
    }
}
