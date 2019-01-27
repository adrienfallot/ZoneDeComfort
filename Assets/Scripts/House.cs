using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	public Animator dodoImage;

    void OnTriggerExit2D(Collider2D other)
    {
		AkSoundEngine.SetState("ST_Close_Home", "Close");
		DaysManager.StartDay();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		AkSoundEngine.SetState("ST_Close_Home", "Affar");
		
		if (PlayerController.croissant)
		{
			dodoImage.Play("Dodo");

			PlayerController.croissant = false;
			DaysManager.dayNumber ++;
			DaysManager.FinishDay();
		}
    }

}
