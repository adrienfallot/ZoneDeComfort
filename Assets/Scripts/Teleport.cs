using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private Transform target;
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
            Player.transform.position = target.position;

            //Si j'ai activé la quête de l'ami
            if (PlayerController.friendquest)
            {
                //J'active l'ami qui suit, je désactive l'ami qui suit pas
                Player.Coffee_friend.SetActive(true);
                Player.Coffee_owner.gameObject.SetActive(false);
            }

            if (CompareTag("Exit"))
            {
                Coffee_Come_In.tempzoom = Player.zoomoutside;
                PlayerController.currentzoom = Player.zoomoutside;
                Camera.main.orthographicSize = PlayerController.currentzoom;
            }
            if (CompareTag("Enter"))
            {
                Coffee_Come_In.tempzoom = Player.zoominside;
                PlayerController.currentzoom = Player.zoominside;
                Camera.main.orthographicSize = PlayerController.currentzoom;
            }

            teleported = true;
        }
    }
}
