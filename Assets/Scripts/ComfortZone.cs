using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComfortZone : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        PlayerController.isDiscomfort = true;
        PlayerController.timerDiscomfort = 0.0f;
        AkSoundEngine.SetState("ST_Player_Confort", "No");
        AkSoundEngine.PostEvent("Amb_Creep", this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController.isDiscomfort = false;
        AkSoundEngine.SetState("ST_Player_Confort", "Yes");
    }
}
