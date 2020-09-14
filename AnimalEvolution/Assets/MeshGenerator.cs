using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    int xsize=5;
    int zsize=4;
    int yheight=0;
    float[,] heightMap;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        heightMap = newHeightMap(xsize, yheight, zsize);
        mesh.vertices = prepareVertices(xsize, heightMap, zsize);
        mesh.triangles = prepareTriangles(xsize, zsize);
    }
    
    Vector3[] prepareVertices(int xSize, float[,] heightMap, int zSize)
    {
        Vector3[] result = new Vector3[xSize * zSize];
        int currentVertice = 0;
        for (int j = 0; j < zSize; j++)
        {
            for (int i = 0; i < xSize; i++)
            {
                result[currentVertice] = new Vector3(i, heightMap[i, j], j);
                currentVertice++;
            }
        }
        return result;
    }

    int[] prepareTriangles(int xSize, int zSize)
    {
        int[] result = new int[6*(xSize-1)*(zSize-1)];
        int currentTrianglePoint = 0;
        int startVertice = 0;
        while (currentTrianglePoint < result.Length)
        {
            result[currentTrianglePoint++] = startVertice;
            result[currentTrianglePoint++] = startVertice + xSize;
            result[currentTrianglePoint++] = startVertice + 1;
            startVertice++;
            result[currentTrianglePoint++] = startVertice;
            result[currentTrianglePoint++] = startVertice + xSize - 1;
            result[currentTrianglePoint++] = startVertice + xSize;

            if ((startVertice % xSize) == (xSize-1))
            {
                startVertice++;
            }
        }
        return result;
    }

    float[,] newHeightMap(int xSize, int yHeight, int zSize)
    {
        float[,] result = new float[xSize, zSize];
        for (int j = 0; j < zSize; j++)
        {
            for (int i = 0; i < xSize; i++)
            {
                result[i, j] = 0;
            }
        }
        return (result);
    }
}
