using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MeshGenerator))]
public class MeshEditorScript : Editor {

    public override void OnInspectorGUI()
    {
        MeshGenerator meshGen = (MeshGenerator)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Regenerate"))
        {
            meshGen.Redraw();
        }

    }
}
