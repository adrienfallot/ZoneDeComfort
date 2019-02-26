using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfTown : MonoBehaviour
{
    public GameObject Ending_Out, Ending_Friend;
    [SerializeField]
    PlayerController thePlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Si je suis parti pour avoir la fin en sortant de la ville avec un ami
            if (PlayerController.IsWithFriend)
            {
                thePlayer.StopPlayer(100f);
                Ending_Friend.GetComponent<Animator>().Play("Pop", -1);
            }
            //Si je suis parti pour avoir la fin en sortant de la ville seul
            else
            {
                thePlayer.StopPlayer(100f);
                Ending_Out.GetComponent<Animator>().Play("Pop", -1);
            }
        }
    }
}
