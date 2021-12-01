using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution
{
    public class PlantEntity : MonoBehaviour
    {
        public string species;
        public int nutritionalValue;
        public int ticksWithoutChild;
        public int currentTicksWithoutChild;
        public float size;
        public int mutationStrength;
        public Color color;
        public GameObject plant;
        private static System.Random rand = new System.Random();
        private MaterialPropertyBlock mpb;

        public void SetFrom(PlantEntity parent, GameObject _plant)
        {
            plant = _plant;
            nutritionalValue = parent.nutritionalValue;
            ticksWithoutChild = parent.ticksWithoutChild;
            currentTicksWithoutChild = 0;
            size = parent.size + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100;
            mutationStrength = parent.mutationStrength + rand.Next(-parent.mutationStrength, parent.mutationStrength) / 5;
            color = new Color(
                parent.color.r + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100,
                parent.color.g + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100,
                parent.color.b + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100);
            if (mpb == null) mpb = new MaterialPropertyBlock();
            Renderer renderer = GetComponentInChildren<Renderer>();
            mpb.SetColor("_Color", color);
            plant.transform.localScale = new Vector3(1 + 1 * size, 5 + 5 * size, 1 + 1 * size);
            renderer.SetPropertyBlock(mpb);
        }

        

        // Update is called once per frame
        void Update()
        {
            if (currentTicksWithoutChild < ticksWithoutChild)
            {
                currentTicksWithoutChild++;
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
    }
}