﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnimalEvolution
{
    public delegate bool setupTerrainDelegate(int xsize, int zsize, int yheight, int waterheight, int seed);
    public delegate void terrainRegenerateDelegate();
    public delegate PlantEntity plantPlacerDelegate(Vector3 where);
    public delegate void BoolSwitchDelegate(bool target);
    public delegate void requestOffspringDelegate(GameObject parent);
    public delegate void plantSetterDelegate(string _name, float _nutritionalValue, int _ticksWithoutChild, int _childrenToLive, float _size, int _mutationStrength, Color _color);
    public class Controller : MonoBehaviour
    {
        public TerrainGenerator terrainGenerator;
        public WaterGeneration waterGeneration;
        public UITerrainAndWater terrainAndWateUI;
        public UIEntityCreation entityCreationUI;
        public PlantScript plantScript;
        public CameraController cameraController;
        public Camera mainCamera;
        public UIEntityInfo entityInfoUI;

        RaycastHit hit;
        Vector3 mouse;
        Ray ray;

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
                mouse = Input.mousePosition;
                ray = mainCamera.ScreenPointToRay(mouse);

                if (Physics.Raycast(ray, out hit))
                {
                    PlantEntity newPlant = plantScript.PlacePlantAt(hit.point);
                    entityCreationUI.plantSetterDelegate = newPlant.Set;
                    entityCreationUI.propagatePlantInfo();
                }
                entityCreationUI.placing = false;
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                mouse = Input.mousePosition;
                ray = mainCamera.ScreenPointToRay(mouse);

                if (Physics.Raycast(ray, out hit))
                {
                    PlantEntity plantEntity = hit.collider.gameObject.GetComponent<PlantEntity>();
                    if (plantEntity != null)
                    {
                        entityInfoUI.displayText(plantEntity.ToString());
                    }
                    else
                    {
                        entityInfoUI.EntityInfoUIButtonClicked();
                    }
                }
            }
        }
    }
}