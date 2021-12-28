using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnimalEvolution
{
    public class UITerrainAndWater : MonoBehaviour
    {
        public Text widthText;
        public Text lengthText;
        public Text heightText;
        public Text waterText;
        public Text seedText;
        public setupTerrainDelegate setupTerrainDelegate;
        public terrainRegenerate terrainRegenerate;
        public GameObject panel;
        public BoolSwitch cameraSwitch;

        public int width = 100;
        public int length = 100;
        public int height = 40;
        public int water = 20;
        public int seed = 1005001;
        bool seedset = false;
        bool generated = false;

        void Start()
        {
            panel.SetActive(true);
        }
        public void GenerationButtonClicked()
        {
            if (setupTerrainDelegate != null)
            {
                if (!seedset)
                {
                    System.Random r = new System.Random();
                    seed = r.Next(0, int.MaxValue);
                }
                seedText.text = seed.ToString();
                setupTerrainDelegate(width, length, height, water, seed);
                terrainRegenerate();
                generated = true;
            }
        }

        public void ContinueButtonClicked()
        {
            /*
            if (!generated)
            {
                width = 100;
                length = 100;
                height = 40;
                water = 20;
                seed = 1005001;
                seedset = true;
                GenerationButtonClicked();
            }*/
            panel.SetActive(false);
            cameraSwitch(true);
        }

        public void WidthSliderChanged(float newwidth)
        {
            width = (int)newwidth * 5;
            widthText.text = width.ToString();
        }

        public void LengthSliderChanged(float newlength)
        {
            length = (int)newlength * 5;
            lengthText.text = length.ToString();
        }
        public void HeightSliderChanged(float newheight)
        {
            height = (int)newheight * 5;
            heightText.text = height.ToString();
        }
        public void WaterSliderChanged(float newwater)
        {
            water = (int)newwater * 5;
            waterText.text = water.ToString();
        }
        public void SeedFieldChanged(string newSeed)
        {
            if (newSeed != null && newSeed.Length != 0)
            {
                seed = int.Parse(newSeed);
                seedset = true;
            }
            else
            {
                seedset = false;
            }
        }
    }
}
