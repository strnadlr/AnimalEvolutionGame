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
        static Vector3 down = new Vector3(0, -1, 0);
        static private System.Random r = new System.Random();

        // Start is called before the first frame update
        void Start()
        {
            entities = new List<GameObject>();
            entities.Add(plantPrototype);
            PlantEntity pE = plantPrototype.GetComponent<PlantEntity>();
            pE.Set("DefaultPlant", 20f, 3, 60, 1, 25, Color.green);
            pE.gObject = plantPrototype;
            pE.valid = false;
            PlantEntity.requestOffspring = MakeOffspring;

            entities.Add(animalHerbivorePrototype);
            AnimalEntity aE = animalHerbivorePrototype.GetComponent<AnimalEntity>();
            aE.Set("DefaultAnimal", 40f, 3, 60, 1, 25, 50, Color.red, 10, 100, 50, false);
            aE.gObject = animalHerbivorePrototype;
            aE.valid = false;
            AnimalEntity.requestOffspring = MakeOffspring;

            entities.Add(animalCarnivorePrototype);
            AnimalEntity aEC = animalCarnivorePrototype.GetComponent<AnimalEntity>();
            aEC.Set("DefaultAnimal", 40f, 3, 60, 1, 25, 50, Color.red, 10, 100, 50, true);
            aEC.gObject = animalCarnivorePrototype;
            aEC.valid = false;
            AnimalEntity.requestOffspring = MakeOffspring;
        }

        public void Initialize(MeshCollider setground)
        {
            ground = setground;
            PlantEntity.populate = true;
            AnimalEntity.populate = true;
        }

        public PlantEntity PlacePlantAt(Vector3 where)
        {
            GameObject newPlant = Instantiate(plantPrototype);
            PlantEntity nPE = newPlant.GetComponent<PlantEntity>();

            newPlant.transform.position = where;
            newPlant.GetComponent<Renderer>().enabled = true;
            entities.Add(newPlant);
            return nPE;
        }

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
            nE.SetFrom(pE, newEntity);

            newEntity.transform.position = new Vector3(parent.transform.position.x + r.Next(-(int)pE.size*50, (int)pE.size * 50), 300, parent.transform.position.z + r.Next(-(int)pE.size * 50, (int)pE.size * 50));


            Ray ray = new Ray(newEntity.transform.position, -newEntity.transform.up);
            RaycastHit hit;
            for (int i = 0; i < 20; i++)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    newEntity.transform.position = hit.point;
                    if (nE is AnimalEntity)
                    {
                        newEntity.transform.up = hit.normal;
                        newEntity.transform.position += hit.normal * newEntity.transform.localScale.y / 2;
                    }
                    if (hit.collider.gameObject.tag=="Ground")
                    {
                        newEntity.GetComponent<Renderer>().enabled = true;
                        entities.Add(newEntity);
                        break;
                    }
                    else
                    {
                        Destroy(newEntity);
                        Destroy((Object)nE);
                    }
                }
                else
                {
                    Destroy(newEntity);
                    Destroy((Object)nE);
                }
            }

        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey("x"))
            {
                PlantEntity.populate = !PlantEntity.populate;
                AnimalEntity.populate = !AnimalEntity.populate;
            }
        }


    }
}