using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ComfortZone : MonoBehaviour
{
    public float scaleFactor;
    public int maxNbScale;
    private int nbScale;

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

    public void Expend()
    {
        if(nbScale == maxNbScale){
            return;
        }

        Vector3 scale = this.transform.localScale;
        scale += new Vector3(scaleFactor, scaleFactor, 0.0f);
        this.transform.localScale = scale;

        nbScale++;
    }
}
