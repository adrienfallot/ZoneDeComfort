using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ComfortZone : MonoBehaviour
{
    public PlayerController ThePlayer;

    public float scaleFactor;
    public int maxNbScale;
    private int nbScale;

    void OnTriggerExit2D(Collider2D other)
    {
        ThePlayer.nbComfortZones--;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ThePlayer.nbComfortZones++;
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
