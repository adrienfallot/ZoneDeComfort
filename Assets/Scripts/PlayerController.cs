using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rigidBody2D;

    [Header("Zone")]
    public Transform[] comfortZone;

    [Header("Sprite")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public static bool croissant = false;
    public static bool hasQuest = false;
    public static bool hasPackage = false;
    public bool isDiscomfort = false;
    public bool previousisDiscomfort = false;
    public bool cured = false;

    public float timerDiscomfort = 0.0f;
    public float discomfortPercentage = 0f;
    public float timerDiscomfortMax = 10.0f;
    public float timerDiscomfortMin = 20.0f;
    public GameObject OwnComfort;

    public int nbComfortZones = 0;
    private float closeZone;

    public PostProcessingBehaviour postProcessing;
    private PostProcessingProfile discomfortProfile;

    void Start()
    {
        AkSoundEngine.SetSwitch("SW_Game_Status", "Game", this.gameObject);

        rigidBody2D = this.GetComponent<Rigidbody2D>();

        discomfortProfile = postProcessing.profile;
    }

    void IsInComfortZone()
    {
        if (nbComfortZones > 0)
            isDiscomfort = false;
        else
            isDiscomfort = true;
    }

    void StartedDiscomfort()
    {
        //Si j'ai pas bougé il se passe rien
        if (previousisDiscomfort == isDiscomfort)
            return;
        else
        //Si ça a bougé et que je suis en état d'inconfort, je viens de passer dans cet état, donc je reset le timer
            if (isDiscomfort)
            timerDiscomfort = 0.0f;

        //Je save l'état d'inconfort
        previousisDiscomfort = isDiscomfort;
    }

    void SetSound()
    {
        if (isDiscomfort)
        {
            AkSoundEngine.SetState("ST_Player_Confort", "No");
            AkSoundEngine.PostEvent("Amb_Creep", this.gameObject);
        }
        else
        {
            AkSoundEngine.SetState("ST_Player_Confort", "Yes");
        }
    }

    void MoveCharacter()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        rigidBody2D.velocity = move * speed;

        if ((move.x != 0.0f || move.y != 0.0f))
        {
            if (move.x < 0.0f)
            {
                spriteRenderer.flipX = false;
            }
            else if (move.x > 0.0f)
            {
                spriteRenderer.flipX = true;
            }

            if (!animator.GetBool("Walking"))
            {
                animator.SetBool("Walking", true);
                AkSoundEngine.PostEvent("P_Walk", this.gameObject);
            }
        }
        else if (move.x == 0.0f && move.y == 0.0f && animator.GetBool("Walking"))
        {
            animator.SetBool("Walking", false);
            AkSoundEngine.PostEvent("P_Walk_Stop", this.gameObject);
        }
    }

    void IncreaseTimerDiscomfort()
    {
        if (isDiscomfort)
            timerDiscomfort += Time.deltaTime;
    }

    void DistComfortZone()
    {
        //Pour déterminer la distance la plus faible séparant d'une limite de zone
        closeZone = Mathf.Infinity;

        foreach (Transform zone in comfortZone)
        {
            //La distance est celle entre le joueur et le centre de la zone moins son rayon
            float dist = Vector3.Distance(zone.position, this.transform.position) - (zone.GetComponent<CircleCollider2D>().radius * zone.transform.lossyScale.x);
            closeZone = (dist < closeZone ? dist : closeZone);
        }

        //Si des imprécisions font que la valeur est négative, je l'arrondie à 0
        if (closeZone < 0)
            closeZone = 0;

        AkSoundEngine.SetRTPCValue("RTPC_D_Closest_Zone", discomfortPercentage);
    }

    void DiscomfortPercentage()
    {
        //Si je suis en phase croissante d'anxiété
        if (timerDiscomfort <= timerDiscomfortMax)
        {
            discomfortPercentage = (timerDiscomfort / timerDiscomfortMax) * 100;
            discomfortPercentage = Mathf.Clamp(Mathf.Max(discomfortPercentage, closeZone), 0, 100);
        }

        //Si je suis en phase décroissante d'anxiété
        if ((timerDiscomfort >= timerDiscomfortMax) && (timerDiscomfort <= timerDiscomfortMin))
        {
            discomfortPercentage = 100 - (((timerDiscomfort - timerDiscomfortMax) / (timerDiscomfortMin - timerDiscomfortMax)) * 100);

            if (discomfortPercentage < 0)
                discomfortPercentage = 0;
        }

        //Si je suis au bout du timer je suis guéri
        if (timerDiscomfort >= timerDiscomfortMin)
            cured = true;
    }

    void CuredFinally ()
    {
        if (cured)
        {
            postProcessing.enabled = false;
            OwnComfort.SetActive(true);
        }
    }

    void PostProcessIncomfort()
    {
        if (isDiscomfort)
        {
            postProcessing.enabled = true;

            var grain = discomfortProfile.grain.settings;
            var vignette = discomfortProfile.vignette.settings;

            grain.intensity = (discomfortPercentage / 100);
            vignette.intensity = (discomfortPercentage / 100);

            discomfortProfile.grain.settings = grain;
            discomfortProfile.vignette.settings = vignette;
        }
        else
        {
            postProcessing.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Je check si le joueur est dans une comfort zone
        IsInComfortZone();
        //Je met le son bien en fonction de son état
        SetSound();

        //Je bouge le perso selon les inputs
        MoveCharacter();

        //Je détermine la distance jusqu'à la prochaine zone de confort
        DistComfortZone();

        //Je modifie l'effet de postprocessing correctement
        PostProcessIncomfort();

        //Je check si je viens de passer en inconfort
        StartedDiscomfort();

        //Si je suis en inconfort, j'augmente l'état d'inconfort
        IncreaseTimerDiscomfort();

        //Je calcule le pourcentage d'effet d'anxiété
        DiscomfortPercentage();

        //Est-ce que le perso est guéri?
        CuredFinally();
    }

    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
