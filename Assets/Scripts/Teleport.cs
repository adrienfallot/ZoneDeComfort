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
            Player.StopPlayer(TimerStopped);
            teleporting = true;
            teleported = false;
            if (this.tag == "Exit")
                AkSoundEngine.PostEvent("Env_Room_Leave", this.gameObject);
            if (this.tag == "Enter")
                AkSoundEngine.PostEvent("Env_Room_Enter", this.gameObject);

            Anim.Play("Dodo", -1, 0f);
        }
    }

    void Update()
    {
        //Je gère le timer si je téléporte depuis ici
        if (teleporting)
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
            teleporting = false;
        }

        //Je téléporte le perso et change la caméra à la moitié de l'anim (quand il fait tout noir)
        if (timer >= TimerStopped / 2f && !teleported)
        {
            Player.transform.position = target.gameObject.transform.position;

            if (this.tag == "Exit")
            {
                Coffee_Come_In.tempzoom = Player.zoomoutside;
                PlayerController.currentzoom = Player.zoomoutside;
                Camera.main.orthographicSize = PlayerController.currentzoom;
            }
            if (this.tag == "Enter")
            {
                Coffee_Come_In.tempzoom = Player.zoominside;
                PlayerController.currentzoom = Player.zoominside;
                Camera.main.orthographicSize = PlayerController.currentzoom;
            }

            teleported = true;
        }
    }
}
