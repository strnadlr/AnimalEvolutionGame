using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate bool setupTerrainDelegate(int xsize, int zsize, int yheight, int waterheight, int seed);
public delegate void terrainRegenerate();

public class Controller : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;
    public WaterGeneration waterGeneration;
    public UITerrainAndWater terrainAndWateUI;
    public PlantScript plantScript;
    

    // Start is called before the first frame update
    void Start()
    {
        terrainGenerator.SetWater(waterGeneration);
        terrainAndWateUI.setupTerrainDelegate = terrainGenerator.SetValues;
        terrainAndWateUI.terrainRegenerate = terrainGenerator.Regenerate;
        plantScript.Iniialize(terrainGenerator.meshCollider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
