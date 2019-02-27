using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dezoom_end : MonoBehaviour
{

    [SerializeField]
    private PlayerController ThePlayer;

    public float newzoomend;
    public float speedzoomend;
    [HideInInspector]
    public static float tempzoomend;

    private Camera LaCamera;

    private float timerzoomend;
    private bool zoom = true;

    //Le joueur entre dans la zone, je vais dézoomer
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timerzoomend = 0;
            tempzoomend = LaCamera.orthographicSize;
            zoom = false;
        }
    }

    //Le joueur sors de la zone je vais zoomer
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timerzoomend = 0;
            if (!ThePlayer.Stopped) //Pour éviter le problème du café
                tempzoomend = LaCamera.orthographicSize;
            zoom = true;
        }
    }    

    private void Start()
    {
        LaCamera = Camera.main;
        tempzoomend = PlayerController.currentzoom;
    }

    void DeZoom()
    {
        if (!zoom && timerzoomend <= 1)
        {
            LaCamera.orthographicSize = Mathf.Lerp(tempzoomend, newzoomend, timerzoomend);
            timerzoomend += Time.deltaTime * speedzoomend;
        }
    }

    void Zoom()
    {
        if (zoom && timerzoomend <= 1)
        {
            LaCamera.orthographicSize = Mathf.Lerp(tempzoomend, PlayerController.currentzoom, timerzoomend);
            timerzoomend += Time.deltaTime * speedzoomend;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        DeZoom();
    }
}