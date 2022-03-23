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
        public float food { get; set; }
        public float foodCapacity { get; set; }
        private float foodToBreed { get; set; }
        private bool isPredator { get; set; }
        private Color tastyColor;
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
        public static float xBoundary;
        public static float zBoundary;

        /// <summary>
        /// Set used when creating a new species of AnimalEntity.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_nutritionalValue"></param>
        /// <param name="_timeWithoutChildren"></param>
        /// <param name="_lifeMax"></param>
        /// <param name="_size"></param>
        /// <param name="_mutationStrength"></param>
        /// <param name="_sences"></param>
        /// <param name="_color"></param>
        /// <param name="_speed"></param>
        /// <param name="_foodCapacity"></param>
        /// <param name="_foodToBreed"></param>
        /// <param name="_isPredator"></param>
        public void Set(string _name, float _nutritionalValue, float _timeWithoutChildren, float _lifeMax, float _size, int _mutationStrength, float _sences, Color _color, float _speed, float _foodCapacity, float _foodToBreed, bool _isPredator)
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
            speed = _speed;
            foodCapacity = _foodCapacity;
            foodToBreed = _foodToBreed;
            food = foodToBreed / 2f;
            isPredator = _isPredator;
            tastyColor = Color.white;
            gObject.transform.localScale = new Vector3(1.5f + 1.5f * size, 1.5f + 1.5f * size, 4 + 4 * size);
            renderer.SetPropertyBlock(mpb);
            valid = true;
            gObject.SetActive(true);
            targetSet = false;
        } 
        /// <summary>
        /// Set used to create mutated child of an existing animalEntity.
        /// </summary>
        /// <param name="parentEntity"></param>
        /// <param name="targetGObject"></param>
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
                    parent.color.r + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100,
                    parent.color.g + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100,
                    parent.color.b + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100);
                speed = Mathf.Max(0.1f, parent.speed + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100);
                foodCapacity= Mathf.Max(0.1f, parent.foodCapacity + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100);
                foodToBreed = Mathf.Max(0.1f, parent.foodToBreed + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100);
                food = foodToBreed / 2f;
                isPredator = parent.isPredator;
                tastyColor = parent.tastyColor;
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
            food -= Time.deltaTime*2;

            if (lifeRemaining < 0 || food < 0)
            {
                if (gObject != null)
                {
                    Destroy(gObject);
                    Destroy(this);
                }
                return;
            }

            if (!targetSet && wentStraightfor > 0.2)
            {
                Collider[] nearbyEntities=null;
                if (isPredator)
                {
                    //searches for nearby animals
                    nearbyEntities = Physics.OverlapSphere(gObject.transform.position, sences, 0b1000000000);
                }
                else
                {
                    //searches for nearby plants
                    nearbyEntities = Physics.OverlapSphere(gObject.transform.position, sences, 0b100000000);
                }
                if (nearbyEntities.Length > 0)
                {
                    target = selectTarget(nearbyEntities);
                    if (target != null)
                    {
                    direction = target.transform.position - gObject.transform.position;
                    gObject.transform.forward = direction;
                    targetSet = true;
                    }
                }
                /*
                 * Went straight for too long?
                */
                else if (wentStraightfor > 5)
                {
                    direction = (new Vector3((float)rand.NextDouble() * 2f - 1f, 0, (float)rand.NextDouble() * 2f - 1f)).normalized;/*
                    nearbyEntities = Physics.OverlapSphere(gObject.transform.position, sences, 0b1000000000);
                    if (nearbyEntities.Length > 0)
                    {
                        Collider threat = selectThreat(nearbyEntities);
                        if (threat != null)
                        {
                            direction = -(threat.transform.position - gObject.transform.position).normalized;
                        }
                    }*/
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
                    /*
                     * Checking if I can eat my target.
                    */
                    direction=target.transform.position - gObject.transform.position;
                    gObject.transform.forward = direction;
                    if (Vector3.Distance(gObject.transform.position, target.transform.position) < size*2)
                    {
                        food = Mathf.Min(foodCapacity, food + target.GetComponent<Entity>().nutritionalValue);
                        tastyColor = tastyColor.Average(target.GetComponent<Entity>().color);
                        if (target != null)
                        {
                            Destroy(target.gameObject.GetComponent<PlantEntity>());
                            Destroy(target.gameObject);
                        }
                        targetSet = false;
                    }
                }

            }


            // Fix rotation with respect to surface.
            if (allignIn>0.5 && wentStraightfor>0.2)
            {
                RaycastHit hit;
                if (Physics.Raycast(gObject.transform.position + gObject.transform.up, -gObject.transform.up, out hit, 0b10000000000))
                {
                    direction = gameObject.transform.forward;
                    gObject.transform.up = hit.normal;
                    gObject.transform.position = hit.point + hit.normal * gObject.transform.localScale.y / 2;
                    gObject.transform.forward = direction;
                }
                allignIn = 0;
            }
            
            // Move forward.
            gObject.transform.position += gObject.transform.forward * Time.deltaTime * speed;
            wentStraightfor += Time.deltaTime;
            if (gObject.transform.position.x < 0 || gObject.transform.position.z < 0 || gObject.transform.position.x > xBoundary || gObject.transform.position.z > zBoundary)
            {
                gObject.transform.position -= 1.5f * (gObject.transform.forward * Time.deltaTime * speed);
                direction = (-gObject.transform.forward + new Vector3((float)rand.NextDouble() * 0.5f - 0.25f, 0, (float)rand.NextDouble() * 0.5f - 0.25f)).normalized;
                gObject.transform.forward = direction;
            }

            if (populate && valid && food>foodToBreed && currentTimeWithoutChild < 0)
            {
                currentTimeWithoutChild = timeWithoutChildren;
                requestOffspring(gObject);
                food -= foodToBreed / 2f;
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

        Collider selectTarget(Collider[] colliders)
        {
            Collider res = null;
            float minDistance = float.MaxValue;
            float distance = 0;
            if (isPredator)
            {
                foreach (Collider c in colliders)
                {
                    distance = c.GetComponent<Entity>().color.Distance(tastyColor);
                    if (c.GetComponent<Entity>().size <size && distance < minDistance)
                    {
                        res = c;
                        minDistance = distance;
                    }
                }
            }
            else foreach (Collider c in colliders)
            {
                distance = c.GetComponent<Entity>().color.Distance(tastyColor);
                if (distance < minDistance)
                {
                    res = c;
                    minDistance = distance;
                }
            }
            return res;
        }
        /// <summary>
        /// Select the most threatening collider the entity can sense.
        /// </summary>
        /// <param name="colliders">Input array of colliders to check.</param>
        /// <returns>closest larger collider</returns>
        Collider selectThreat(Collider[] colliders)
        {
            Collider res = null;
            float minDistance = float.MaxValue;
            float distance = 0;
            foreach (Collider c in colliders)
            {
                distance = Vector3.Distance(c.transform.position, gObject.transform.position);
                if (c.GetComponent<Entity>().size > size && distance < minDistance)
                {
                    res = c;
                    minDistance = distance;
                }
            }
            return res;
        }

        public override string ToString()
        {
            return $"Name: {name}\nLife remaining: {(lifeRemaining / lifeMax).ToString("P")}\nFood Status:" +
                $" {(food / foodCapacity).ToString("P")}\nNutritional Value: {nutritionalValue}\nTime between children:" +
                $" {(currentTimeWithoutChild / timeWithoutChildren).ToString("P")}\nSize: {size}\nMutation Strength: {mutationStrength}";
        }
    }

}