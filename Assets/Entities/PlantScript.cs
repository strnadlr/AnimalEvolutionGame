using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution
{
    public class PlantScript : MonoBehaviour
    {

        public GameObject plantPrototype;
        static List<GameObject> plants;
        int i = 0;
        static MeshCollider ground;
        bool initialized = false;
        static Vector3 down = new Vector3(0, -1, 0);
        static private System.Random r = new System.Random();

        // Start is called before the first frame update
        void Start()
        {
            plants = new List<GameObject>();
            plants.Add(plantPrototype);
            PlantEntity pE = plantPrototype.GetComponent<PlantEntity>();
            pE.nutritionalValue = 20f;
            pE.name = "DefaultPlant";
            pE.ticksWithoutChild = 500;
            pE.currentTicksWithoutChild = 0;
            pE.childrenToLive = 5;
            pE.currentChildren = 0;
            pE.size = 1f;
            pE.mutationStrength = 20;
            pE.color = Color.green;
            pE.gObject = plantPrototype;
            PlantEntity.requestOffspring = MakeOffspring;
        }

        public void Initialize(MeshCollider setground)
        {
            ground = setground;
            initialized = true;
            PlantEntity.populate = true;
        }

        public void PlacePlantAt(Vector3 where)
        {
            System.Random r = new System.Random();
            GameObject newPlant = Instantiate(plantPrototype);
            GameObject parent = plants[r.Next(plants.Count)];
            PlantEntity pE = parent.GetComponent<PlantEntity>();
            PlantEntity nPE = newPlant.GetComponent<PlantEntity>();
            nPE.SetFrom(pE, newPlant);

            newPlant.transform.position = where;
            newPlant.GetComponent<Renderer>().enabled = true;
            plants.Add(newPlant);
        }

        public static void MakeOffspring(GameObject parent)
        {
            GameObject newPlant = Instantiate(parent);
            PlantEntity pE = parent.GetComponent<PlantEntity>();
            PlantEntity nPE = newPlant.GetComponent<PlantEntity>();
            nPE.SetFrom(pE, newPlant);

            newPlant.transform.position = new Vector3(parent.transform.position.x + r.Next(-50, 50), 300, parent.transform.position.z + r.Next(-50, 50));


            Ray ray = new Ray(newPlant.transform.position, -newPlant.transform.up);
            RaycastHit hit;
            for (int i = 0; i < 20; i++)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    newPlant.transform.position = hit.point;
                    newPlant.GetComponent<Renderer>().enabled = true;
                    plants.Add(newPlant);
                    break;
                }
                else
                {
                    Destroy(newPlant);
                    Destroy(nPE);
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