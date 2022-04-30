using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnimalEvolution
{

    [RequireComponent(typeof(MeshFilter))]
    public class TerrainGenerator : MonoBehaviour
    {
        Mesh mesh;
        public MeshCollider meshCollider;
        public WaterGeneration water;
        public int xsize = 100;
        public int zsize = 100;
        public int yheight = 20;
        public int waterheight = 50;
        public int seed = 1005001;
        int sizeMultiplier = 4;
        float[,] heightMap;
        float scale = 50f;

        int octaves = 8;
        float persistence = 0.2f;
        float lacunarity = 1.6f;

        /// <summary>
        /// Checks the provided values against the criteria and if all are accepted
        /// updates the values in this script.
        /// </summary>
        /// <param name="xsize">width of the map</param>
        /// <param name="zsize">length of the map</param>
        /// <param name="yheight">height of the map</param>
        /// <param name="waterheight">percentage of height tha should be submerged in water</param>
        /// <param name="seed">seed for map generation</param>
        /// <returns> if the parameters fit the terrain creation criteria</returns>
        public bool SetValues(int xsize, int zsize, int yheight, int waterheight, int seed)
        {
            if (xsize < 0 || xsize > 250) return false;
            if (zsize < 0 || zsize > 250) return false;
            if (yheight < 0 || yheight > 30) return false;
            if (waterheight < 0 || waterheight > 100) return false;

            this.xsize = xsize;
            this.zsize = zsize;
            this.yheight = yheight;
            this.waterheight = waterheight;
            this.seed = seed;


            return true;
        }
        public bool SetWater(WaterGeneration water)
        {
            if (water == null) return false;
            this.water = water;
            return true;
        }

        public void Regenerate()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            Destroy(mesh);
            GetComponent<MeshFilter>().mesh = new Mesh();
            mesh = GetComponent<MeshFilter>().mesh;
            ;
            heightMap = Noise.GenerateNoiseMap(xsize, zsize, seed, scale, octaves, persistence, lacunarity, new Vector2(0, 0));

            for (int i = 0; i < xsize; i++)
            {
                for (int j = 0; j < zsize; j++)
                {
                    heightMap[i, j] *= yheight;
                }
            }
            mesh.vertices = PrepareVertices(xsize, heightMap, zsize);
            mesh.triangles = PrepareTriangles(xsize, zsize);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            water.Generate(xsize, yheight * waterheight / 100, zsize, sizeMultiplier);

            
            meshCollider = GetComponent<MeshCollider>();
            Destroy(meshCollider);
            MeshCollider mC = this.gameObject.AddComponent<MeshCollider>() as MeshCollider;
            mC.sharedMesh = GetComponent<MeshFilter>().mesh;
            mC.enabled = true;
            Controller.xBoundary = (xsize - 1) * sizeMultiplier;
            Controller.zBoundary = (zsize - 1) * sizeMultiplier;
            Controller.yWaterLevel = yheight * waterheight / 100*sizeMultiplier;
        }

        Vector3[] PrepareVertices(int xSize, float[,] heightMap, int zSize)
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

        int[] PrepareTriangles(int xSize, int zSize)
        {
            int planetriangles = 6 * (xSize - 1) * (zSize - 1);
            int sidestriangles = 24 * (xSize + zSize - 2);
            int[] result = new int[planetriangles + sidestriangles + 6];
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

                if ((startVertice % xSize) == (xSize - 1))
                {
                    startVertice++;
                }
            }
            int planevertices = xSize * zSize;
            for (int i = 0; i < xSize - 1; i++)
            {
                result[currentTrianglePoint++] = i;
                result[currentTrianglePoint++] = i + 1;
                result[currentTrianglePoint++] = planevertices + i;

                result[currentTrianglePoint++] = i + 1;
                result[currentTrianglePoint++] = planevertices + i + 1;
                result[currentTrianglePoint++] = planevertices + i;
            }
            int planeverticesplus = planevertices + xSize;
            for (int i = 0; i < zSize - 1; i++)
            {
                result[currentTrianglePoint++] = i * xSize;
                result[currentTrianglePoint++] = planeverticesplus + i;
                result[currentTrianglePoint++] = (i + 1) * xSize;

                result[currentTrianglePoint++] = (i + 1) * xSize;
                result[currentTrianglePoint++] = planeverticesplus + i;
                result[currentTrianglePoint++] = planeverticesplus + i + 1;
            }
            planeverticesplus += zSize;
            for (int i = 0; i < xSize - 1; i++)
            {
                result[currentTrianglePoint++] = planevertices - xSize + i;
                result[currentTrianglePoint++] = planeverticesplus + i;
                result[currentTrianglePoint++] = planevertices - xSize + i + 1;

                result[currentTrianglePoint++] = planevertices - xSize + i + 1;
                result[currentTrianglePoint++] = planeverticesplus + i;
                result[currentTrianglePoint++] = planeverticesplus + i + 1;
            }
            planeverticesplus += xSize;
            for (int i = 0; i < zSize - 1; i++)
            {
                result[currentTrianglePoint++] = i * xSize - 1 + xSize;
                result[currentTrianglePoint++] = (i + 1) * xSize - 1 + xSize;
                result[currentTrianglePoint++] = planeverticesplus + i;

                result[currentTrianglePoint++] = (i + 1) * xSize - 1 + xSize;
                result[currentTrianglePoint++] = planeverticesplus + i + 1;
                result[currentTrianglePoint++] = planeverticesplus + i;
            }
            result[currentTrianglePoint++] = planevertices;
            result[currentTrianglePoint++] = planevertices + xSize - 1;
            result[currentTrianglePoint++] = planevertices + xSize + zSize - 1;
            result[currentTrianglePoint++] = planevertices + xSize - 1;
            result[currentTrianglePoint++] = mesh.vertices.Length - 1;
            result[currentTrianglePoint++] = planevertices + xSize + zSize - 1;
            return result;
        }

    }
}