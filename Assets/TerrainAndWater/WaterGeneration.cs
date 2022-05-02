using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class WaterGeneration : MonoBehaviour
{
    Mesh mesh;

    /// <summary>
    /// Creates a box of water slightly less wide and long than the map to be placed into the world.
    /// </summary>
    /// <param name="xsize">Width of the map.</param>
    /// <param name="waterheight">Height of water.</param>
    /// <param name="zsize">Length of the map.</param>
    /// <param name="sizeMultiplier">unit length multiplier.</param>
    public void Generate(float xsize, float waterheight, float zsize, int sizeMultiplier)
    {
        xsize *= sizeMultiplier;
        waterheight *= sizeMultiplier;
        zsize *= sizeMultiplier;
        float sn = 0.1f;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = new Vector3[] {
        new Vector3(0+sn, waterheight, 0+sn),
        new Vector3(xsize-1-sn, waterheight, 0+sn),
        new Vector3(xsize-1-sn, waterheight, zsize-1-sn),
        new Vector3(0+sn, waterheight, zsize-1-sn),
        new Vector3(0+sn, 0+sn, 0+sn),
        new Vector3(xsize-1-sn,  0+sn, 0+sn),
        new Vector3(xsize-1-sn,  0+sn, zsize-1-sn),
        new Vector3(0+sn,  0+sn, zsize-1-sn) };
        mesh.triangles = new int[] { 0, 3, 1, 1, 3, 2, 0, 5, 4, 1, 5, 0, 1, 6, 5, 1, 2, 6, 2, 7, 6, 2, 3, 7, 0, 7, 3, 0, 4, 7, };
        mesh.RecalculateNormals();
    }
}
