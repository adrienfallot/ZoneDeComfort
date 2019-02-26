using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	public Animator dodoImage;
    public float stoptime;
    [SerializeField]
    PlayerController thePlayer;
    [SerializeField]
    GameObject Ending_comfortmax;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            DaysManager.StartDay();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (PlayerController.croissant)
            {
                other.GetComponent<PlayerController>().StopPlayer(stoptime);
                dodoImage.Play("Dodo", -1, 0f);

                AkSoundEngine.PostEvent("P_Sleep", this.gameObject);

                PlayerController.croissant = false;
                DaysManager.dayNumber++;
                DaysManager.FinishDay();

                //Si je suis au confort max, j'affiche la fin de confort max
                if (IsMaxComfort())
                {
                    Debug.Log("Full_comfort");
                    Ending_comfortmax.GetComponent<Animator>().Play("PopUnpop", -1);
                }
            }
        }
    }

    //Je vérifie si le joueur est à son comfort maximal
    bool IsMaxComfort ()
    {
        //Je regarde si toutes les zones de confort sont pleines
        foreach (Transform zone in thePlayer.comfortZone)
        {
            if (zone.GetComponent<ComfortZone>().maxNbScale != zone.GetComponent<ComfortZone>().nbScale)
                return false;
        }

        //Toutes les zones de comfort sont pleines donc je suis au confort max
        return true;
    }
}
