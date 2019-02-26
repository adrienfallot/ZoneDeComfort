using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanTalk : MonoBehaviour
{
    [SerializeField]
    Friend_behaviour theFriend;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            theFriend.isTalking = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Si le joueur avait parlé au perso alors qu'il paniquait, alors il arrête de paniquer maintenant
            if (theFriend.stoppanic)
            {
                theFriend.panic = false;
                theFriend.leCanvas.GetComponent<Animator>().Play("Unpop", -1);
            }

            theFriend.isTalking = false;
        }
    }

}
