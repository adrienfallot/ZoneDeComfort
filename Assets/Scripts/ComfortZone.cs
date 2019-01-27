using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComfortZone : MonoBehaviour
{
    public GameObject discomfortImage;

    void OnTriggerExit2D(Collider2D other)
    {
        discomfortImage.SetActive(true);
        AkSoundEngine.SetState("ST_Player_Confort", "No");
        AkSoundEngine.PostEvent("Amb_Creep", this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        discomfortImage.SetActive(false);
        AkSoundEngine.SetState("ST_Player_Confort", "Yes");
    }
}
