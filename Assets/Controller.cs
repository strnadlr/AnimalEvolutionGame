using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnimalEvolution
{
    /// <summary>
    /// Delegate
    /// Necessary for communication between UITerrainAndWater and TerrainGenerator.
    /// </summary>
    public delegate bool terrainSetupDelegate(int xsize, int zsize, int yheight, int waterheight, int seed);
    /// <summary>
    /// Delegate
    /// Passes the terrain regeneration function between UITerrainAndWater and TerrainGenerator.
    /// </summary>
    public delegate void terrainRegenerateDelegate();
    /// <summary>
    /// Delegate
    /// Used for all scripts that can be disabled by another script.
    /// </summary>
    public delegate bool boolSwitchDelegate(bool target);
    /// <summary>
    /// Delegate
    /// Used by PlantEntity and AnimalEntity to ask EntityScript to spawn offspring.
    /// </summary>
    public delegate void requestOffspringDelegate(GameObject parent);
    /// <summary>
    /// Delegate
    /// Necessary for communication between PlantEntity and UIEntityCreation
    /// </summary>
    public delegate void plantSetterDelegate(string _name, float _nutritionalValue, float _ticksWithoutChild, float _lifeMax, float _size, int _mutationStrength, Color _color);
    /// <summary>
    /// Delegate
    /// Necessary for communication between AnimalEntity and UIEntityCreation
    /// </summary>
    public delegate void animalSetterDelegate(string _name, float _nutritionalValue, float _ticksWithoutChild, float _lifeMax, float _size, int _mutationStrength, float sences, Color _color, float _speed, float _foodCapacity, float _foodToBreed, bool _isPredator);
    /// <summary>
    /// Delegate
    /// Used by UIEntityInfo to edit properties of PlantEntity or AnimalEntity
    /// </summary>
    public delegate void changeEntityProperties(int property);

    /// <summary>
    /// Main class of the program, sets up correct interactions between all other parts of the code.
    /// </summary>
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
        public UIExit exitUI;
        public UIGuideText guideTextUI;
        private GameObject currentInfoEntity;
        private float entityInfoDelayTime;
        public static float simulationSpeed = 1f;
        public static bool paused = false;
        /// <summary>
        /// Used for determining whether animals and plants are within the boundaries of the map
        /// and above water.
        /// </summary>
        public static float xBoundary, zBoundary, yWaterLevel;

        RaycastHit hit;
        Vector3 mouse;
        Ray ray;

        // Start is called before the first frame update
        void Start()
        {
            terrainGenerator.SetWater(waterGeneration);
            terrainAndWateUI.setupTerrainDelegate = terrainGenerator.SetValues;
            terrainAndWateUI.terrainRegenerate = terrainGenerator.Regenerate;
            terrainAndWateUI.cameraSwitch = cameraController.MovementSwitch;
            entityCreationUI.cameraSwitch = cameraController.MovementSwitch;
            guideTextUI.cameraSwitch = cameraController.MovementSwitch;
            exitUI.cameraSwitch = cameraController.MovementSwitch;
            terrainAndWateUI.entityCreationSwitch = speedControlsUI.EnableEntityCreationSwitch;
            entityCreationUI.speedControlsSwitch = speedControlsUI.EnableSwitch;
            exitUI.speedControlsSwitch = speedControlsUI.EnableSwitch;
            guideTextUI.speedControlsSwitch = speedControlsUI.EnableSwitch;
            guideTextUI.entityCreationSwitch = entityCreationUI.EnableSwitch;
            guideTextUI.entityInfoSwitch = entityInfoUI.EnableSwitch;
            guideTextUI.terrainCreationSwitch = terrainAndWateUI.EnableSwitch;
            entityScript.Initialize(terrainGenerator.meshCollider);
            entityInfoUI.EntityInfoUIXButtonClicked();
            Methods.SetUpLog();
        }

        // Update is called once per frame
        void Update()
        {
            entityInfoDelayTime += Time.deltaTime;
            if (entityCreationUI.placing && (Input.GetMouseButtonDown(0)))
            {
                mouse = Input.mousePosition;
                ray = mainCamera.ScreenPointToRay(mouse);

                if (Physics.Raycast(ray, out hit, 1 << 10))
                {
                    if (entityCreationUI.isPlant)
                    {
                        PlantEntity newPlant = entityScript.PlacePlantAt(hit.point);
                        entityCreationUI.plantSetterDelegate = newPlant.Set;
                        entityCreationUI.PropagateEntityInfo();
                    }
                    else
                    {
                        AnimalEntity newAnimal = entityScript.PlaceAnimalAt(hit.point, hit.normal, entityCreationUI.isCarnivore);
                        entityCreationUI.animalSetterDelegate = newAnimal.Set;
                        entityCreationUI.PropagateEntityInfo();
                    }
                }
                entityCreationUI.placing = false;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                mouse = Input.mousePosition;
                ray = mainCamera.ScreenPointToRay(mouse);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag != "UI")
                    {
                        if (currentInfoEntity != null)
                        {
                            currentInfoEntity.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
                        }

                        currentInfoEntity = hit.collider.gameObject;
                        currentInfoEntity.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.white);
                        SetEntityInfoPanel();
                        UpdateEntityInfoPanel();
                    }
                }
                else
                {
                    if (currentInfoEntity != null)
                    {
                        currentInfoEntity.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
                    }
                    currentInfoEntity = null;
                    UpdateEntityInfoPanel();
                }
            }
            if (entityInfoUI.isActiveAndEnabled && entityInfoDelayTime > 0.2)
            {
                UpdateEntityInfoPanel();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                exitUI.ResumeButtonClicked();
            }
        }

        /// <summary>
        /// if currentInfoEntity has an Entity attached to it, propagets the entity's ChangeMyProperties and
        /// if it's a plant, deactivated the predator-specific buttons.
        /// </summary>
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

        /// <summary>
        /// Requests a new string from the currentInfoEntity, if it hasn't been destroyed yet.
        /// </summary>
        private void UpdateEntityInfoPanel()
        {
            if (currentInfoEntity != null)
            {
                Entity e = currentInfoEntity.GetComponent<Entity>();
                if (e != null)
                {
                    entityInfoUI.DisplayText(e.ToString());
                    entityInfoDelayTime = 0;
                }
                else
                {
                    entityInfoUI.EntityInfoUIXButtonClicked();
                }

            }
            else
            {
                entityInfoUI.EntityInfoUIXButtonClicked();
            }

        }

    }
}