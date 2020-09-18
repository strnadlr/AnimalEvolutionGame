using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class WaterGeneration : MonoBehaviour
{
    Mesh mesh;
    public MeshGenerator terrain;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = new Vector3[] {
        new Vector3(0, terrain.yheight/2, 0),
        new Vector3(terrain.xsize, terrain.yheight/2, 0),
        new Vector3(terrain.xsize, terrain.yheight/2, terrain.zsize),
        new Vector3(0, terrain.yheight/2, terrain.zsize) };
        mesh.triangles = new int[] { 0, 3, 1, 1, 3, 2 };
        mesh.RecalculateNormals();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
