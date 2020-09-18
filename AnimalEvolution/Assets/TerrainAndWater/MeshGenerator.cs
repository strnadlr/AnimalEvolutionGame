using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    public int xsize=100;
    public int zsize=100;
    public int yheight=20;
    float[,] heightMap;
    public float scale=0.124f;
    public int seed = 502;
    public int octaves=4;
    public float persistence=0.5f;
    public float lacunarity=1.87f;
    public Button redraw;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        heightMap = newHeightMap(seed, xsize, yheight, zsize, scale, octaves, persistence, lacunarity);
        mesh.vertices = prepareVertices(xsize, heightMap, zsize);
        mesh.triangles = prepareTriangles(xsize, zsize);
        mesh.RecalculateNormals();
    }

    public void Redraw()
    {
        heightMap = newHeightMap(seed, xsize, yheight, zsize, scale, octaves, persistence, lacunarity);
        mesh.vertices = prepareVertices(xsize, heightMap, zsize);
        mesh.triangles = prepareTriangles(xsize, zsize);
        mesh.RecalculateNormals();
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

    float[,] newHeightMap(int seed, int xSize, int yHeight, int zSize, float scale, int octaves, float persistence, float lacunarity)
    {
        System.Random random = new System.Random(seed);
        float[,] result = new float[xSize, zSize];
        float max = float.MinValue;
        float min = float.MaxValue;
        if (scale <= 0) scale = 0.0001f;
            for (int j = 0; j < zSize; j++)
            {
                for (int i = 0; i < xSize; i++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int k = 0; k < octaves; k++)
                    {
                        float x = (float)(i / scale * frequency);
                        float z = (float)(j / scale * frequency);
                        float perlin = Mathf.PerlinNoise(x, z) * 2 - 1;
                        noiseHeight += (perlin * amplitude);
                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }
                    result[i, j] = noiseHeight;
                    if (noiseHeight > max) max = noiseHeight;
                    else if (noiseHeight < min) min = noiseHeight;
                    
                }
            }
        
        for (int j = 0; j < zSize; j++)
        {
            for (int i = 0; i < xSize; i++)
            {
                result[i, j] = Mathf.InverseLerp(min, max, result[i, j]) * yHeight;
            }
        }
        return result;
    }
}
