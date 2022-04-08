using UnityEngine;
using System.Collections;


/// <summary>
/// Source:
/// https://github.com/SebLague/Procedural-Landmass-Generation.git
/// 
/// License:
/// MIT License
///
/// Copyright (c) 2016 Sebastian
///
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the 
/// "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish,
/// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject
/// to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
/// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
/// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
/// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
/// </summary>


namespace AnimalEvolution
{
	public static class Noise
	{

		public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
		{
			float[,] noiseMap = new float[mapWidth, mapHeight];

			System.Random prng = new System.Random(seed);
			Vector2[] octaveOffsets = new Vector2[octaves];
			for (int i = 0; i < octaves; i++)
			{
				float offsetX = prng.Next(-100000, 100000) + offset.x;
				float offsetY = prng.Next(-100000, 100000) + offset.y;
				octaveOffsets[i] = new Vector2(offsetX, offsetY);
			}

			if (scale <= 0)
			{
				scale = 0.0001f;
			}

			float maxNoiseHeight = float.MinValue;
			float minNoiseHeight = float.MaxValue;

			float halfWidth = mapWidth / 2f;
			float halfHeight = mapHeight / 2f;


			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{

					float amplitude = 1;
					float frequency = 1;
					float noiseHeight = 0;

					for (int i = 0; i < octaves; i++)
					{
						float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
						float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

						float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
						noiseHeight += perlinValue * amplitude;

						amplitude *= persistance;
						frequency *= lacunarity;
					}

					if (noiseHeight > maxNoiseHeight)
					{
						maxNoiseHeight = noiseHeight;
					}
					else if (noiseHeight < minNoiseHeight)
					{
						minNoiseHeight = noiseHeight;
					}
					noiseMap[x, y] = noiseHeight;
				}
			}

			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{
					noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
				}
			}

			return noiseMap;
		}

	}
}