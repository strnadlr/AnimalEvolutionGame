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
        float sn =0.1f;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = new Vector3[] {
        new Vector3(0+sn, terrain.yheight/2, 0+sn),
        new Vector3(terrain.xsize-1-sn, terrain.yheight/2, 0+sn),
        new Vector3(terrain.xsize-1-sn, terrain.yheight/2, terrain.zsize-1-sn),
        new Vector3(0+sn, terrain.yheight/2, terrain.zsize-1-sn),
        new Vector3(0+sn, 0+sn, 0+sn),
        new Vector3(terrain.xsize-1-sn,  0+sn, 0+sn),
        new Vector3(terrain.xsize-1-sn,  0+sn, terrain.zsize-1-sn),
        new Vector3(0+sn,  0+sn, terrain.zsize-1-sn) };
        mesh.triangles = new int[] { 0, 3, 1, 1, 3, 2, 0, 5, 4, 1, 5, 0, 1, 6, 5, 1, 2,6,2, 7,6, 2, 3, 7, 0, 7, 3, 0, 4, 7, };
        mesh.RecalculateNormals();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
