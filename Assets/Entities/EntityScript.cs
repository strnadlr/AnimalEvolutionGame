using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution
{
    public class EntityScript : MonoBehaviour
    {

        public GameObject plantPrototype;
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
            pE.Set("DefaultPlant", 20f, 1000, 5, 1, 20, Color.green);
            pE.gObject = plantPrototype;
            pE.valid = false;
            PlantEntity.requestOffspring = MakeOffspring;
        }

        public void Initialize(MeshCollider setground)
        {
            ground = setground;
            PlantEntity.populate = true;
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

        /*
        public void PlacePlantAt(Vector3 where, string name, float nutritionalValue, int ticksWithoutChild, int childrenToLive, float size , int mutationStrength, Color color)
        {
            System.Random r = new System.Random();
            GameObject newPlant = Instantiate(plantPrototype);
            PlantEntity nPE = newPlant.GetComponent<PlantEntity>();

            nPE.Set(name, nutritionalValue, ticksWithoutChild, childrenToLive, size, mutationStrength, color);
            nPE.gObject = newPlant;

            newPlant.transform.position = where;
            newPlant.GetComponent<Renderer>().enabled = true;
            plants.Add(newPlant);
        }*/

        public static void MakeOffspring(GameObject parent)
        {
            GameObject newEntity = Instantiate(parent);
            Entity pE = parent.GetComponent<Entity>();
            Entity nE = newEntity.GetComponent<Entity>();
            nE.SetFrom(pE, newEntity);

            newEntity.transform.position = new Vector3(parent.transform.position.x + r.Next(-50, 50), 300, parent.transform.position.z + r.Next(-50, 50));


            Ray ray = new Ray(newEntity.transform.position, -newEntity.transform.up);
            RaycastHit hit;
            for (int i = 0; i < 20; i++)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    newEntity.transform.position = hit.point;
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
            }
        }


    }
}