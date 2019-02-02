using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sport_Guy : MonoBehaviour
{
    public Transform[] keypos;
    public SpriteRenderer LeSprite;
    public float speed;
    public float maxidle;

    [HideInInspector]
    public bool walking = false;
    private Vector3 directionRun;
    private Animator NPCAnimator;
    private float timer, timeidle;

    private int previouspoint, nextpoint;
    private float progress;
    private bool senspositif = true;

    // Use this for initialization
    void Start()
    {
        //Je place le perso à son premier point
        transform.position = keypos[0].position;
        //Je récupère son animator
        NPCAnimator = GetComponent<NPCTalk>().NPCAnimator;
        //j'évite des bugs à la con de trucs mals settés
        nextpoint = 1;
        previouspoint = 0;
    }

    //Le perso est immobile et fait une pause à son point puis repart + anim idle
    void DoIdle()
    {
        if (!walking)
        {
            //Je lui dis de faire l'anim d'idle
            NPCAnimator.SetBool("Walking", walking);
            //J'augmente le timer
            timer += Time.deltaTime;

            //S'il est resté assez longtemps en idle
            if (timer >= timeidle)
            {
                //Il repart courir
                walking = true;
                //Je reset le timer et je choisis prochain temps d'idle
                timer = 0f;
                timeidle = Random.Range(0.5f, maxidle);
            }
        }
    }

    void ChooseAnimationRun()
    {
        //Je lance la bonne animation
        directionRun = keypos[nextpoint].position - keypos[previouspoint].position;

        //S'il va selon l'axe x
        if (directionRun.x != 0)
        {
            NPCAnimator.SetBool("Horizontal", false);
            //S'il va vers la droite je le flippe pas, sinon (il va à gauche, je le flippe)
            if (directionRun.x >= 0)
            {
                LeSprite.flipX = false;
            }
            else
            {
                LeSprite.flipX = true;
            }
        }
        else
        {
            //Il va de haut en bas
            NPCAnimator.SetBool("Horizontal", true);
        }
    }

    void DoRun()
    {
        //Si le perso marche
        if (walking)
        {
            //Je lui dis de faire l'anim de course
            NPCAnimator.SetBool("Walking", walking);
            ChooseAnimationRun();
            //Je le déplace vers son prochain point
            transform.position = Vector3.Lerp(keypos[previouspoint].position, keypos[nextpoint].position, progress);
            //Je dis que le perso a progressé un peu
            progress += speed / Vector3.Distance(keypos[nextpoint].position, keypos[previouspoint].position);
            //S'il est proche du point je l'arrête
            if (IsCloseToPoint())
            {
                walking = false;
            }
        }
    }

    bool IsCloseToPoint()
    {
        //Si je suis proche du point de destination, je m'y snap et je change de point de destination
        if (Vector3.Distance(keypos[nextpoint].position, transform.position) <= 0.1f)
        {
            transform.position = keypos[nextpoint].position;
            ChangePoint();
            return true;
        }
        else
            return false;
    }

    void ChangePoint()
    {
        //Je reset mon progrès
        progress = 0;
        //L'aller
        if (senspositif)
        {
            //Si je suis au bout de l'aller
            if (nextpoint == keypos.Length - 1)
            {
                senspositif = false;
                previouspoint = nextpoint;
                nextpoint = keypos.Length - 1;
            }
            else //Sinon je change les points
            {
                previouspoint = nextpoint;
                nextpoint++; 
            }
        }

        //Le retour
        if (!senspositif)
        {
            //Si je suis au bout du retour
            if (nextpoint == 0)
            {
                senspositif = true;
                previouspoint = nextpoint;
                nextpoint = 1;
            }
            else //Sinon je change les points
            {
                previouspoint = nextpoint;
                nextpoint--;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DoIdle();
        DoRun();
    }
}
