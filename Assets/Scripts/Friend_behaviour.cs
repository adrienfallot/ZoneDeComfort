using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friend_behaviour : MonoBehaviour
{
    [SerializeField]
    GameObject thePlayer;
    [SerializeField]
    float closeToPlayer, tooFar;
    [SerializeField]
    SpriteRenderer LeSpriteFriend;
    [SerializeField]
    float speed;
    [SerializeField]
    PanicMessages[] Messages_Paniqués;
    [SerializeField]
    Text texte_paniqué;
    [SerializeField]
    public GameObject leCanvas;

    [System.Serializable]
    public struct PanicMessages
    {
        [Multiline]
        public string panicking;
        [Multiline]
        public string thanks;
    }

    [HideInInspector]
    public bool walking = false;
    private Animator NPCAnimator;
    private Vector3 directionfriend;
    [HideInInspector]
    public bool panic, startpanic, stoppanic, isTalking;
    private int panicid;

    // Use this for initialization
    void Start()
    {
        NPCAnimator = LeSpriteFriend.GetComponent<Animator>();
    }    

    void CalculateDirection()
    {
        //Je détermine le sens le plus adapté au mouvement en cours
        directionfriend = Vector3.Normalize(thePlayer.transform.position - transform.position);
    }

    void ChooseAnimationRun()
    {

        //Si mon personnage va globalement dans le sens horizontal
        if (Mathf.Abs(directionfriend.x) >= Mathf.Abs(directionfriend.y))
        {
            NPCAnimator.SetBool("Horizontal", false);
            //S'il va vers la droite je le flippe pas, sinon (il va à gauche, je le flippe)
            if (directionfriend.x >= 0)
                LeSpriteFriend.flipX = false;
            else
                LeSpriteFriend.flipX = true;
        }
        else
        {
            NPCAnimator.SetBool("Horizontal", true);
        }
    }

    void DoMove()
    {
        //Si je suis loin du personnage, et que je panique pas j'avance vers lui
        if (IsCloseToPlayer() || panic)
            walking = false;
        else
            walking = true;

        //Si le perso marche
        if (walking)
        {
            //Je lui dis de faire l'anim de course
            NPCAnimator.SetBool("Walking", walking);
            ChooseAnimationRun();
        }
        else
            NPCAnimator.SetBool("Walking", walking);
    }

    private void WalkToPlayer()
    {
        if (walking == true)
        {
            CalculateDirection();
            transform.position += directionfriend * speed;
        }
    }

    void Panicking()
    {
        //Si le joueur est trop loin, il s'arrête et panique
        if (!panic && (Vector3.Distance(thePlayer.transform.position, transform.position) >= tooFar))
        {
            //Je choisis un dialogue aléatoire
            panicid = Random.Range(0, Messages_Paniqués.Length);
            panic = true;
            startpanic = true;
        }

        //S'il panique, il affiche un message de panique, si le joueur lui parle, alors il arrête de paniquer
        if (panic)
        {
            //J'affiche la bulle de stress
            if (startpanic)
            {
                texte_paniqué.text = Messages_Paniqués[panicid].panicking;
                leCanvas.GetComponent<Animator>().Play("Pop", -1);
                startpanic = false;
            }

            //Si le joueur lui parle, ça le calme
            if (Input.GetButtonDown("Interact") && isTalking)
            {
                texte_paniqué.text = Messages_Paniqués[panicid].thanks;
                leCanvas.GetComponent<Animator>().Play("Pop", -1);
                stoppanic = true;
            }
        }
    }

    bool IsCloseToPlayer()
    {
        //Si je suis proche du joueur, je bouge plus et passe en idle
        if (Vector3.Distance(thePlayer.transform.position, transform.position) <= closeToPlayer)
        {
            NPCAnimator.SetBool("Walking", false);
            return true;
        }
        else
            return false;
    }

    // Update is called once per frame
    void Update()
    {
        DoMove();
        WalkToPlayer();
        Panicking();

        if (!panic)
            PlayerController.IsWithFriend = true;
        else
            PlayerController.IsWithFriend = false;

    }
}