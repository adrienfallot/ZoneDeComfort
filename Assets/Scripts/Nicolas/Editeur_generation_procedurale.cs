#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Generation_procedurale))]
public class Editeur_generation_procedurale : Editor
{

    bool showMobilier;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Generation_procedurale myScript = (Generation_procedurale)target;

        /*
        showMobilier = EditorGUILayout.Foldout(showMobilier, "Mobilier urbain");

        if (showMobilier)
        {
            EditorGUILayout.IntField("Variétés", myScript.Mobilier.Length);

            for (int i=0; i < myScript.Mobilier.Length; i++)
            {
                myScript.Mobilier[i] = EditorGUILayout.ObjectField("Mobilier" + (i + 1), myScript.Mobilier[i], typeof(GameObject));
            }
        }
        */

        if (GUILayout.Button("Corriger chemin"))
        {
            myScript.Recur_Triche();
        }

        //myScript.Distance_séparation = EditorGUILayout.FloatField("Distance", myScript.Distance_séparation);

        if (GUILayout.Button("Etendre chemin"))
        {
            myScript.GrowPath();
        }

        if (GUILayout.Button("Construire poteaux"))
        {
            myScript.BuildPylones();
        }

        if (GUILayout.Button("Construire bâtiments"))
        {
            myScript.BuildBuildings();
        }

        if (GUILayout.Button("Rotation bâtiments"))
        {
            myScript.RotateBuildings();
        }

        if (GUILayout.Button("Construire Gardes-Fous"))
        {
            myScript.GardeFous();
        }
    }
}
#endif