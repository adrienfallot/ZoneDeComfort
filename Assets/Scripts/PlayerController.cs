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
    public static bool isDiscomfort = false;

    public static float timerDiscomfort = 0.0f;
    public float timerDiscomfortMax = 10.0f;

    public PostProcessingBehaviour postProcessing;
    private PostProcessingProfile discomfortProfile;

    void Start()
    {
        AkSoundEngine.SetSwitch("SW_Game_Status", "Game", this.gameObject);

        rigidBody2D = this.GetComponent<Rigidbody2D>();

        discomfortProfile = postProcessing.profile;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        rigidBody2D.velocity = move * speed;

        if ((move.x != 0.0f || move.y != 0.0f) && !animator.GetBool("Walking"))
        {
            if (move.x < 0.0f)
            {
                spriteRenderer.flipX = false;
            }
            else if (move.x > 0.0f)
            {
                spriteRenderer.flipX = true;
            }

            animator.SetBool("Walking", true);
            AkSoundEngine.PostEvent("P_Walk", this.gameObject);
        }
        else if (move.x == 0.0f && move.y == 0.0f && animator.GetBool("Walking"))
        {
            animator.SetBool("Walking", false);
            AkSoundEngine.PostEvent("P_Walk_Stop", this.gameObject);
        }

        float closeZone = Mathf.Infinity;

        foreach (Transform zone in comfortZone)
        {
            float dist = Vector3.Distance(zone.position, this.transform.position);
            closeZone = (dist < closeZone ? dist : closeZone);
        }
        AkSoundEngine.SetRTPCValue("RTPC_D_Closest_Zone", closeZone);

        if (isDiscomfort)
        {
            postProcessing.enabled = true;

            var grain = discomfortProfile.grain.settings;
            var vignette = discomfortProfile.vignette.settings;

            if (timerDiscomfort <= timerDiscomfortMax)
            {
                grain.intensity = Mathf.Min(Map(closeZone, 0.0f, 15.0f, 0.0f, 1.0f), 1);
                vignette.intensity = Mathf.Min(Map(closeZone, 0.0f, 15.0f, 0.0f, 1.0f), 1);
            }
            else
            {
                grain.intensity = Mathf.Max(0, grain.intensity - 0.001f);
                vignette.intensity = Mathf.Max(0, vignette.intensity - 0.001f);
            }

            discomfortProfile.grain.settings = grain;
            discomfortProfile.vignette.settings = vignette;

        }
        else
        {
            postProcessing.enabled = false;
        }
    }

    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }

    void FixedUpdate()
    {
        timerDiscomfort += 0.01f;
    }
}
