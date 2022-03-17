using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution {
    public class AnimalEntity : MonoBehaviour, Entity
    {
        public float nutritionalValue { get; set; }
        public float lifeMax { get; set; }
        public float lifeRemaining { get; set; }
        public float timeWithoutChildren { get; set; }
        public float currentTimeWithoutChild { get; set; }
        public float size { get; set; }
        public int mutationStrength { get; set; }
        public float sences { get; set; }
        public float speed { get; set; }
        public GameObject gObject;
        public Color color { get; set; }
        private static System.Random rand = new System.Random();
        private MaterialPropertyBlock mpb;
        public static requestOffspringDelegate requestOffspring;
        public static bool populate { get; set; }
        public bool valid;
        private Collider target;
        private bool targetSet;
        private float wentStraightfor;
        private float allignIn;
        private Vector3 direction;


        public void Set(string _name, float _nutritionalValue, float _timeWithoutChildren, float _lifeMax, float _size, int _mutationStrength, float _sences, Color _color)
        {
            name = _name;
            nutritionalValue = _nutritionalValue;
            timeWithoutChildren = _timeWithoutChildren;
            currentTimeWithoutChild = timeWithoutChildren;
            lifeMax = _lifeMax;
            lifeRemaining = _lifeMax;
            size = _size;
            mutationStrength = _mutationStrength;
            sences = _sences;
            color = _color;
            if (mpb == null) mpb = new MaterialPropertyBlock();
            Renderer renderer = GetComponentInChildren<Renderer>();
            mpb.SetColor("_Color", color);
            gObject.transform.localScale = new Vector3(1.5f + 1.5f * size, 1.5f + 1.5f * size, 4 + 4 * size);
            renderer.SetPropertyBlock(mpb);
            valid = true;
            gObject.SetActive(true);
            targetSet = false;
        }
        public void SetFrom(Entity parentEntity, GameObject targetGObject)
        {
            if (parentEntity is AnimalEntity)
            {
                AnimalEntity parent = (AnimalEntity)parentEntity;
                gObject = targetGObject;
                name = parent.name;
                nutritionalValue = Mathf.Max(1f, parent.nutritionalValue * (1 + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100));
                timeWithoutChildren = Mathf.Max(0.5f, (parent.timeWithoutChildren * (1 + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100)));
                currentTimeWithoutChild = timeWithoutChildren;
                lifeMax = parent.lifeMax;
                lifeRemaining = lifeMax;
                size = Mathf.Max(0.1f, parent.size + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100);
                mutationStrength = parent.mutationStrength + rand.Next(-parent.mutationStrength, parent.mutationStrength) / 5;
                sences = parent.sences + rand.Next(-parent.mutationStrength, parent.mutationStrength) / 5;
                color = new Color(
                    parent.color.r + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 300,
                    parent.color.g + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 300,
                    parent.color.b + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 300);
                if (mpb == null) mpb = new MaterialPropertyBlock();
                Renderer renderer = GetComponentInChildren<Renderer>();
                mpb.SetColor("_Color", color);
                gObject.transform.localScale = new Vector3(1.5f + 1.5f * size, 1.5f + 1.5f * size, 4 + 4 * size);
                renderer.SetPropertyBlock(mpb);
                valid = true;
                gObject.SetActive(true);
                targetSet = false;
            }
        }
        void Update()
        {
            lifeRemaining -= Time.deltaTime;
            currentTimeWithoutChild -= Time.deltaTime;
            allignIn += Time.deltaTime;
            if (lifeRemaining < 0)
            {
                Destroy(gObject);
                Destroy(this);
                return;
            }

            if (!targetSet)
            {
                Collider[] nearbyPlants = Physics.OverlapSphere(gObject.transform.position, sences, 0b100000000);
                if (nearbyPlants.Length > 0)
                {
                    target = nearbyPlants[0];
                    direction = target.transform.position - gObject.transform.position;
                    gObject.transform.forward = direction;
                    targetSet = true;
                }
                else if (wentStraightfor > 5)
                {
                    direction =(new Vector3((float)rand.NextDouble()*2f-1f, 0, (float)rand.NextDouble()*2f-1f)).normalized;
                    gObject.transform.forward = direction;
                    wentStraightfor = 0;
                }
            }
            else
            {
                if (target == null)
                {
                    targetSet = false;
                }
                else
                {
                    direction=target.transform.position - gObject.transform.position;
                    gObject.transform.forward = direction;
                    if (Vector3.Distance(gObject.transform.position, target.transform.position) < this.size*2)
                    {
                        Destroy(target.gameObject.GetComponent<PlantEntity>());
                        Destroy(target.gameObject);
                        targetSet = false;
                    }
                }

            }

            

            if (allignIn>0.5 && wentStraightfor>0.2)
            {
                RaycastHit hit;
                if (Physics.Raycast(gObject.transform.position + gObject.transform.up, -gObject.transform.up, out hit, 0b10000000000))
                {
                    gObject.transform.up = hit.normal;
                    gObject.transform.position = hit.point + hit.normal * gObject.transform.localScale.y / 2;
                    gObject.transform.forward = direction;
                }
                allignIn = 0;
            }
            

            gObject.transform.position += gObject.transform.forward * Time.deltaTime * 10;
            wentStraightfor += Time.deltaTime;




            if (populate && valid && currentTimeWithoutChild < 0)
            {
                currentTimeWithoutChild = timeWithoutChildren;
                requestOffspring(gObject);
            }

        }
/*
       private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.gameObject.layer==8)
            {
                Destroy(collision.collider.gameObject.GetComponent<PlantEntity>());
                Destroy(collision.collider.gameObject);
            }
        }*/
        
        void OnTriggerEnter(Collider other)
        {
            transform.position = transform.position + transform.up * 3;
            if (other.gameObject.layer == 8)
            {
                Destroy(other.gameObject.GetComponent<PlantEntity>());
                Destroy(other.gameObject);
                targetSet = false;
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
            sB.Append((lifeRemaining / lifeMax).ToString("F"));
            sB.Append("%\n");
            sB.Append("Nutritional Value: ");
            sB.Append(nutritionalValue);
            sB.Append('\n');
            sB.Append("Time between children: ");
            sB.Append((currentTimeWithoutChild / timeWithoutChildren).ToString("F"));
            sB.Append("%\n");
            sB.Append("Size: ");
            sB.Append(size);
            sB.Append('\n');
            sB.Append("Mutation Strength: ");
            sB.Append(mutationStrength);
            Collider[] nearbyPlants = Physics.OverlapSphere(gObject.transform.position, sences, 0b100000000);
            sB.Append("\n Plants Nearby: ");
            sB.Append(nearbyPlants.Length);
            sB.Append("\n Has target: ");
            sB.Append(targetSet);
            return sB.ToString();
        }
    }

}