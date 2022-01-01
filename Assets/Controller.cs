using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnimalEvolution
{
    public delegate bool setupTerrainDelegate(int xsize, int zsize, int yheight, int waterheight, int seed);
    public delegate void terrainRegenerateDelegate();
    public delegate void plantPlacerDelegate(Vector3 where);
    public delegate void BoolSwitchDelegate(bool target);
    public delegate void requestOffspringDelegate(GameObject parent);

    public class Controller : MonoBehaviour
    {
        public TerrainGenerator terrainGenerator;
        public WaterGeneration waterGeneration;
        public UITerrainAndWater terrainAndWateUI;
        public UIEntityCreation entityCreationUI;
        public PlantScript plantScript;
        public CameraController cameraController;
        public Camera camera;

        // Start is called before the first frame update
        void Start()
        {
            terrainGenerator.SetWater(waterGeneration);
            terrainAndWateUI.setupTerrainDelegate = terrainGenerator.SetValues;
            terrainAndWateUI.terrainRegenerate = terrainGenerator.Regenerate;
            entityCreationUI.plantPlacer = plantScript.PlacePlantAt;
            terrainAndWateUI.cameraSwitch = cameraController.MovementSwitch;
            entityCreationUI.cameraSwitch = cameraController.MovementSwitch;
            plantScript.Initialize(terrainGenerator.meshCollider);
        }

        // Update is called once per frame
        void Update()
        {
            if (entityCreationUI.placing && (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)))
            {
                RaycastHit hit;
                Vector3 mouse = Input.mousePosition;
                Ray ray = camera.ScreenPointToRay(mouse);

                Debug.DrawRay(ray.origin, ray.direction);

                if (Physics.Raycast(ray, out hit))
                {
                    plantScript.PlacePlantAt(hit.point);
                }
                entityCreationUI.placing = false;
            }


        }
    }
}