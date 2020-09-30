using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]

public class TerrainGenerator : MonoBehaviour
{
    Mesh mesh;
    public WaterGeneration water;
    public int xsize=100;
    public int zsize=100;
    public int yheight=40;
    public int waterheight = 20;
    int sizeMultiplier=4;
    float[,] heightMap;
    float scale= 205.23f;
    public int seed = 502;
    int octaves=4;
    float persistence=0.5f;
    float lacunarity=2f;

    public bool SetValues(WaterGeneration water, int xsize, int zsize, int yheight, int waterheight)
    {
        if (water == null) return false;
        this.water = water;
        if (xsize < 0 || xsize > 250) return false;
        this.xsize = xsize;
        if (zsize < 0 || zsize > 250) return false;
        this.zsize = zsize;
        if (yheight < 0 || yheight > 200) return false;
        this.yheight = yheight;
        if (waterheight < 0 || waterheight > 100) return false;
        this.waterheight = waterheight;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        heightMap = newHeightMap(seed, xsize, yheight, zsize, scale, octaves, persistence, lacunarity);
        mesh.vertices = prepareVertices(xsize, heightMap, zsize);
        mesh.triangles = prepareTriangles(xsize, zsize);
        mesh.RecalculateNormals();
        water.Generate(xsize, yheight*waterheight/100, zsize, sizeMultiplier);
    }

    public void Regenerate()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        heightMap = newHeightMap(seed, xsize, yheight, zsize, scale, octaves, persistence, lacunarity);
        mesh.vertices = prepareVertices(xsize, heightMap, zsize);
        mesh.triangles = prepareTriangles(xsize, zsize);
        mesh.RecalculateNormals();
        water.Generate(xsize, yheight * waterheight / 100, zsize, sizeMultiplier);
    }

    Vector3[] prepareVertices(int xSize, float[,] heightMap, int zSize)
    {
        Vector3[] result = new Vector3[(xSize * zSize) + 2 * (xSize + zSize)];
        int currentVertice = 0;
        for (int j = 0; j < zSize; j++)
        {
            for (int i = 0; i < xSize; i++)
            {
                result[currentVertice] = new Vector3(sizeMultiplier * i, sizeMultiplier * heightMap[i, j], sizeMultiplier * j);
                currentVertice++;
            }
        }
        for (int i = 0; i < xSize; i++)
        {
            result[currentVertice++] = new Vector3(sizeMultiplier * i, 0, 0); //First xSize
        }
        for (int i = 0; i < zSize; i++)
        {
            result[currentVertice++] = new Vector3(0, 0, sizeMultiplier * i); //every xSize-th
        }

        for (int i = 0; i < xSize; i++)
        {
            result[currentVertice++] = new Vector3(sizeMultiplier * i, 0, sizeMultiplier * (zSize - 1)); //Last xSize
        }
        for (int i = 0; i < zSize; i++)
        {
            result[currentVertice++] = new Vector3(sizeMultiplier * (xSize - 1), 0, sizeMultiplier * i); //every 2xSize-1th
        }
        return result;
    }

    int[] prepareTriangles(int xSize, int zSize)
    {
        int planetriangles = 6 * (xSize - 1) * (zSize - 1);
        int sidestriangles = 24 * (xSize + zSize - 2);
        int[] result = new int[planetriangles+sidestriangles+6];
        int currentTrianglePoint = 0;
        int startVertice = 0;
        while (currentTrianglePoint < planetriangles)
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
        int planevertices = xSize * zSize;
        for (int i = 0; i < xSize-1; i++)
        {
            result[currentTrianglePoint++] = i;
            result[currentTrianglePoint++] = i + 1;
            result[currentTrianglePoint++] = planevertices + i;

            result[currentTrianglePoint++] = i+1;
            result[currentTrianglePoint++] = planevertices+ i + 1;
            result[currentTrianglePoint++] = planevertices + i;
        }
        int planeverticesplus = planevertices + xSize;
        for (int i = 0; i < zSize - 1; i++)
        {
            result[currentTrianglePoint++] = i*xSize;
            result[currentTrianglePoint++] = planeverticesplus + i;
            result[currentTrianglePoint++] = (i + 1)*xSize;

            result[currentTrianglePoint++] = (i + 1)*xSize;
            result[currentTrianglePoint++] = planeverticesplus + i;
            result[currentTrianglePoint++] = planeverticesplus + i + 1;
        }
        planeverticesplus += zSize;
        for (int i = 0; i < xSize - 1; i++)
        {
            result[currentTrianglePoint++] = planevertices-xSize+i;
            result[currentTrianglePoint++] = planeverticesplus + i;
            result[currentTrianglePoint++] = planevertices - xSize + i + 1;

            result[currentTrianglePoint++] = planevertices - xSize + i + 1;
            result[currentTrianglePoint++] = planeverticesplus + i;
            result[currentTrianglePoint++] = planeverticesplus + i + 1;
        }
        planeverticesplus += xSize;
        for (int i = 0; i < zSize - 1; i++)
        {
            result[currentTrianglePoint++] = i * xSize-1+xSize;
            result[currentTrianglePoint++] = (i + 1) * xSize-1+xSize;
            result[currentTrianglePoint++] = planeverticesplus + i;

            result[currentTrianglePoint++] = (i + 1) * xSize-1+xSize;
            result[currentTrianglePoint++] = planeverticesplus + i + 1;
            result[currentTrianglePoint++] = planeverticesplus + i;
        }
        result[currentTrianglePoint++] = planevertices;
        result[currentTrianglePoint++] = planevertices+xSize-1;
        result[currentTrianglePoint++] = planevertices+xSize+zSize-1;
        result[currentTrianglePoint++] = planevertices + xSize - 1;
        result[currentTrianglePoint++] = mesh.vertices.Length - 1;
        result[currentTrianglePoint++] = planevertices + xSize + zSize - 1;
        return result;
    }

    float[,] newHeightMap(int seed, int xSize, int yHeight, int zSize, float scale, int octaves, float persistence, float lacunarity)
    {
        System.Random random = new System.Random(seed);
        float[] rands = new float[2*octaves];
        for (int i = 0; i < 2*octaves; i++)
        {
            rands[i] = random.Next(-10000,10000);
        }
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
                    float x = i / scale * frequency + rands[2*k];
                    float z = j / scale * frequency + rands[2*k+1];
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
