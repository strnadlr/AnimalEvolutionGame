using System.Collections;
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
    public delegate void plantSetterDelegate(string _name, float _nutritionalValue, float _ticksWithoutChild, float _lifeMax, float _size, int _mutationStrength, Color _color);
    public delegate void animalSetterDelegate(string _name, float _nutritionalValue, float _ticksWithoutChild, float _lifeMax, float _size, int _mutationStrength, float sences, Color _color);
    public class Controller : MonoBehaviour
    {
        public TerrainGenerator terrainGenerator;
        public WaterGeneration waterGeneration;
        public UITerrainAndWater terrainAndWateUI;
        public UIEntityCreation entityCreationUI;
        public EntityScript entityScript;
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
            entityCreationUI.plantPlacer = entityScript.PlacePlantAt;
            terrainAndWateUI.cameraSwitch = cameraController.MovementSwitch;
            entityCreationUI.cameraSwitch = cameraController.MovementSwitch;
            entityScript.Initialize(terrainGenerator.meshCollider);
            entityInfoUI.EntityInfoUIXButtonClicked();
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
                    if (entityCreationUI.isPlant)
                    {
                        PlantEntity newPlant = entityScript.PlacePlantAt(hit.point);
                        entityCreationUI.plantSetterDelegate = newPlant.Set;
                        entityCreationUI.propagateEntityInfo();
                    }
                    else
                    {
                        AnimalEntity newAnimal = entityScript.PlaceAnimalAt(hit.point, hit.normal);
                        entityCreationUI.animalSetterDelegate = newAnimal.Set;
                        entityCreationUI.propagateEntityInfo();
                    }
                }
                entityCreationUI.placing = false;
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                mouse = Input.mousePosition;
                ray = mainCamera.ScreenPointToRay(mouse);

                if (Physics.Raycast(ray, out hit))
                {
                    Entity entity = hit.collider.gameObject.GetComponent<PlantEntity>();
                    if (entity != null)
                    {
                        entityInfoUI.displayText(entity.ToString());
                    }
                    else
                    {
                        entity = hit.collider.gameObject.GetComponent<AnimalEntity>();
                        if (entity != null)
                        {
                            entityInfoUI.displayText(entity.ToString());
                        }
                        else
                        {
                            entityInfoUI.EntityInfoUIXButtonClicked();
                        }
                    }
                    
                }
            }
        }
    }
}