using System;
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
        public InputField seedField;
        public terrainSetupDelegate setupTerrainDelegate;
        public terrainRegenerateDelegate terrainRegenerate;
        public GameObject panel;
        public boolSwitchDelegate cameraSwitch;
        public boolSwitchDelegate entityCreationSwitch;

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

        /// <summary>
        /// Locks in the current values, generates a new seed (if needed) and triggers terrain regeneration.
        /// </summary>
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

        /// <summary>
        /// Locks in the current terrain or substitutes a pre selected one if it's not generated, closes the UI, enables camera and Entiy creation.
        /// Logs the valuses used to generate the current map.
        /// </summary>
        public void ContinueButtonClicked()
        { 
            if (!generated)
            {
                width = 100;
                length = 100;
                height = 40;
                water = 20;
                seed = 1005001;
                seedset = true;
                GenerationButtonClicked();
            }
            panel.SetActive(false);
            cameraSwitch(true);
            entityCreationSwitch(true);
            Methods.Log($"MAP seed: {seed} width: {width} length: {length} height: {height} water: {water}");
        }

        /// <summary>
        /// Slider for the map's width. Updates the widthText.
        /// </summary>
        /// <param name="newwidth">Value of the new map's width.</param>
        public void WidthSliderChanged(float newwidth)
        {
            width = (int)newwidth * 5;
            widthText.text = width.ToString();
        }

        /// <summary>
        /// Slider for the map's leght. Updates the lengthText.
        /// </summary>
        /// <param name="newlength">Value of the new map's length.</param>
        public void LengthSliderChanged(float newlength)
        {
            length = (int)newlength * 5;
            lengthText.text = length.ToString();
        }

        /// <summary>
        /// Slider for the map's height. Updates the heightText.
        /// </summary>
        /// <param name="newheight">Value of the new map's height.</param>
        public void HeightSliderChanged(float newheight)
        {
            height = (int)newheight * 5;
            heightText.text = height.ToString();
        }

        /// <summary>
        /// Slider for the map's water height percentage. Updates the waterText.
        /// </summary>
        /// <param name="newwater">Value of the new map's water hight percentage.</param>
        public void WaterSliderChanged(float newwater)
        {
            water = (int)newwater * 5;
            waterText.text = water.ToString();
        }

        /// <summary>
        /// Field for the map's seed.
        /// </summary>
        /// <param name="newSeed">Value of the new map's seed.</param>
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

        /// <summary>
        /// Copies the last generated seed into the seedField.
        /// </summary>
        public void CopySeedButtonClicked()
        {
            seedset = true;
            seedField.text = seed.ToString();
        }

        /// <summary>
        /// Used to allow the UI to be showed (such us when other UI, hiding this one, should be displayed).
        /// </summary>
        /// <param name="target">True if UI should be allowed.</param>
        /// <returns></returns>
        internal bool EnableSwitch(bool target)
        {

            bool prev = panel.activeInHierarchy;
            panel.SetActive(target);
            return prev;
            
        }
    }
}
