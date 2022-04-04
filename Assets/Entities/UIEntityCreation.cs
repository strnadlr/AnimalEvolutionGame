﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnimalEvolution
{
    public class UIEntityCreation : MonoBehaviour
    {
        public Button plantButton;
        public Button animalButton;
        public Button carnivoreButton;
        public Button herbivoreButton;
        public Text nutriValueText;
        public Text timeBeOfText;
        public Text lifeMaxText;
        public Text sizeText;
        public Text mutationStrText;
        public Text sensesText;
        public Text speedText;
        public Text foodCapacityText;
        public Text foodToBreedText;
        public Slider sensesSlider;
        public Slider speedSlider;
        public Slider foodCapacitySlider;
        public Slider foodToBreedSlider;
        public RawImage colorRawImage;
        public RawImage saturationColorRawImage;
        public RawImage valueColorRawImage;
        public RawImage BWSaturationColorRawImage;
        public plantPlacerDelegate plantPlacer;
        public BoolSwitchDelegate cameraSwitch;
        public BoolSwitchDelegate speedControlsSwitch;
        public plantSetterDelegate plantSetterDelegate;
        public animalSetterDelegate animalSetterDelegate;

        private string newName = "New Species";
        private int nutriValue = 50;
        private float timeBeOf = 3;
        private float lifeMax = 60;
        private float size = 1f;
        private int mutationStr = 25;
        private float senses = 50;
        public float hue = 138 / 360;
        public float saturation = 77 / 100;
        public float value = 90 / 100;
        private Color color;
        private float speed = 10;
        private float foodCapacity = 100;
        private float foodToBreed = 70;
        public bool isCarnivore = false;
        public bool placing = false;
        public bool isPlant = true;

        public GameObject panel;
        private bool active = false;

        // Start is called before the first frame update
        void Start()
        {
            panel.SetActive(active);
        }

        public void EntityUIButtonClicked()
        {
            active = !active;
            panel.SetActive(active);
            cameraSwitch(!active);
            speedControlsSwitch(!active);
        }

        public void plantButtonClicked()
        {
            isPlant = true;
            plantButton.GetComponent<Image>().color = Color.green;
            animalButton.GetComponent<Image>().color = Color.red;
            sensesSlider.interactable = false;
            speedSlider.interactable = false;
            foodCapacitySlider.interactable = false;
            foodToBreedSlider.interactable = false;
            carnivoreButton.interactable = false;
            herbivoreButton.interactable = false;
        }

        internal bool EnableSwitch(bool target)
        {
            bool prev = active;
            active = target;
            panel.SetActive(active);
            return prev;
        }

        public void animalButtonClicked()
        {
            isPlant = false;
            animalButton.GetComponent<Image>().color = Color.green;
            plantButton.GetComponent<Image>().color = Color.red;
            sensesSlider.interactable = true;
            speedSlider.interactable = true;
            foodCapacitySlider.interactable = true;
            foodToBreedSlider.interactable = true;
            carnivoreButton.interactable = true;
            herbivoreButton.interactable = true;
        }

        public void NameFieldChanged(string newname)
        {
            newName = newname;
        }

        public void NutriValueSliderChanged(float newnutriValue)
        {
            nutriValue = (int)newnutriValue;
            nutriValueText.text = nutriValue.ToString();
        }

        public void TimeBeOfSliderChanged(float newtimeBeOf)
        {
            timeBeOf = newtimeBeOf/2;
            timeBeOfText.text = timeBeOf.ToString("F");
        }

        public void MaxLifeChanged(float newlifeMax)
        {
            lifeMax = newlifeMax*10;
            lifeMaxText.text = lifeMax.ToString();
        }

        public void SizeSliderChanged(float newsize)
        {
            size = newsize/4;
            sizeText.text = size.ToString("F");
        }

        public void MutationStrSliderChanged(float newmutationStr)
        {
            mutationStr = (int)newmutationStr;
            mutationStrText.text = mutationStr.ToString();
        }

        public void HueSliderChanged(float newhue)
        {
            hue = newhue / 360f;
            color = Color.HSVToRGB(hue, saturation, value);
            colorRawImage.color = color;
            valueColorRawImage.color = Color.HSVToRGB(hue, saturation, 1);
            saturationColorRawImage.color = Color.HSVToRGB(hue, 1, value);
        }

        public void SaturationStrSliderChanged(float newsaturation)
        {
            saturation = newsaturation / 100f;
            color = Color.HSVToRGB(hue, saturation, value);
            colorRawImage.color = color;
            valueColorRawImage.color = Color.HSVToRGB(hue, saturation, 1);
        }

        public void ValueSliderChanged(float newvalue)
        {
            value = newvalue / 100f;
            color = Color.HSVToRGB(hue, saturation, value);
            colorRawImage.color = color;
            saturationColorRawImage.color = Color.HSVToRGB(hue, 1, value);
            BWSaturationColorRawImage.color = Color.HSVToRGB(hue, 0, value);
        }

        public void SensesSliderChanged(float newsenses)
        {
            senses = (int)newsenses*5;
            sensesText.text = senses.ToString();
        }

        public void SpeedSliderChanged(float newspeed)
        {
            speed = (int)newspeed;
            speedText.text = speed.ToString();
        }

        public void FoodCapacitySliderChanged(float newfoodCapacity)
        {
            foodCapacity = (int)newfoodCapacity*5;
            foodCapacityText.text = foodCapacity.ToString();
        }

        public void FoodToBreedSliderChanged(float newfoodToBreed)
        {
            foodToBreed = (int)newfoodToBreed*5;
            foodToBreedText.text = foodToBreed.ToString();
        }

        public void CarnivoreButtonClicked()
        {
            isCarnivore = true;
            carnivoreButton.GetComponent<Image>().color = Color.green;
            herbivoreButton.GetComponent<Image>().color = Color.red;
        }

        public void HerbivoreButtonClicked()
        {
            isCarnivore = false;
            herbivoreButton.GetComponent<Image>().color = Color.green;
            carnivoreButton.GetComponent<Image>().color = Color.red;
        }

        public void CreateAncestorButtonClicked()
        {
            placing = true;
            color = Color.HSVToRGB(hue, saturation, value);
            EntityUIButtonClicked();
        }

        public void propagateEntityInfo()
        {
            if (isPlant)
            {
                plantSetterDelegate(newName, nutriValue, timeBeOf, lifeMax, size, mutationStr, color);
            }
            else
            {
                animalSetterDelegate(newName, nutriValue, timeBeOf, lifeMax, size, mutationStr, senses, color, speed, foodCapacity,foodToBreed, isCarnivore);
            }
            
        }


    }
}