using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComfortZone : MonoBehaviour
{
    public GameObject comfortImage;

    void OnTriggerExit2D(Collider2D other)
    {
        comfortImage.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        comfortImage.SetActive(false);
    }
}
