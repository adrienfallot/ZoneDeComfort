using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour 
{
	public Teleport target;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.transform.position = target.gameObject.transform.position;

            if (this.tag == "Exit")
                Camera.main.orthographicSize = 10;
            if (this.tag == "Enter")
                Camera.main.orthographicSize = 5;
        }
    }
}
