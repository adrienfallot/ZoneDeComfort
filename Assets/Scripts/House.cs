using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	public Animator dodoImage;
    public float stoptime;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AkSoundEngine.SetState("ST_Close_Home", "Close");
            DaysManager.StartDay();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AkSoundEngine.SetState("ST_Close_Home", "Affar");

            if (PlayerController.croissant)
            {
                other.GetComponent<PlayerController>().StopPlayer(stoptime);
                dodoImage.Play("Dodo", -1, 0f);

                PlayerController.croissant = false;
                DaysManager.dayNumber++;
                DaysManager.FinishDay();
            }
        }
    }
}
