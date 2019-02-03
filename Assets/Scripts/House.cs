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
            }
        }
    }
}
