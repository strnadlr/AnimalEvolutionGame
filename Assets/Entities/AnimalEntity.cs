using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// How far is the animal able to sense it's prefered food source.
        /// </summary>
        public float senses { get; set; }
        /// <summary>
        /// How fast the animal moves.
        /// </summary>
        public float speed { get; set; }
        /// <summary>
        /// The maximum amount of food the animal can eat.
        /// </summary>
        public float foodMax { get; set; }
        /// <summary>
        /// The current amount of food eaten by the animal.
        /// </summary>
        public float foodCurrent { get; set; }
        /// <summary>
        /// The amount of food the animal needs in order to breed.
        /// Hlaf of this amount is consumed upon breeding.
        /// </summary>
        private float foodToBreed { get; set; }
        /// <summary>
        /// True when the animal is a Carnivore
        /// </summary>
        private bool isCarnivore { get; set; }
        public Color color { get; set; }
        public ulong ID { get; set; }
        public bool valid { get; set; }

        private Color tastyColor;
        private float lastMealsNutriValue=0;
        private static System.Random rand = new System.Random();
        private MaterialPropertyBlock mpb;
        public static requestOffspringDelegate requestOffspring;
        public static bool populate;
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
        /// For info about input arguments, see EntityInterface and AnimalEntity
        /// </summary>
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
            foodToBreed = _foodToBreed/100*_foodCapacity;
            foodCurrent = foodToBreed / 2f;
            isCarnivore = _isCarnivore;
            tastyColor = Color.white;
            gameObject.transform.localScale = new Vector3(1.5f + 1.5f * size, 1.5f + 1.5f * size, 4 + 4 * size);
            renderer.SetPropertyBlock(mpb);
            gameObject.SetActive(true);
            targetSet = false;
            if (valid)
            {
                if (isCarnivore)
                {
                    Methods.Log($"{ID}; C; {name}; {nutritionalValue}; {timeToBreedMin}; {lifeMax}; {size}; #{mutationStrength}; {ColorUtility.ToHtmlStringRGB(color)}; " +
                                    $"{senses}; {speed}; {foodMax}; {foodToBreed}; T; #{ColorUtility.ToHtmlStringRGB(tastyColor)}");
                }
                else
                {
                    Methods.Log($"{ID}; C; {name}; {nutritionalValue}; {timeToBreedMin}; {lifeMax}; {size}; {mutationStrength}; #{ColorUtility.ToHtmlStringRGB(color)}; " +
                $"{senses}; {speed}; {foodMax}; {foodToBreed}; F; #{ColorUtility.ToHtmlStringRGB(tastyColor)}");
                }
            }
        }

        public void SetFrom(Entity parentEntity)
        {
            if (parentEntity is AnimalEntity)
            {
                AnimalEntity parent = (AnimalEntity)parentEntity;
                name = parent.name;
                nutritionalValue = Mathf.Max(1f, parent.nutritionalValue * (1 + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f));
                timeToBreedMin = Mathf.Max(0.5f, (parent.timeToBreedMin * (1 + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f)));
                timeToBreedCurrent = timeToBreedMin;
                lifeMax = parent.lifeMax;
                lifeCurrent = lifeMax;
                size = Mathf.Min(Mathf.Max(0.1f, parent.size + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f), 10);
                mutationStrength = parent.mutationStrength + rand.Next(-parent.mutationStrength, parent.mutationStrength) / 5;
                senses = parent.senses + rand.Next(-parent.mutationStrength, parent.mutationStrength) / 5;
                color = new Color(
                    parent.color.r + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f,
                    parent.color.g + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f,
                    parent.color.b + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f);
                speed = Mathf.Max(0.1f, parent.speed + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f);
                foodMax = Mathf.Max(0.1f, parent.foodMax + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f);
                foodToBreed = Mathf.Max(0.1f, parent.foodToBreed + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100f);
                foodCurrent = foodToBreed / 2f;
                isCarnivore = parent.isCarnivore;
                tastyColor = parent.tastyColor;
                if (mpb == null) mpb = new MaterialPropertyBlock();
                Renderer renderer = GetComponentInChildren<Renderer>();
                mpb.SetColor("_Color", color);
                gameObject.transform.localScale = new Vector3(1.5f + 1.5f * size, 1.5f + 1.5f * size, 4 + 4 * size);
                renderer.SetPropertyBlock(mpb);
                valid = true;
                gameObject.SetActive(true);
                //gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
                targetSet = false;
                if (isCarnivore)
                {
                    Methods.Log($"{ID}; C; {name}; {nutritionalValue}; {timeToBreedMin}; {lifeMax}; {size}; {mutationStrength}; #{ColorUtility.ToHtmlStringRGB(color)}; " +
                                    $"{senses}; {speed}; {foodMax}; {foodToBreed}; T; #{ColorUtility.ToHtmlStringRGB(tastyColor)}");
                }
                else
                {
                    Methods.Log($"{ID}; C; {name}; {nutritionalValue}; {timeToBreedMin}; {lifeMax}; {size}; {mutationStrength}; #{ColorUtility.ToHtmlStringRGB(color)}; " +
                $"{senses}; {speed}; {foodMax}; {foodToBreed}; F; #{ColorUtility.ToHtmlStringRGB(tastyColor)}");
                }
            }
        }
        void Update()
        {
            float timePassed = Time.deltaTime;
            if (Controller.paused) return;

            lifeCurrent -= timePassed * Controller.simulationSpeed; 

            if (populate)
            {
                timeToBreedCurrent -= timePassed * Controller.simulationSpeed;
            }
            allignIn += timePassed * Controller.simulationSpeed;
            timeToHungry-= timePassed * Controller.simulationSpeed;
            if (timeToHungry < 0)
            {
                foodCurrent -= timePassed * 2 * Controller.simulationSpeed;
            }

            if (lifeCurrent < 0 || foodCurrent < 0)
            {
                if (gameObject != null)
                {
                    if (lifeCurrent < 0) LogDeath("O");
                    else LogDeath("H");
                    Destroy(gameObject);
                    Destroy(this);
                }
                return;
            }

            if (!targetSet && wentStraightfor > 0.2) //I don't have a target.
            {
                List<Collider> nearbyEntities = null;
                if (isCarnivore)
                {
                    //searches for nearby animals
                    nearbyEntities = (Physics.OverlapSphere(gameObject.transform.position, senses, 0b1000000000)).ToList();
                    if (nearbyEntities.Contains(gameObject.GetComponent<Collider>()))
                    {
                        nearbyEntities.Remove(gameObject.GetComponent<Collider>());
                    }
                }
                else
                {
                    //searches for nearby plants
                    nearbyEntities = (Physics.OverlapSphere(gameObject.transform.position, senses, 0b100000000)).ToList();
                }
                if (nearbyEntities.Count > 0)
                {
                    target = SelectTarget(nearbyEntities);
                    if (target != null)
                    {
                        targetSet = true;
                        targetposORrandDir = target.transform.position;
                        //recalculate = true;
                        gameObject.transform.forward = gameObject.transform.CalculateDirection(targetposORrandDir, gameObject.transform.up);
                    }

                }
                /*
                 * Went straight for too long?
                */
                else if (wentStraightfor > 5)
                {
                    targetposORrandDir = gameObject.transform.position + 200 * (new Vector3((float)rand.NextDouble() * 2f - 1f, 0, (float)rand.NextDouble() * 2f - 1f));
                    targetposORrandDir.x = Mathf.Clamp(targetposORrandDir.x, 1, Controller.xBoundary - 1);
                    targetposORrandDir.z = Mathf.Clamp(targetposORrandDir.z, 1, Controller.zBoundary - 1);
                    wentStraightfor = 0;
                    //recalculate = true;
                    gameObject.transform.forward = gameObject.transform.CalculateDirection(targetposORrandDir, gameObject.transform.up);
                }
            }
            else //I do have a target.
            {
                if (target == null)
                {
                    targetSet = false;
                }
            }

            if (Vector3.Distance(gameObject.transform.position, targetposORrandDir) < size * 3)
            {
                if (targetSet)
                {
                    foodCurrent = Mathf.Min(foodMax, foodCurrent + target.GetComponent<Entity>().nutritionalValue);
                    timeToHungry = 2;
                    tastyColor = tastyColor.Average(target.GetComponent<Entity>().color, lastMealsNutriValue, target.GetComponent<Entity>().nutritionalValue);
                    lastMealsNutriValue = target.GetComponent<Entity>().nutritionalValue;
                    if (target != null)
                    {
                        target.GetComponent<Entity>().LogDeath($"E {ID}");
                        Destroy(target.gameObject.GetComponent<PlantEntity>());
                        Destroy(target.gameObject.GetComponent<AnimalEntity>());
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
                gameObject.transform.position += gameObject.transform.forward * timePassed * speed * Controller.simulationSpeed;
                wentStraightfor += timePassed * Controller.simulationSpeed;
                if (gameObject.transform.position.x <= 0 || gameObject.transform.position.z <= 0 || gameObject.transform.position.x >= Controller.xBoundary || gameObject.transform.position.z >= Controller.zBoundary)
                {
                    Vector3 directionToCenter = (new Vector3(Controller.xBoundary / 2, 0, Controller.zBoundary / 2) - base.gameObject.transform.position).normalized;
                    gameObject.transform.position += (directionToCenter * timePassed * speed);
                    targetposORrandDir = gameObject.transform.position + 5 * (-gameObject.transform.forward + new Vector3((float)rand.NextDouble() * 0.5f - 0.25f, 0, (float)rand.NextDouble() * 0.5f - 0.25f)).normalized;
                    recalculate = true;
                    if (gameObject.transform.position.x <= 0 || gameObject.transform.position.z <= 0 || gameObject.transform.position.x >= Controller.xBoundary || gameObject.transform.position.z >= Controller.zBoundary)
                    {
                        LogDeath("L");
                        Destroy(gameObject);
                        Destroy(this);
                    }
                }

            }

            // Fix rotation with respect to surface.
            if (recalculate || (wentStraightfor > 0.1 && allignIn > 0.3))
            {
                RaycastHit hit;
                if (Physics.Raycast(gameObject.transform.position + 128 * gameObject.transform.up, -gameObject.transform.up, out hit, Mathf.Infinity, layerMask))
                {
                    //Debug.DrawRay(gameObject.transform.position + 128 * gameObject.transform.up, hit.point-(gameObject.transform.position + 128 * gameObject.transform.up), Color.green, 1f, false);
                    gameObject.transform.up = hit.normal;
                    gameObject.transform.position = hit.point + hit.normal * gameObject.transform.localScale.y / 2;
                    gameObject.transform.forward = gameObject.transform.CalculateDirection(targetposORrandDir, hit.normal);
                }
                recalculate = false;
                allignIn = 0;
                if (targetSet) wentStraightfor = 0;
            }
            if (valid && foodCurrent > foodToBreed && timeToBreedCurrent < 0)
            {
                timeToBreedCurrent = timeToBreedMin;
                requestOffspring(gameObject);
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
        private Collider SelectTarget(List<Collider> colliders)
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
            
            result.Append($"\nSize: {size}\nMutation Strength: {mutationStrength}\nID: {ID}");
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
                    LogDeath("K");
                    Destroy(gameObject);
                    Destroy(this);
                    return;
                case 4:
                    timeToBreedCurrent = timeToBreedMin;
                    requestOffspring(gameObject);
                    break;
                default:
                    return;
            }
        }

        public void LogDeath(string cause)
        {
            Methods.Log($"{ID}; D; {cause} ; #{ColorUtility.ToHtmlStringRGB(tastyColor)}");

        }
    }

}