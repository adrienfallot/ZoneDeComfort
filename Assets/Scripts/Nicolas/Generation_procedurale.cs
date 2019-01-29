#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Generation_procedurale : MonoBehaviour {

    //public GameObject Parent_Poteaux;
    //public GameObject Chemin;
    //La distance entre chaque poteau
    //public GameObject Parent_Buildings;

    private int oldvariete;

    [Header("Correction du chemin")]
    public bool Noeud_final_concave;

    [Header("Création de chemin internes/externes")]
    public float Distance = 3;
    public bool Chemin_interne;

    [Header("Génération mobilier urbain")]
    public Object[] Mobilier_urbain;
	public float Distance_séparation = 3;
	public float Décalage_initial = 0;
	public Vector3 Rotation_mobilier;
	public bool Aligner_mobilier_avec_pentes = true;
	public bool Générer_sur_le_dernier_trait = false;

    [Header("Construction bâtiments")]
    public bool Extérieur_du_Chemin;

    [Header("Rotation des bâtiments")]
    public GameObject Tous_les_bâtiments;
    public string Taille_batiment_à_tourner;

    [Header("Création des gardes-fous")]
    public GameObject Garde_fou;

    private Transform chemin;
	private Transform[] noeuds;
	private float length;
	private Vector3 spawnPoint;
	private Vector3 angle;
	private float restedist = 0;
	private GameObject leparent;
	private GameObject[] lespoteaux;
	private GameObject poteauclone;
	private int nb_variations = 0;

	private GameObject[] lesbuildings;
	private Object[] mesbat;
	private GameObject buildingclone;
	private Object randbat;
	private int sommebat;
	private int randrot;
	private bool tryagain = true;
	private Vector3 centrebat;
    private Vector3 toground;

    private int sens;

	private bool jelaveuxencore = true;
	private int securite, securite2, securite3;

	public void Recur_Triche () {

		jelaveuxencore = true;
		securite = 0;

		while (jelaveuxencore) {
			if (securite > 100) {
				Debug.Log ("Merde");
				break;
			}
			Correc_Path ();
		}
		
	}

    public void Rename_Nodes (GameObject path)
    {
        Transform[] lesnoeuds = path.GetComponentsInChildren<Transform>();

        for (int i = 0; i <lesnoeuds.Length; i++)
        {
            if (lesnoeuds[i].tag != "Chemin")
            lesnoeuds[i].name = "Noeud (" + i + ")";
        }
    }

	public void Correc_Path () {

		//Je dis que le chemin est ce gameobject
		chemin = this.transform;

		//Je définis la taille du chemin
		noeuds = new Transform[chemin.childCount];

		//Je check tous les noeuds du chemin
		for (int i = 0; i < chemin.childCount; i++) {
			noeuds [i] = chemin.GetChild (i);
		}

		//Le vecteur direction pour l'éventuel nouveau point
		Vector3 arctemp = Vector3.zero;

		//Je corrige les angles aigus
		for (int i = 0; i < chemin.childCount; i++) {
			
			//Je déclare les deux vecteurs de part et d'autres du point
			Vector3 vecbefore = Vector3.zero;
			Vector3 vecafter = Vector3.zero;

            if (i == 0)
            {
                //Les deux vecteurs de part et d'autres du point
                vecbefore = noeuds[chemin.childCount - 1].position - noeuds[i].position;
                vecafter = noeuds[i + 1].position - noeuds[i].position;
            }
            else if (i == chemin.childCount-1)
            {
                //Les deux vecteurs de part et d'autres du point
                vecbefore = noeuds[i - 1].position - noeuds[i].position;
                vecafter = noeuds[0].position - noeuds[i].position;
            }
            else
            {
                //Les deux vecteurs de part et d'autres du point
                vecbefore = noeuds[i - 1].position - noeuds[i].position;
                vecafter = noeuds[i + 1].position - noeuds[i].position;
            }

            //Pour se débarrasser des angles aigus
            if ((Vector3.SignedAngle (vecbefore, vecafter, Vector3.up) < 88) && (Vector3.SignedAngle (vecbefore, vecafter, Vector3.up) > 0)) {
				
				//Je défini le vecteur vers le point que je vais rajouter
				arctemp = Vector3.Normalize (vecafter) * 4;
				//Je rajoute le point
				GameObject newnode = new GameObject ();
				newnode.name = "Noeud (temp)";
				newnode.transform.parent = chemin;
				newnode.transform.SetSiblingIndex (i + 1);
				newnode.transform.position = noeuds [i].position + arctemp;

				//Je modifie la position du point actuel pour l'éloigner vers le précédent point
				noeuds [i].position = noeuds [i].position + (Vector3.Normalize (vecbefore) * 4);

				//Je stoppe la fonction et je dis que je la recommence après
				securite++;
				return;
			}
		}

		//J'allonge chaque trait pour qu'il soit un multiple de 2 sauf cas particulier du dernier noeud
		for (int i = 1; i < chemin.childCount - 1; i++) {

			/* Si je veux faire mes angles droits foireux
			if ((Vector3.SignedAngle (vecbefore, vecafter, Vector3.up) <= 135) && (Vector3.SignedAngle (vecbefore, vecafter, Vector3.up) > 90)) {
				
				//Je défini le vecteur vers le point que je vais rajouter
				arctemp = Vector3.Normalize (Vector3.Cross (Vector3.up, vecbefore)) * 6;
				//Je rajoute le point
				GameObject newnode = new GameObject();
				newnode.name = "Noeud " + (chemin.childCount + 1);
				newnode.transform.parent = chemin;
				newnode.transform.SetSiblingIndex (i + 1);
				newnode.transform.position = noeuds [i].position + arctemp;

				//Je stoppe la fonction et je dis que je la recommence après
				securite ++;
				return;
			}
			*/

			//Je regarde quelle est la longueur de l'arc, je la divise par deux, j'arrondi puis je multiplie
			length = Mathf.RoundToInt(Vector3.Distance(noeuds[i-1].position, noeuds[i].position) / 2f);
			length = 2 * length;
			//Le vecteur allant du précédent point à l'actuel
			arctemp = Vector3.Normalize (noeuds [i].position - noeuds [i-1].position);
			//La nouvelle position du point actuel pour que la distance soit un multiple de 2
			Vector3 newpos = noeuds[i-1].position + (arctemp * length);
			//J'attribue cette nouvelle position au noeud
			noeuds[i].position = newpos;

		}

		//Je m'ocuppe du cas particulier du dernier noeud

		//Je regarde quelle est la longueur de l'avant-dernier arc, je la divise par deux, j'arrondi au supérieur puis je multiplie
		length = Mathf.RoundToInt(Vector3.Distance(noeuds[chemin.childCount-1].position, noeuds[chemin.childCount-2].position) / 2f);
		length = 2 * length;

		//Je regarde quelle est la longueur du dernier, je la divise par deux, j'arrondi au supérieur puis je multiplie
		float finallength = Mathf.RoundToInt(Vector3.Distance(noeuds[0].position, noeuds[chemin.childCount-1].position) / 2f);
		finallength = 2 * finallength;

		//je détermine la distance entre les deux noeuds de part et d'autres du noeud dont je cherche la position
		float distinter = Vector3.Distance(noeuds[0].position, noeuds[chemin.childCount-2].position);

		//Je trouve le point qui permet d'avoir pile ces deux distance entre le dernier noeud et ses noeuds adjascents
		//La distance avec le point projeté perpendiculairement à la droite reliant les noeuds de part et d'autre du noeud cherché
		float x = ((finallength * finallength) - (length * length) + (distinter * distinter)) / (2 * distinter);
		//La distance entre le point projeté en question et le noeud cherché
		float z = Mathf.Sqrt((finallength * finallength) - (x * x));

		//Le vecteur normalisé reliant le noeud après le noeud cherché avec le noeud avant le noeud cherché
		Vector3 xvec = Vector3.Normalize(noeuds[chemin.childCount-2].position - noeuds[0].position);
		//Le vecteur normalisé perpendiculaire au précédent qui va vers la position désirée du noeud cherché
		Vector3 zvec = Vector3.zero;
			
		if (Noeud_final_concave)
			zvec = Vector3.Normalize(Vector3.Cross(Vector3.up, xvec));
		else
			zvec = Vector3.Normalize(Vector3.Cross(Vector3.down, xvec));

		//Je met le noeud à sa bonne position
		noeuds [chemin.childCount - 1].position = noeuds[0].position + (xvec * x) + (zvec * z);

		//Je dis que j'ai pas a recommencer la fonction
		jelaveuxencore = false;

        //Je renomme les noeuds correctement
        Rename_Nodes(chemin.gameObject);
	}

	public void GrowPath () {

		//Je dis que le chemin est ce gameobject
		chemin = this.transform;

		//Je définis la taille du chemin
		noeuds = new Transform[chemin.childCount];

		//Je check tous les noeuds du chemin
		for (int i = 0; i < chemin.childCount; i++) {
			noeuds [i] = chemin.GetChild (i);
		}

		//Si je veux faire le rétrécissement, j'inverse le sens de ma génération de chemin
		if (Chemin_interne)
			sens = -1;
		else
			sens = 1;

        //J'instancie le parent vide au niveau du premier point en tant que prefab
        Generation_procedurale lageneration = this.GetComponent<Generation_procedurale>();
        leparent = new GameObject();
        leparent.transform.position = noeuds[0].position;
        leparent.transform.rotation = Quaternion.identity;
        UnityEditorInternal.ComponentUtility.CopyComponent(lageneration);
        UnityEditorInternal.ComponentUtility.PasteComponentAsNew(leparent);
        leparent.AddComponent<Gizmo_chemin>();
        leparent.tag = "Chemin";

		if (Chemin_interne)
			leparent.name = chemin.name + " rétréci";
		else
			leparent.name = chemin.name + " agrandi";

		for (int i = 0; i < chemin.childCount; i++) {

			Instantiate_nodes (i);

		}
	}

	private void Instantiate_nodes (int i) {

		Vector3 nextpath, previouspath;

		if (i == 0) {
			//Je déclare les deux vecteurs partant des points suivants et précédents et allant vers mon point
			nextpath = sens * (noeuds [i].position - noeuds [i + 1].position);
			previouspath = sens * (noeuds [i].position - noeuds [chemin.childCount-1].position);
		} else if (i == chemin.childCount-1) {
			//Je déclare les deux vecteurs partant des points suivants et précédents et allant vers mon point
			nextpath = sens * (noeuds [i].position - noeuds [0].position);
			previouspath = sens * (noeuds [i].position - noeuds [i - 1].position);
		} else {
			//Je déclare les deux vecteurs partant des points suivants et précédents et allant vers mon point
			nextpath = sens * (noeuds [i].position - noeuds [i + 1].position);
			previouspath = sens * (noeuds [i].position - noeuds [i - 1].position);
		}

		//Je normalise les vecteurs pour éviter mes erreurs à la con
		nextpath = Vector3.Normalize(nextpath);
		previouspath = Vector3.Normalize(previouspath);

		//Je fais la somme de ces vecteurs pour donner le vecteur d'agrandissement
		Vector3 growpath = (nextpath + previouspath);
		growpath *= (Mathf.Sign(Vector3.SignedAngle(previouspath,nextpath,Vector3.up)));
		growpath = Vector3.Normalize (growpath);

		//Je fais le calcul permettant de trouver sa longueur
		float y = Distance / Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * Vector3.Angle(nextpath,previouspath)));
		growpath = 2 * growpath * y * Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(nextpath,growpath)));

		//Je génère le nouveau noeud du chemin à son emplacement
		GameObject newnode = new GameObject();
		newnode.transform.parent = leparent.transform;
		newnode.name = "Noeud (" + (i + 1) + ")";
		newnode.transform.position = noeuds [i].position + growpath;

	}

	private void Instantiate_pylones (int i, int ibis) {
        
		//Je regarde combien de poteaux je vais mettre et le reste de longueur que j'aurais
		length = Vector3.Distance(noeuds[i].position, noeuds[ibis].position);
        
        int jend = (int)(length / Distance_séparation);
        int j = 1;

        //Le vecteur allant du premier point au second
        Vector3 arctempfull = noeuds[ibis].position - noeuds[i].position;
        Vector3 arctemp = Vector3.Normalize(arctempfull);

        //Le vecteur précédent
        if ((i > 0) && (ibis > 0))
        {
            Vector3 pastarctempfull = noeuds[ibis - 1].position - noeuds[i - 1].position;
            Vector3 pastarctemp = Vector3.Normalize(pastarctempfull);

            if (Vector3.SignedAngle(pastarctemp, arctemp, Vector3.forward) > 0)
            {
                restedist -= 3 * Distance_séparation;
                j = (int)(length / Distance_séparation) + 3;
                jend = (int)(length / Distance_séparation) - 3;
            }
        }

        if ((length + restedist) >= Distance_séparation) {

			for (j = 1; j <= jend; j++) {

				//Je déclare mon angle pour les poteaux qui suivent la pente
				Quaternion anglequat = Quaternion.identity;

                //Le point où je vais faire poper mon poteau
                spawnPoint = noeuds [i].position + (j * arctemp * Distance_séparation) - (restedist * arctemp);

				if (Aligner_mobilier_avec_pentes) {
					//Je fais un raycast pour que ce soit propre
					RaycastHit monray;
					Physics.Raycast((spawnPoint + Vector3.up), Vector3.down, out monray);
                    //L'angle que je veux pour le poteau
                    Vector3 perpente = Vector3.Cross(monray.normal, arctemp);
                    Vector3 pente = Vector3.Cross(monray.normal, perpente);
                    anglequat = Quaternion.LookRotation (pente,monray.normal);
                    toground = (Vector3.down * monray.distance) + Vector3.up;
				}

				else {
					//L'angle que je veux pour le poteau
					angle = new Vector3 (0, 0, Vector3.SignedAngle(Vector3.right, arctemp, Vector3.forward));
					angle = angle + Rotation_mobilier;
				}

                int caseof = 0;
                int variete = oldvariete;

                //J'instancie le poteau en tant que prefab
                while (oldvariete == variete)
                {
                    caseof++;
                    variete = Random.Range(0, Mobilier_urbain.Length);
                    if (caseof >= 1000)
                        break;
                }
                oldvariete = variete;
				poteauclone = PrefabUtility.InstantiatePrefab (Mobilier_urbain [variete]) as GameObject;

				//Je lui dit de passer au gameobject suivant dans la liste des gameobjects
				nb_variations++;

				//Je le met dans le bon parent, je le place dans l'espace et lui met la bonne rotation
				poteauclone.name = "Poteau_" + (lespoteaux.Length + 1) + "_" + (leparent.transform.childCount + 1);
				poteauclone.transform.parent = leparent.transform;
				poteauclone.transform.position = spawnPoint;
                if (Aligner_mobilier_avec_pentes)
                {
                    poteauclone.transform.rotation = anglequat;
                    poteauclone.transform.position += toground;
                }
                else
                {
                    poteauclone.transform.rotation = Quaternion.Euler(angle);
                }
			}
		}
		//Je détermine le reste de la distance qu'il me restait à parcourir
		restedist = (length + restedist) % Distance_séparation;
	}

	public void BuildPylones()
	{
		//Je récupère tous les groupes de poteaux existants
		lespoteaux = GameObject.FindGameObjectsWithTag("Poteaux");

		//Je reset le reste de distance
		restedist = Décalage_initial;

		//Je dis que le chemin est ce gameobject
		chemin = this.transform;

		//Je définis la taille du chemin
		noeuds = new Transform[chemin.childCount];

		//Je check tous les noeuds du chemin
		for (int i = 0; i < chemin.childCount; i++) {
			noeuds [i] = chemin.GetChild (i);
		}

        //J'instancie le parent vide au niveau du premier point
        leparent = new GameObject();
        leparent.transform.position = noeuds[0].position;
		leparent.name = "Parent_Poteaux " + (lespoteaux.Length + 1);

		//Je précise qu'il y a eu aucune variation pour le moment
		nb_variations = 0;

		for (int i = 0; i < chemin.childCount-1; i++) {
			
			Instantiate_pylones (i, i+1);

		}

		if (!Générer_sur_le_dernier_trait)
		//J'instancie le dernier poteau
		Instantiate_pylones (chemin.childCount - 1, 0);

	}

	private void Instantiate_Buildings (int i, int ibis) {

		//Je regarde combien d'unités de bâtiments je pourrais mettre
		length = Mathf.RoundToInt(Vector3.Distance(noeuds[i].position, noeuds[ibis].position) / 2f);

		//Le vecteur allant du premier point au second
		Vector3 arctemp = Vector3.Normalize (noeuds [ibis].position - noeuds [i].position);

		//Tant que j'ai pas atteint le bout de cet arc
		while (sommebat != length)
        {
			
            securite2++;

            if (securite2 > 200)
            {
                Debug.Log("Merde");
                break;
            }

			//Je choisi un bâtiment au pif parmi ceux existant et recommence si il est trop grand
			while (tryagain) {

                securite3++;

                if (securite3 > 200)
                {
                    Debug.Log("Merde");
                    break;
                }

                randbat = mesbat[Random.Range(0,mesbat.Length)];
				if (sommebat + TailleBat (randbat) <= length)
					tryagain = false;
				
			}

            securite3 = 0;

			tryagain = true;

			//Le point où je vais faire poper mon bâtiment
			spawnPoint = noeuds [i].position + (sommebat * 2 * arctemp);
			
			//L'angle que je veux pour le bâtiment
			if (!Extérieur_du_Chemin)
			angle = new Vector3 (0, Vector3.SignedAngle (arctemp, Vector3.right, Vector3.down), 0);
			if (Extérieur_du_Chemin)
				angle = new Vector3 (0, Vector3.SignedAngle (arctemp, Vector3.right, Vector3.down) + 90, 0);

			//J'instancie le bâtiment en tant que prefab
			buildingclone = PrefabUtility.InstantiatePrefab (randbat) as GameObject;

			//Je fais tourner le bâtiment sur lui même pour changer de face visible
			Transform batipivot = buildingclone.transform.Find("Batiment");
			randrot = Random.Range (0, 4) * 90;
			centrebat = TailleBat(randbat) * Vector3.one;
			batipivot.transform.RotateAround(centrebat, Vector3.up,randrot);

			//Je le met dans le bon parent, je le place dans l'espace et lui met la bonne rotation
			buildingclone.name = "Bâtiment_" + (lesbuildings.Length + 1) + "_" + (leparent.transform.childCount + 1);
			buildingclone.transform.parent = leparent.transform;
			buildingclone.transform.position = spawnPoint;
			buildingclone.transform.rotation = Quaternion.Euler (angle);

			//Je met à jour la somme de la longueur des bâtiments
			sommebat += TailleBat(randbat);

		}

        securite2 = 0;

		//Je reset la somme de la longueur des bâtiments
		sommebat = 0;

	}

	private int TailleBat (Object lebat) {

		if ((lebat as GameObject).tag == "1")
			return 1;
		else if ((lebat as GameObject).tag == "2")
			return 2;
		else if ((lebat as GameObject).tag == "3")
			return 3;
		else if ((lebat as GameObject).tag == "4")
			return 4;
		else if ((lebat as GameObject).tag == "5")
			return 5;
		else
			return 1000;
		
	}

	public void BuildBuildings()
	{
		//Je récupère tous les groupes de bâtiments existants
		lesbuildings = GameObject.FindGameObjectsWithTag("Buildings");

		//Je charge tous les bâtiments modélisés dans un tableau
		mesbat = Resources.LoadAll ("Prefabs/Batiments");

		//Je reset le reste de distance
		restedist = 0;

		//Je dis que le chemin est ce gameobject
		chemin = this.transform;

		//Je définis la taille du chemin
		noeuds = new Transform[chemin.childCount];

		//Je check tous les noeuds du chemin
		for (int i = 0; i < chemin.childCount; i++) {
			noeuds [i] = chemin.GetChild (i);
		}

        //J'instancie le parent vide au niveau du premier point
        leparent = new GameObject(); 
        leparent.transform.parent = Tous_les_bâtiments.transform;
        leparent.transform.position = noeuds[0].position;
		leparent.name = "Parent_Buildings " + (lesbuildings.Length + 1);

		for (int i = 0; i < chemin.childCount-1; i++) {

			Instantiate_Buildings (i, i+1);

		}

		//J'instancie la dernière rangée de buildings
		Instantiate_Buildings (chemin.childCount - 1, 0);

	}

	public void RotateBuildings()
	{
		Vector3 centre = new Vector3 (10, 10, 10);

		Transform[] Bat_all = Tous_les_bâtiments.GetComponentsInChildren<Transform> ();

		foreach (Transform lebat in Bat_all) {

			if ((lebat.name == "Batiment") && (lebat.tag == Taille_batiment_à_tourner)){

				//Je trouve les coordonnées locales du centre de rotation
				Transform[] Bat_int = lebat.GetComponentsInChildren<Transform>();

				foreach (Transform interieur in Bat_int) {

					if (interieur.name == "Centre") {

						centre = interieur.position;

					}
				}

				//Je fais tourner mon bâtiment autour de son centre de rotation d'une valeur aléatoire de l'ordre de 90°
				lebat.transform.RotateAround(centre,Vector3.up,Random.Range(0,3) * 90);

			}
		}
	}

	public void GardeFous()
	{
		//Je récupère tous les groupes de poteaux existants
		lespoteaux = GameObject.FindGameObjectsWithTag("Poteaux");

		//Je dis que le chemin est ce gameobject
		chemin = this.transform;

		//Je définis la taille du chemin
		noeuds = new Transform[chemin.childCount];

		//Je précise qu'il y a eu aucune variation pour le moment
		nb_variations = 0;

		//Je check tous les noeuds du chemin
		for (int i = 0; i < chemin.childCount; i++) {
			noeuds [i] = chemin.GetChild (i);
		}

        //J'instancie le parent vide au niveau du premier point
        leparent = new GameObject();
        leparent.transform.position = noeuds[0].position;
		leparent.name = "Parent_Garde-Fous " + (lespoteaux.Length + 1);

		for (int i = 0; i < chemin.childCount-1; i++) {

			Instantiate_gardefou (i, i+1);

		}
	}

	private void Instantiate_gardefou (int i, int ibis) {

		//Le vecteur allant du premier point au second
		Vector3 arctempfull = noeuds [ibis].position - noeuds [i].position;
		Vector3 arctemp = Vector3.Normalize (arctempfull);

		//Le point où je vais faire poper mon garde-fou
		spawnPoint = noeuds [i].position;
				
		//L'angle que je veux pour le poteau
		Quaternion anglequat = Quaternion.LookRotation (arctemp);

		//J'instancie le poteau en tant que prefab
		//int variete = nb_variations % Mobilier_urbain.Length;
		poteauclone = PrefabUtility.InstantiatePrefab (Garde_fou) as GameObject;

		//Je lui dit de passer au gameobject suivant dans la liste des gameobjects
		nb_variations++;

		//Je le met dans le bon parent, je le place dans l'espace et lui met la bonne rotation
		poteauclone.name = "Garde_Fou_" + (lespoteaux.Length + 1) + "_" + (leparent.transform.childCount + 1);
		poteauclone.transform.parent = leparent.transform;
		poteauclone.transform.position = spawnPoint;
		poteauclone.transform.rotation = Quaternion.Euler (Rotation_mobilier) * anglequat;

		//Je rescale le prefab à la bonne longeur
		float scalez = arctempfull.magnitude;
		poteauclone.transform.localScale = new Vector3 (poteauclone.transform.localScale.x, poteauclone.transform.localScale.y, scalez);

	}

}
#endif