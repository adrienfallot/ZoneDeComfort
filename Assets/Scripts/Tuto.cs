using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto : MonoBehaviour
{

    public GameObject Tuto_text;
    public NPCTalk Hi_Guy;

    // Update is called once per frame
    void Update()
    {
       //Si le Hi_guy est arrivé à son au revoir, alors j'enlève le tuto
       if (Hi_Guy.numberDialogue > 0)
        {
            Tuto_text.SetActive(false);
        }
    }
}
