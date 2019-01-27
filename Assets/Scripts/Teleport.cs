using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour 
{
	public Teleport target;

	void OnTriggerEnter2D(Collider2D other)
    {
		other.gameObject.transform.position = target.gameObject.transform.position;
    }
}
