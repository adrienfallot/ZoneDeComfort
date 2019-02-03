using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coffee_Come_In : MonoBehaviour
{

    public NPCTalk Coffee_Owner;
    public Animator bulle;
    public Text textBulle;

    public float newzoom;
    public float speedzoom;
    private float tempzoom;

    private bool isTalking = false;
    private bool bullePop = false;
    private Camera LaCamera;

    private float timerzoom;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timerzoom = 0;
            tempzoom = LaCamera.orthographicSize;
            isTalking = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timerzoom = 0;
            tempzoom = LaCamera.orthographicSize;
            isTalking = false;
        }
    }

    void Talk()
    {
        if (isTalking)
        {
            if (Mathf.Sin(Time.time * Mathf.PI / 4) > 0)
            {
                if (!bullePop)
                {
                    AkSoundEngine.PostEvent("Env_Greet_Nice", this.gameObject); //TODO: comfort/discomfort
                    bulle.Play("Pop");
                    bullePop = true;
                }
            }
            else
            {
                if (bullePop)
                {
                    bulle.Play("Unpop");
                    bullePop = false;
                }
            }
        }
    }

    private void Start()
    {
        LaCamera = Camera.main;
        tempzoom = Camera.main.orthographicSize;
    }

    void DeZoom()
    {
        if (isTalking && timerzoom <= 1)
        {
            LaCamera.orthographicSize = Mathf.Lerp(tempzoom, newzoom, timerzoom);
            timerzoom += Time.deltaTime * speedzoom;
        }
    }

    void Zoom()
    {
        if (!isTalking && timerzoom <= 1)
        {
            LaCamera.orthographicSize = Mathf.Lerp(tempzoom, PlayerController.currentzoom, timerzoom);
            timerzoom += Time.deltaTime * speedzoom;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Si j'ai déjà parlé au coffee owner j'affiche plus la boîte
        if (Coffee_Owner.numberText > 0)
        {
            this.gameObject.SetActive(false);
        }

        Talk();
        Zoom();
        DeZoom();
    }
}
