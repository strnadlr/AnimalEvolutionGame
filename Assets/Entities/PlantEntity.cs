using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution
{
    public class PlantEntity : MonoBehaviour, Entity
    {
        public float nutritionalValue { get; set; }
        public float lifeMax { get; set; }
        public float lifeRemaining { get; set; }
        public float timeWithoutChildren { get; set; }
        public float currentTimeWithoutChild { get; set; }
        public float size { get; set; }
        public int mutationStrength { get; set; }
        public GameObject gObject;
        public Color color { get; set; }
        private static System.Random rand = new System.Random();
        private MaterialPropertyBlock mpb;
        public static requestOffspringDelegate requestOffspring;
        public static bool populate { get; set; }
        public bool valid;

    public void SetFrom(Entity parentEntity, GameObject targetGObject)
        {
            if (parentEntity is PlantEntity)
            {
            PlantEntity parent = (PlantEntity)parentEntity;
            gObject = targetGObject;
            name = parent.name;
            nutritionalValue = Mathf.Max(1f,parent.nutritionalValue * (1+(float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100));
            timeWithoutChildren = Mathf.Max(0.5f,(parent.timeWithoutChildren * (1+(float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100)));
            currentTimeWithoutChild = timeWithoutChildren;
            lifeMax = parent.lifeMax;
            lifeRemaining = lifeMax;
            size = Mathf.Max(0.1f,parent.size + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100);
            mutationStrength = parent.mutationStrength + rand.Next(-parent.mutationStrength, parent.mutationStrength) / 5;
            color = new Color(
                parent.color.r + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 300,
                parent.color.g + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 300,
                parent.color.b + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 300);
            if (mpb == null) mpb = new MaterialPropertyBlock();
            Renderer renderer = GetComponentInChildren<Renderer>();
            mpb.SetColor("_Color", color);
            gObject.transform.localScale = new Vector3(1 + 1 * size, 5 + 5 * size, 1 + 1 * size);
            renderer.SetPropertyBlock(mpb);
            valid = true;
            gObject.SetActive(true);
            }
        }

        public void Set(string _name, float _nutritionalValue, float _timeWithoutChildren, float _lifeMax, float _size, int _mutationStrength, Color _color)
        {
            name = _name;
            nutritionalValue = _nutritionalValue;
            timeWithoutChildren = _timeWithoutChildren;
            currentTimeWithoutChild = timeWithoutChildren;
            lifeMax = _lifeMax;
            lifeRemaining = _lifeMax;
            size = _size;
            mutationStrength = _mutationStrength;
            color = _color;
            if (mpb == null) mpb = new MaterialPropertyBlock();
            Renderer renderer = GetComponentInChildren<Renderer>();
            mpb.SetColor("_Color", color);
            gObject.transform.localScale = new Vector3(1 + 1 * size, 5 + 5 * size, 1 + 1 * size);
            renderer.SetPropertyBlock(mpb);
            valid = true;
            gObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            Collider[] nearbyPlants = Physics.OverlapSphere(gObject.transform.position, size * 20, 0b100000000);
            lifeRemaining -= Time.deltaTime*((nearbyPlants.Length)*3+1);
            currentTimeWithoutChild -= Time.deltaTime;
            if (lifeRemaining < 0)
            {
                Destroy(gObject);
                Destroy(this);
            }
            else if (populate&&valid)
            {
                if (currentTimeWithoutChild < 0)
                {
                    currentTimeWithoutChild = timeWithoutChildren;
                    requestOffspring(gObject);
                }
            }
        }

        private void OnValidate()
        {
            if (mpb == null) mpb = new MaterialPropertyBlock();
            Renderer renderer = GetComponentInChildren<Renderer>();
            //set the color property
            mpb.SetColor("_Color", color);
            //apply propertyBlock to renderer
            renderer.SetPropertyBlock(mpb);
        }

        public override string ToString()
        {
            System.Text.StringBuilder sB = new System.Text.StringBuilder();
            sB.Append("Name: ");
            sB.Append(name);
            sB.Append('\n');
            sB.Append("Life remaining: ");
            sB.Append((lifeRemaining/lifeMax).ToString("F"));
            sB.Append("%\n");
            sB.Append("Nutritional Value: ");
            sB.Append(nutritionalValue);
            sB.Append('\n');
            sB.Append("Time between children: ");
            sB.Append((currentTimeWithoutChild/timeWithoutChildren).ToString("F"));
            sB.Append("%\n");
            sB.Append("Size: ");
            sB.Append(size);
            sB.Append('\n');
            sB.Append("Mutation Strength: ");
            sB.Append(mutationStrength);
            Collider[] nearbyPlants = Physics.OverlapSphere(gObject.transform.position, size*20, 0b100000000);
            sB.Append("\n Plants Nearby: ");
            sB.Append(nearbyPlants.Length);
            return sB.ToString();
        }

    }
}