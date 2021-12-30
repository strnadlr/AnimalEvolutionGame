using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution
{
    public class PlantScript : MonoBehaviour
    {

        public GameObject plantPrototype;
        List<GameObject> plants;
        int i = 0;
        static MeshCollider ground;
        bool initialized = false;
        static Vector3 down = new Vector3(0, -1, 0);

        // Start is called before the first frame update
        void Start()
        {
            plants = new List<GameObject>();
            plants.Add(plantPrototype);
            PlantEntity pE = plantPrototype.GetComponent<PlantEntity>();
            pE.color = Color.green;
            pE.mutationStrength = 20;
            pE.plant = plantPrototype;
        }

        public void Initialize(MeshCollider setground)
        {
            ground = setground;
            initialized = true;
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


        // Update is called once per frame
        void Update()
        {
            if (!initialized)
            {
                return;
            }
            i++;
            System.Random r = new System.Random();
            if (Input.GetKey("x") && i > 10)
            {
                GameObject newPlant = Instantiate(plantPrototype);
                GameObject parent = plants[r.Next(plants.Count)];
                PlantEntity pE = parent.GetComponent<PlantEntity>();
                PlantEntity nPE = newPlant.GetComponent<PlantEntity>();
                nPE.SetFrom(pE, newPlant);

                newPlant.transform.position = new Vector3(parent.transform.position.x + r.Next(-50, 50), 300, parent.transform.position.z + r.Next(-50, 50));
                

                Ray ray = new Ray(newPlant.transform.position, -newPlant.transform.up);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    newPlant.transform.position = hit.point;
                    newPlant.GetComponent<Renderer>().enabled = true;
                    plants.Add(newPlant);
                }
                else
                {
                    Destroy(newPlant);
                    Destroy(nPE);
                }

            }
            /*

            List < GameObject > newPlants= new List<GameObject>();
            foreach(GameObject plant in plants)
            {

            }*/
        }

        bool Validate (GameObject plantToValidate)
        {
            Vector3 closestPoint = ground.ClosestPoint(plantToValidate.transform.position);

        }

    }
}