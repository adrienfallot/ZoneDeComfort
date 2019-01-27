using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour 
{
	public Teleport target;

	void OnTriggerEnter2D(Collider2D other)
    {
		print(other.gameObject.transform.position);
		print(target.gameObject.transform.position);

		other.gameObject.transform.position = target.gameObject.transform.position;
    }
}
