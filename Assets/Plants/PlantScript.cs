using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{

    public GameObject plantPrototype;
    List<GameObject> plants;
    int i = 0;
    static MeshCollider ground;
    bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        plants = new List<GameObject>();
        plants.Add(plantPrototype);
    }

    public void Iniialize(MeshCollider setground)
    {
        ground = setground;
        initialized = true;
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
        if (Input.GetKey("x")&& i>1000)
        {
            GameObject newPlant = Instantiate(plantPrototype);
            GameObject parent = plants[r.Next(plants.Count)];
            newPlant.transform.position = new Vector3(parent.transform.position.x+r.Next(-50,50), parent.transform.position.y, parent.transform.position.z+ r.Next(-50,50));
            Vector3 newPos = ground.ClosestPoint(newPlant.transform.position);
            newPlant.transform.position = newPos;
            newPlant.GetComponent<Renderer>().enabled = true;
            plants.Add(newPlant);
        }
        /*
        
        List < GameObject > newPlants= new List<GameObject>();
        foreach(GameObject plant in plants)
        {

        }*/
    }
}
