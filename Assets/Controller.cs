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
    public delegate void animalSetterDelegate(string _name, float _nutritionalValue, float _ticksWithoutChild, float _lifeMax, float _size, int _mutationStrength, float sences, Color _color, float _speed, float _foodCapacity, float _foodToBreed, bool _isPredator);
    public delegate void ChangeMyProperties(int property);


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
        public UISpeedControls speedControlsUI;
        private GameObject currentInfoEntity;
        private float waitTime;
        public static float speed = 1f;
        public static bool paused = false;

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
            terrainAndWateUI.entityCreationSwitch = speedControlsUI.EnableEntityCreationSwitch;
            entityCreationUI.speedControlsSwitch = speedControlsUI.EnableSwitch;
            entityScript.Initialize(terrainGenerator.meshCollider);
            entityInfoUI.EntityInfoUIXButtonClicked();
            AnimalEntity.xBoundary = (terrainGenerator.xsize - 1) * 4;
            AnimalEntity.zBoundary = (terrainGenerator.zsize - 1) * 4;
        }

        // Update is called once per frame
        void Update()
        {
            waitTime += Time.deltaTime;
            if (entityCreationUI.placing && (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)))
            {
                mouse = Input.mousePosition;
                ray = mainCamera.ScreenPointToRay(mouse);

                if (Physics.Raycast(ray, out hit, 1 << 10))
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

                if (Physics.Raycast(ray, out hit, ((1 << 8) | (1 << 9))))
                {
                    currentInfoEntity = hit.collider.gameObject;
                    SetEntityInfoPanel();
                    UpdateEntityInfoPanel();

                }
            }
            if (entityInfoUI.isActiveAndEnabled && waitTime > 0.2)
            {
                UpdateEntityInfoPanel();
            }

        }

        private void SetEntityInfoPanel()
        {
            if (currentInfoEntity != null)
            {
                Entity e = currentInfoEntity.GetComponent<Entity>();
                if (e != null)
                {
                    PlantEntity comp;
                    entityInfoUI.changeTarget = currentInfoEntity.GetComponent<Entity>().ChangeMyProperties;
                    if (currentInfoEntity.TryGetComponent<PlantEntity>(out comp))
                    {
                        entityInfoUI.feedButton.interactable = false;
                        entityInfoUI.starveButton.interactable = false;
                    }
                    else
                    {
                        entityInfoUI.feedButton.interactable = true;
                        entityInfoUI.starveButton.interactable = true;
                    }
                }
            }
        }

        private void UpdateEntityInfoPanel()
        {
            if (currentInfoEntity != null)
            {
                Entity e = currentInfoEntity.GetComponent<Entity>();
                if (e != null)
                {
                    entityInfoUI.displayText(e.ToString());
                    waitTime = 0;
                    return;
                }
            }
            
            entityInfoUI.EntityInfoUIXButtonClicked();
            
        }
    }
}