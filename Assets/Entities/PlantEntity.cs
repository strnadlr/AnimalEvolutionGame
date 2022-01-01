using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution
{
    public class PlantEntity : MonoBehaviour
    {
        public float nutritionalValue;
        public int ticksWithoutChild;
        public int currentTicksWithoutChild;
        public int childrenToLive;
        public int currentChildren;
        public float size;
        public int mutationStrength;
        public Color color;
        public GameObject gObject;
        private static System.Random rand = new System.Random();
        private MaterialPropertyBlock mpb;
        public static requestOffspringDelegate requestOffspring;
        public static bool populate;

        public void SetFrom(PlantEntity parent, GameObject targetGObject)
        {
            gObject = targetGObject;
            name = parent.name;
            nutritionalValue = Mathf.Max(1f,(float)parent.nutritionalValue * (1+(float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100));
            ticksWithoutChild = Mathf.Max(200,(int)((float)parent.ticksWithoutChild * (1+(float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100)));
            currentTicksWithoutChild = 0;
            childrenToLive = parent.childrenToLive;
            currentChildren = 0;
            size = Mathf.Max(0.1f,parent.size + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100);
            mutationStrength = parent.mutationStrength + rand.Next(-parent.mutationStrength, parent.mutationStrength) / 5;
            color = new Color(
                parent.color.r + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100,
                parent.color.g + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100,
                parent.color.b + (float)rand.Next(-parent.mutationStrength, parent.mutationStrength) / 100);
            if (mpb == null) mpb = new MaterialPropertyBlock();
            Renderer renderer = GetComponentInChildren<Renderer>();
            mpb.SetColor("_Color", color);
            gObject.transform.localScale = new Vector3(1 + 1 * size, 5 + 5 * size, 1 + 1 * size);
            renderer.SetPropertyBlock(mpb);
        }



        // Update is called once per frame
        void Update()
        {
            if (populate)
            {
                if (currentTicksWithoutChild < ticksWithoutChild)
                {
                    currentTicksWithoutChild++;
                }
                else
                {
                    currentTicksWithoutChild = 0;
                    currentChildren += 1;
                    requestOffspring(gObject);
                    if (currentChildren > childrenToLive)
                    {
                        Destroy(gObject);
                        Destroy(this);
                    }
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
    }
}