using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseDumy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Main()
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\out.txt"))
        {
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    float perlin = Mathf.PerlinNoise(x, z) * 10 - 5;
                    file.Write((int)perlin + ' ');
                }
                file.WriteLine();
            }
        }

    }
}
