using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution {
    public class AnimalEntity : MonoBehaviour, Entity
    {
        public float nutritionalValue { get; set; }
        public float lifeMax { get; set; }
        public float lifeCurrent { get; set; }
        public float timeToBreedMin { get; set; }
        public float timeToBreedCurrent { get; set; }
        public float size { get; set; }
        public int mutationStrength { get; set; }
        public float senses { get; set; }
        public float speed { get; set; }
        public float foodMax { get; set; }
        public float foodCurrent { get; set; }
        private float foodToBreed { get; set; }
        private bool isCarnivore { get; set; }
        public Color color { get; set; }
        private Color tastyColor;
        private float lastMealsNutriValue=0;
        public GameObject gObject;
        private static System.Random rand = new System.Random();
        private MaterialPropertyBlock mpb;
        public static requestOffspringDelegate requestOffspring;
        public static bool populate;
        public bool valid;
        private Collider target;
        private bool targetSet;
        private bool recalculate;
        private float wentStraightfor;
        private float allignIn;
        private float timeToHungry;
        private Vector3 targetposORrandDir;
        private int layerMask = ~((1 << 8) | (1 << 9));

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
        /// <param name="_isCarnivore"></param>
        public void Set(string _name, float _nutritionalValue, float _timeWithoutChildren, float _lifeMax, float _size, int _mutationStrength, float _senses, Color _color, float _speed, float _foodCapacity, float _foodToBreed, bool _isCarnivore)
        {
            name = _name;
            nutritionalValue = _nutritionalValue;
            timeToBreedMin = _timeWithoutChildren;
            timeToBreedCurrent = timeToBreedMin;
            lifeMax = _lifeMax;
            lifeCurrent = _lifeMax;
            size = _size;
            mutationStrength = _mutationStrength;
            senses = _senses;
            color = _color;
            if (mpb == null) mpb = new MaterialPropertyBlock();
            Renderer renderer = GetComponentInChildren<Renderer>();
            mpb.SetColor("_Color", color);
            speed = _speed;
            foodMax = _foodCapacity;
            foodToBreed = _foodToBreed;
            foodCurrent = foodToBreed / 2f;
            isCarnivore = _isCarnivore;
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
                nutritionalValue = Mathf.Max(1f, parent.nutritionalValue * (1 + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f));
                timeToBreedMin = Mathf.Max(0.5f, (parent.timeToBreedMin * (1 + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f)));
                timeToBreedCurrent = timeToBreedMin;
                lifeMax = parent.lifeMax;
                lifeCurrent = lifeMax;
                size = Mathf.Min(Mathf.Max(0.1f, parent.size + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f),10);
                mutationStrength = parent.mutationStrength + rand.Next(-parent.mutationStrength, parent.mutationStrength) / 5;
                senses = parent.senses + rand.Next(-parent.mutationStrength, parent.mutationStrength) / 5;
                color = new Color(
                    parent.color.r + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f,
                    parent.color.g + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f,
                    parent.color.b + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f);
                speed = Mathf.Max(0.1f, parent.speed + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f);
                foodMax= Mathf.Max(0.1f, parent.foodMax + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f);
                foodToBreed = Mathf.Max(0.1f, parent.foodToBreed + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f);
                foodCurrent = foodToBreed / 2f;
                isCarnivore = parent.isCarnivore;
                tastyColor = parent.tastyColor;
                if (mpb == null) mpb = new MaterialPropertyBlock();
                Renderer renderer = GetComponentInChildren<Renderer>();
                mpb.SetColor("_Color", color);
                gObject.transform.localScale = new Vector3(1.5f + 1.5f * size, 1.5f + 1.5f * size, 4 + 4 * size);
                renderer.SetPropertyBlock(mpb);
                valid = true;
                gObject.SetActive(true);
                gObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                targetSet = false;
            }
        }
        void Update()
        {
            float timePassed = Time.deltaTime;
            if (Controller.paused) return;

            lifeCurrent -= timePassed * Controller.speed; 

            if (populate)
            {
                timeToBreedCurrent -= timePassed * Controller.speed;
            }
            allignIn += timePassed * Controller.speed;
            timeToHungry-= timePassed * Controller.speed;
            if (timeToHungry < 0)
            {
                foodCurrent -= timePassed * 2 * Controller.speed;
            }

            if (lifeCurrent < 0 || foodCurrent < 0)
            {
                if (gObject != null)
                {
                    Destroy(gObject);
                    Destroy(this);
                }
                return;
            }

            if (!targetSet && wentStraightfor > 0.2) //I don't have a target.
            {
                Collider[] nearbyEntities = null;
                if (isCarnivore)
                {
                    //searches for nearby animals
                    nearbyEntities = Physics.OverlapSphere(gObject.transform.position, senses, 0b1000000000);
                }
                else
                {
                    //searches for nearby plants
                    nearbyEntities = Physics.OverlapSphere(gObject.transform.position, senses, 0b100000000);
                }
                if (nearbyEntities.Length > 0)
                {
                    target = selectTarget(nearbyEntities);
                    if (target != null)
                    {
                        targetSet = true;
                        targetposORrandDir = target.transform.position;
                        //recalculate = true;
                        gObject.transform.forward = calculateDirection(gObject.transform.position, targetposORrandDir, gObject.transform.up);
                    }

                }
                /*
                 * Went straight for too long?
                */
                else if (wentStraightfor > 5)
                {
                    targetposORrandDir = gObject.transform.position + 200*(new Vector3((float)rand.NextDouble() * 2f - 1f, 0, (float)rand.NextDouble() * 2f - 1f));
                    targetposORrandDir.x = Mathf.Clamp(targetposORrandDir.x, 1, Controller.xBoundary -1);
                    targetposORrandDir.z = Mathf.Clamp(targetposORrandDir.z, 1, Controller.zBoundary -1);
                    wentStraightfor = 0;
                    //recalculate = true;
                    gObject.transform.forward = calculateDirection(gObject.transform.position, targetposORrandDir, gObject.transform.up);
                }
            }
            else //I do have a target.
            {
                if (target == null)
                {
                    targetSet = false;
                }
            }

            if (Vector3.Distance(gObject.transform.position, targetposORrandDir) < size * 3)
            {
                if (targetSet)
                {
                    foodCurrent = Mathf.Min(foodMax, foodCurrent + target.GetComponent<Entity>().nutritionalValue);
                    timeToHungry = 2;
                    tastyColor = tastyColor.Average(target.GetComponent<Entity>().color, lastMealsNutriValue, target.GetComponent<Entity>().nutritionalValue);
                    lastMealsNutriValue = target.GetComponent<Entity>().nutritionalValue;
                    if (target != null)
                    {
                        Destroy(target.gameObject.GetComponent<PlantEntity>());
                        Destroy(target.gameObject);
                    }
                    targetSet = false;
                }
                else
                {
                    wentStraightfor = 6;
                }
            }

            if (foodCurrent < 0.8 * foodMax)
            {
                // Move forward.
                gObject.transform.position += gObject.transform.forward * timePassed * speed * Controller.speed;
                wentStraightfor += timePassed * Controller.speed;
                if (gObject.transform.position.x <= 0 || gObject.transform.position.z <= 0 || gObject.transform.position.x >= Controller.xBoundary || gObject.transform.position.z >= Controller.zBoundary)
                {
                    Vector3 directionToCenter = (new Vector3(Controller.xBoundary / 2, 0, Controller.zBoundary / 2) - gameObject.transform.position).normalized;
                    gObject.transform.position += (directionToCenter * timePassed * speed);
                    targetposORrandDir = gObject.transform.position + 5 * (-gObject.transform.forward + new Vector3((float)rand.NextDouble() * 0.5f - 0.25f, 0, (float)rand.NextDouble() * 0.5f - 0.25f)).normalized;
                    recalculate = true;
                    if (gObject.transform.position.x <= 0 || gObject.transform.position.z <= 0 || gObject.transform.position.x >= Controller.xBoundary || gObject.transform.position.z >= Controller.zBoundary)
                    {
                        Destroy(gObject);
                        Destroy(this);
                    }
                }

            }

            // Fix rotation with respect to surface.
            if (recalculate || (wentStraightfor > 0.1 && allignIn > 0.3))
            {
                RaycastHit hit;
                if (Physics.Raycast(gObject.transform.position + 128 * gObject.transform.up, -gObject.transform.up, out hit, Mathf.Infinity, layerMask))
                {
                    //Debug.DrawRay(gObject.transform.position + 128 * gObject.transform.up, hit.point-(gObject.transform.position + 128 * gObject.transform.up), Color.green, 1f, false);
                    gObject.transform.up = hit.normal;
                    gObject.transform.position = hit.point + hit.normal * gObject.transform.localScale.y / 2;
                    gObject.transform.forward = calculateDirection(gObject.transform.position, targetposORrandDir, hit.normal);
                }
                recalculate = false;
                allignIn = 0;
                if (targetSet) wentStraightfor = 0;
            }
            if (valid && foodCurrent > foodToBreed && timeToBreedCurrent < 0)
            {
                timeToBreedCurrent = timeToBreedMin;
                requestOffspring(gObject);
                foodCurrent -= foodToBreed / 2f;
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
        /// <summary>
        /// select the target that is the closest in color to my tasty Color that is, if requiered, smaller than me.
        /// </summary>
        /// <param name="colliders">list of targets in range</param>
        /// <returns>A single target to pursue, if found.</returns>
        private Collider selectTarget(Collider[] colliders)
        {
            Collider res = null;
            float minDistance = float.MaxValue;
            float distance = 0;
            if (isCarnivore)
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
        private Collider selectThreat(Collider[] colliders)
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

        /// <summary>
        /// Calculate a direction paralel to the surface aiming towards the target.
        /// </summary>
        /// <param name="position">My position</param>
        /// <param name="target">Target's position</param>
        /// <param name="normal">Normal of the surface</param>
        /// <returns></returns>
        private Vector3 calculateDirection(Vector3 position, Vector3 target, Vector3 normal)
        {
            //return target - position;
            Plane surface = new Plane(normal, position);
            Vector3 normalOfCrossplane = Vector3.Cross((target - position).normalized, normal);
            Vector3 result = Vector3.Cross(normal, normalOfCrossplane);
            return result;
        }



        public override string ToString()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            if (isCarnivore)
            {
                result.Append("Carnivore\n");
            }
            else
            {
                result.Append("Herbivore:\n");
            }

            result.Append($"Name: {name}\nLife remaining: {lifeCurrent.ToString("N1")} / {lifeMax.ToString("N1")}\nFood Status:");
            result.Append($" {foodCurrent.ToString("N1")} / { foodMax.ToString("N1")}\nNutritional Value: {nutritionalValue}\nTime between children:");

            if (!populate)
            {
                result.Append(" NOT BREEDING (press X)");
            }
            else if (timeToBreedCurrent < 0)
            {
                result.Append(" HUNGRY");
            }
            else
            {
                result.Append($"{ timeToBreedCurrent.ToString("N1")} / { timeToBreedMin.ToString("N1")}");
            }
            
            result.Append($"\nSize: {size}\nMutation Strength: {mutationStrength}");
            return result.ToString();
        }

        /// <summary>
        /// Edit properties by 10% or complete an action.
        /// </summary>
        /// <param name="property"> 0 add life, 1 add food, 2 remove food, 3 kil, 4 make offspring</param>
        public void ChangeMyProperties(int property)
        {
            switch (property)
            {
                case 0:
                    lifeCurrent = Mathf.Min(lifeCurrent + lifeMax / 10, lifeMax);
                    break;
                case 1:
                    foodCurrent = Mathf.Min(foodCurrent + foodMax / 10, foodMax);
                    break;
                case 2:
                    foodCurrent = Mathf.Max(foodCurrent - foodMax / 10, 0);
                    break;
                case 3:
                    Destroy(gObject);
                    Destroy(this);
                    return;
                case 4:
                    timeToBreedCurrent = timeToBreedMin;
                    requestOffspring(gObject);
                    break;
                default:
                    return;
            }
        }
    }

}