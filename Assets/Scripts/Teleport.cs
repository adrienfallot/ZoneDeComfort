using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private Teleport target;
    [SerializeField]
    private PlayerController Player;
    [SerializeField]
    private float TimerStopped = 1f;
    [SerializeField]
    private Animator Anim;

    private float timer;
    private bool teleporting, teleported;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player.Stopped = true;
            teleporting = true;
            teleported = false;
            Anim.Play("Dodo", -1, 0f);
        }
    }

    void Update()
    {
        //Je gère le timer si je téléporte depuis ici
        if ((Player.Stopped) && (teleporting))
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
        }

        //Le timer est fini
        if (timer >= TimerStopped)
        {
            Player.Stopped = false;
            teleporting = false;
        }

        //Je téléporte le perso et change la caméra à la moitié de l'anim (quand il fait tout noir)
        if ((timer >= TimerStopped / 2f) && (!teleported))
        {
            Player.transform.position = target.gameObject.transform.position;

            if (this.tag == "Exit")
                Camera.main.orthographicSize = 10;
            if (this.tag == "Enter")
                Camera.main.orthographicSize = 5;

            teleported = true;
        }
    }
}
