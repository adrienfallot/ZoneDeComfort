using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe_sound : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AkSoundEngine.SetState("ST_Close_Home", "Close");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AkSoundEngine.SetState("ST_Close_Home", "Affar");
        }
    }
}
