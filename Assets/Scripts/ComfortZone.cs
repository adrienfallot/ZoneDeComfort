using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ComfortZone : MonoBehaviour
{
    public PlayerController ThePlayer;

    public float scaleFactor;
    public int maxNbScale;
    //[HideInInspector]
    public int nbScale;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ThePlayer.nbComfortZones--;

        if (other.CompareTag("NPC"))
            other.GetComponentInParent<NPCTalk>().IsInDiscomfortZone = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ThePlayer.nbComfortZones++;

        if (other.CompareTag("NPC"))
            other.GetComponentInParent<NPCTalk>().IsInDiscomfortZone = false;
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
