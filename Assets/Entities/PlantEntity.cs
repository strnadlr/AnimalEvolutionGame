﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution
{
    public class PlantEntity : MonoBehaviour, Entity
    {
        public float nutritionalValue { get; set; }
        public float lifeMax { get; set; }
        public float lifeCurrent { get; set; }
        public float timeToBreedMin { get; set; }
        public float timeToBreedCurrent { get; set; }
        public float size { get; set; }
        public int mutationStrength { get; set; }
        public Color color { get; set; }
        public ulong ID { get; set; }
        public bool valid { get; set; }

        private static System.Random rand = new System.Random();
        private MaterialPropertyBlock mpb;
        public static requestOffspringDelegate requestOffspring;
        public static bool populate;
        private Collider[] nearbyPlants;

        public void SetFrom(Entity parentEntity)
        {
            if (parentEntity is PlantEntity)
            {
                PlantEntity parent = (PlantEntity)parentEntity;
                name = parent.name;
                nutritionalValue = Mathf.Max(1f, parent.nutritionalValue * (1 + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100));
                timeToBreedMin = Mathf.Max(0.5f, (parent.timeToBreedMin * (1 + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100)));
                timeToBreedCurrent = timeToBreedMin;
                lifeMax = parent.lifeMax;
                lifeCurrent = lifeMax;
                size = Mathf.Min(Mathf.Max(0.1f, parent.size + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100), 10);
                mutationStrength = parent.mutationStrength + rand.Next(-parent.mutationStrength, parent.mutationStrength) / 5;
                color = new Color(
                    parent.color.r + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100,
                    parent.color.g + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100,
                    parent.color.b + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100);
                if (mpb == null) mpb = new MaterialPropertyBlock();
                Renderer renderer = GetComponentInChildren<Renderer>();
                mpb.SetColor("_Color", color);
                gameObject.transform.localScale = new Vector3(1 + 1 * size, 5 + 5 * size, 1 + 1 * size);
                renderer.SetPropertyBlock(mpb);
                gameObject.SetActive(true);
                gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
                Methods.Log($"{ID}; C; {name}; {nutritionalValue}; {timeToBreedMin}; {lifeMax}; {size}; {mutationStrength}; #{ColorUtility.ToHtmlStringRGB(color)}");
            }
        }
        /// <summary>
        /// Set used when creating a new species of PlantEntity.
        /// For info about input arguments, see EntityInterface and PlantEntity
        /// </summary>
        public void Set(string _name, float _nutritionalValue, float _timeWithoutChildren, float _lifeMax, float _size, int _mutationStrength, Color _color)
        {
            name = _name;
            nutritionalValue = _nutritionalValue;
            timeToBreedMin = _timeWithoutChildren;
            timeToBreedCurrent = timeToBreedMin;
            lifeMax = _lifeMax;
            lifeCurrent = _lifeMax;
            size = _size;
            mutationStrength = _mutationStrength;
            color = _color;
            if (mpb == null) mpb = new MaterialPropertyBlock();
            Renderer renderer = GetComponentInChildren<Renderer>();
            mpb.SetColor("_Color", color);
            gameObject.transform.localScale = new Vector3(1 + 1 * size, 5 + 5 * size, 1 + 1 * size);
            renderer.SetPropertyBlock(mpb);
            gameObject.SetActive(true);
            if (valid)
            {
                Methods.Log($"{ID}; C; {name}; {nutritionalValue}; {timeToBreedMin}; {lifeMax}; {size}; {mutationStrength}; #{ColorUtility.ToHtmlStringRGB(color)}");
            }
        }

        // Update is called once per frame
        void Update()
        {
            float timePassed = Time.deltaTime;
            if (Controller.paused) return;

            nearbyPlants = Physics.OverlapSphere(gameObject.transform.position, size * 20, 0b100000000);
            lifeCurrent -= timePassed * (nearbyPlants.Length) * Controller.simulationSpeed;
            if (populate)
            {
                timeToBreedCurrent -= timePassed * Controller.simulationSpeed;
            }
            if (lifeCurrent < 0)
            {
                LogDeath("O");
                Destroy(this);
                Destroy(gameObject);
            }
            else if (timeToBreedCurrent <= 0 && nearbyPlants.Length < 5 && valid)
            {
                timeToBreedCurrent = timeToBreedMin;
                requestOffspring(gameObject);
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
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            result.Append($"Name: {name}\nLife remaining: {lifeCurrent.ToString("N1")} / {lifeMax.ToString("N1")}\nNutritional Value: {nutritionalValue}\nTime between children:");
            if (!populate)
            {
                result.Append(" NOT BREEDING (press X)");
            }
            else if (nearbyPlants.Length >= 5)
            {
                result.Append(" TOO MANY PLANTS NEARBY");
            }
            else{
                result.Append($"{ timeToBreedCurrent.ToString("N1")} / { timeToBreedMin.ToString("N1")}");
            }
            result.Append( $"\nSize: {size}\nMutation Strength: {mutationStrength}\nID: {ID}");
            return result.ToString();
        }

        /// <summary>
        /// Edit properties by 10% or complete an action.
        /// </summary>
        /// <param name="property"> 0 add life, 1-2 left for consistency, inaplicable, 3 kil, 4 make offspring</param>
        public void ChangeMyProperties(int property)
        {
            switch (property)
            {
                case 0:
                    lifeCurrent = Mathf.Min(lifeCurrent + lifeMax / 10, lifeMax);
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
            Methods.Log($"{ID}; D; {cause}");
        }
    }
}