using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("UnitTestMeshGenrator")]
[RequireComponent(typeof(MeshFilter))]

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    int xsize=5;
    int zsize=4;
    int yheight=0;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh = prepareMeshFromHeightMap(xsize, newHeightMap(xsize,yheight,zsize), zsize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    Mesh prepareMeshFromHeightMap(int xSize, float[,] heightMap, int zSize)
    {
        vertices = new Vector3[xSize*zSize];
        int currentVertice = 0;
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < zSize; j++)
            {
                vertices[currentVertice] = new Vector3(i, heightMap[i, j], j);
                currentVertice++;
            }
        }

        triangles = new int[6*(xSize-1)*(zSize-1)];
        int currentTrianglePoint = 0;
        int startVertice = 0;
        while (currentTrianglePoint < triangles.Length)
        {
            triangles[currentTrianglePoint++] = startVertice;
            triangles[currentTrianglePoint++] = startVertice + 1;
            triangles[currentTrianglePoint++] = startVertice + xSize;

            startVertice++;
            triangles[currentTrianglePoint++] = startVertice;
            triangles[currentTrianglePoint++] = startVertice + xSize;
            triangles[currentTrianglePoint++] = startVertice + xSize - 1;

            if ((startVertice % xSize) == (xSize - 1))
            {
                startVertice++;
            }
        }

        Mesh result = new Mesh();
        result.vertices = vertices;
        result.triangles = triangles;
        return result;
    }

    float[,] newHeightMap(int xSize, int yHeight, int zSize)
    {
        float[,] heightMap = new float[xSize, zSize];
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < zSize; j++)
            {
                heightMap[i, j] = 0;
            }
        }
        return (heightMap);
    }
}
