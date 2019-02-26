using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_UI : MonoBehaviour
{
    [SerializeField]
    PlayerController thePlayer;
    [SerializeField]
    float timeFade, timeappear;

    private float timer;
    public bool MovePlayer;

    // Use this for initialization
    void Start()
    {
        thePlayer.StopPlayer(timeFade + 1f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeFade)
        {
            GetComponent<Animator>().Play("Unpop", -1);
            if (MovePlayer)
            {
                thePlayer.Stopped = false;
                this.enabled = false;
            }
        }
    }
}
