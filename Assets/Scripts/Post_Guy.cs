using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Post_Guy : MonoBehaviour
{
    private NPCTalk Talk_Post;
    private int oldday;
    private bool oncequest1, oncequest2, oncepackage;

    // Use this for initialization
    void Start()
    {
        Talk_Post = GetComponent<NPCTalk>();
        //J'initialise à 1 pour sauter le dialogue de quête
        Talk_Post.numberDialogue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Si le joueur a la quête je passe au premier dialogue sinon je reprends là où il en était avant
        if (PlayerController.hasQuest && !oncequest1)
        {
            //J'enregistre le jour où il va falloir reprendre après
            oldday = Talk_Post.numberDialogue;
            Talk_Post.numberDialogue = 0;
            Talk_Post.bye = false;
            Talk_Post.numberText = 0;
            oncequest1 = true;
        }

        //Si j'ai la quête, que je lui parle et que je vais au bout du dialogue de parcel
        if (PlayerController.hasQuest && !oncequest2 && Talk_Post.bye)
        {
            PlayerController.hasPackage = true;
            oncequest2 = true;
        }

        //Si le perso a déjà le colis, je remet le postier au bon dialogue une fois pour toutes
        if (PlayerController.hasPackage && !oncepackage && Talk_Post.numberDialogue > 0)
        {
            Talk_Post.numberDialogue = oldday;
            oncepackage = true;
        }
    }
}
