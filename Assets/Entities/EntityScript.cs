using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution
{
    public class EntityScript : MonoBehaviour
    {

        public GameObject plantPrototype;
        public GameObject animalHerbivorePrototype;
        public GameObject animalCarnivorePrototype;
        static List<GameObject> entities;
        public static MeshCollider ground;
        static private System.Random r = new System.Random();
        static ulong availableID = 0;

        // Start is called before the first frame update
        void Start()
        {
            PlantEntity pE = plantPrototype.GetComponent<PlantEntity>();
            pE.valid = false;
            pE.Set("DefaultPlant", 20f, 3, 60, 1, 25, Color.green);
            PlantEntity.requestOffspring = MakeOffspring;

            AnimalEntity aE = animalHerbivorePrototype.GetComponent<AnimalEntity>();
            aE.valid = false;
            aE.Set("DefaultAnimal", 40f, 3, 60, 1, 25, 50, Color.red, 10, 100, 50, false);
            AnimalEntity.requestOffspring = MakeOffspring;

            AnimalEntity aEC = animalCarnivorePrototype.GetComponent<AnimalEntity>();
            aEC.valid = false;
            aEC.Set("DefaultAnimal", 40f, 3, 60, 1, 25, 50, Color.red, 10, 100, 50, true);
            AnimalEntity.requestOffspring = MakeOffspring;
            entities = new List<GameObject>();
        }

        public void Initialize(MeshCollider setground)
        {
            ground = setground;
            PlantEntity.populate = true;
            AnimalEntity.populate = true;
        }

        /// <summary>
        /// Creates a new plant gameObject and it's plantEntity and places the game object in the world.
        /// </summary>
        /// <param name="where"> Position on the map for the placement</param>
        /// <returns>The plantEntity script of the placed game object.</returns>
        public PlantEntity PlacePlantAt(Vector3 where)
        {
            GameObject newPlant = Instantiate(plantPrototype);
            PlantEntity nPE = newPlant.GetComponent<PlantEntity>();
            nPE.valid = true;
            nPE.ID = availableID++;

            newPlant.transform.position = where;
            newPlant.GetComponent<Renderer>().enabled = true;
            entities.Add(newPlant);
            return nPE;
        }
        /// <summary>
        /// Creates a new animal gameObject and it's animalEntity and places the game object in the world.
        /// </summary>
        /// <param name="where"> Position on the map for the placement</param>
        /// <param name="orientation"> The normal of the surface the animal is to be placed on.</param>
        /// <param name="isCarnivore"> True if the carnivore shape should be used for the game objec. Otherwise false.</param>
        /// <returns>The animalEntity script of the placed game object.</returns>
        public AnimalEntity PlaceAnimalAt(Vector3 where, Vector3 orientation, bool isCarnivore)
        {
            GameObject newAnimal;
            if (isCarnivore)
            {
                newAnimal = Instantiate(animalCarnivorePrototype);
            }
            else
            {
                newAnimal = Instantiate(animalHerbivorePrototype);
            }
             
            AnimalEntity nAE = newAnimal.GetComponent<AnimalEntity>();
            nAE.valid = true;
            nAE.ID = availableID++;

            newAnimal.transform.position = where;
            newAnimal.transform.up = orientation;
            newAnimal.transform.position += orientation * newAnimal.transform.localScale.y / 2;
            newAnimal.GetComponent<Renderer>().enabled = true;
            entities.Add(newAnimal);
            return nAE;
        }

        /// <summary>
        /// Create an Entity nearby of parent set from parent.
        /// </summary>
        /// <param name="parent">parent Entity</param>
        public static void MakeOffspring(GameObject parent)
        {
            GameObject newEntity = Instantiate(parent);
            Entity pE = parent.GetComponent<Entity>();
            Entity nE = newEntity.GetComponent<Entity>();
            nE.ID = availableID++;
            nE.valid = true;
            nE.SetFrom(pE);
            float newx,newz;
            Ray ray;
            RaycastHit hit;
            bool placed = false;
            for (int i = 0; i < 20; i++)
            {
                newx = parent.transform.position.x + r.Next(-(int)pE.size * 40, (int)pE.size * 40);
                newz = parent.transform.position.z + r.Next(-(int)pE.size * 40, (int)pE.size * 40);
                newEntity.transform.position = new Vector3(newx, 300, newz);
                ray = new Ray(newEntity.transform.position, -newEntity.transform.up);
                if (Physics.Raycast(ray, out hit))
                {
                    newEntity.transform.position = hit.point;
                    if (nE is AnimalEntity)
                    {
                        newEntity.transform.up = hit.normal;
                        newEntity.transform.position += hit.normal * newEntity.transform.localScale.y / 2;
                    }
                    else if (hit.point.y <= Controller.yWaterLevel)
                    {
                        continue;
                    }
                    if (hit.collider.gameObject.tag == "Ground")
                    {
                        newEntity.GetComponent<Renderer>().enabled = true;
                        entities.Add(newEntity);
                        placed = true;
                        break;
                    }
                }
            }
            if (!placed)
            {
                Destroy(newEntity);
                Destroy((Object)nE);
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("x"))
            {
                PlantEntity.populate = !PlantEntity.populate;
                AnimalEntity.populate = !AnimalEntity.populate;
            }
        }


    }
}