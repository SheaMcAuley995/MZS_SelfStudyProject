using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof (MapGen))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGen mapgen = (MapGen)target;
        //DrawDefaultInspector();
        if (DrawDefaultInspector())
        {
            if (mapgen.autpUpdate)
            {
                mapgen.GenThatMap();
            }
        }
        if (GUILayout.Button("Generate That Map!"))
        {
            mapgen.GenThatMap();
        }
    }

}
