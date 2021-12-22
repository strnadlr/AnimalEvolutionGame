﻿using UnityEngine;
using UnityEditor;

namespace AnimalEvolution
{

    [CustomEditor(typeof(TerrainGenerator))]
    public class MeshEditorScript : Editor
    {

        public override void OnInspectorGUI()
        {
            TerrainGenerator meshGen = (TerrainGenerator)target;
            DrawDefaultInspector();
            if (GUILayout.Button("Regenerate"))
            {
                meshGen.Regenerate();
            }

        }
    }
}