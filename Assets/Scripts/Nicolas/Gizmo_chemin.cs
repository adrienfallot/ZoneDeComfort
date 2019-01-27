using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo_chemin : MonoBehaviour {

	private Transform chemin;
	private Transform[] noeuds;

	private void DrawLinesNodes (Transform chemin) {
		
		//Je définis la taille du chemin
		noeuds = new Transform[chemin.childCount];

		//Je check tous les noeuds du chemin
		for (int i = 0; i < chemin.childCount; i++) {
			noeuds [i] = chemin.GetChild (i);
		}

		for (int i = 1; i < chemin.childCount; i++) {
			//Je tracerais les traits en rouge
			Gizmos.color = Color.red;
			//Je trace le trait entre chaque points
			Gizmos.DrawLine (noeuds [i - 1].position, noeuds [i].position);
		}

		//Je trace le dernier trait (entre le dernier point et le premier)
		Gizmos.DrawLine (noeuds [chemin.childCount - 1].position, noeuds [0].position);
	}

	private void OnDrawGizmos() {

		if (this.tag == "Chemin") {
			//Je défini quel est le chemin concerné
			chemin = this.transform;
			DrawLinesNodes (chemin);
		}
		/*
		if (this.tag == "Noeud") {
			//Je défini quel est le chemin concerné
			chemin = this.transform.parent;
			DrawLinesNodes (chemin);
		}
		*/
	}
}