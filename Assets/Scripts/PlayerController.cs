using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rigidBody2D;
    private bool moving = false;

    void Start()
    {
        rigidBody2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        print(move);

        rigidBody2D.velocity = move * speed;

        if((move.x != 0.0f || move.y != 0.0f) && !moving)
        {
            moving = true;
            AkSoundEngine.PostEvent("P_Walk", this.gameObject);
        }
        else if (move.x == 0.0f && move.y == 0.0f && moving)
        {
            moving = false;
            AkSoundEngine.PostEvent("P_Walk_Stop", this.gameObject);
        }

    }
}
