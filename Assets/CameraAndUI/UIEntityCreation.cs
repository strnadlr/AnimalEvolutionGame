using System;
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
        public boolSwitchDelegate cameraSwitch;
        public boolSwitchDelegate speedControlsSwitch;
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

        /// <summary>
        /// The Button used to open the UI
        /// </summary>
        public void EntityUIButtonClicked()
        {
            active = !active;
            panel.SetActive(active);
            cameraSwitch(!active);
            speedControlsSwitch(!active);
        }

        /// <summary>
        /// Plant selection button.
        /// Turns the plant button green and the animal buton red.
        /// Disables all animal-specific options.
        /// </summary>
        public void PlantButtonClicked()
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

        /// <summary>
        /// Used to allow the UI to be showed (such us when other UI, hiding this one, should be displayed).
        /// </summary>
        /// <param name="target">True if UI should be allowed.</param>
        /// <returns></returns>
        internal bool EnableSwitch(bool target)
        {
            bool prev = active;
            active = target;
            panel.SetActive(active);
            return prev;
        }

        /// <summary>
        /// Animal selection button.
        /// Turns the animal button green and the plant buton red.
        /// Enables all animal-specific options.
        /// </summary>
        public void AnimalButtonClicked()
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

        /// <summary>
        /// Field for the Entity's name
        /// </summary>
        /// <param name="newname">Name of the newly created species.</param>
        public void NameFieldChanged(string newname)
        {
            newName = newname;
        }

        /// <summary>
        /// Slider for the Entity's nutritional value. Updates the nutriValueText.
        /// </summary>
        /// <param name="newnutriValue">Value of the new specie's nutritional value.</param>
        public void NutriValueSliderChanged(float newnutriValue)
        {
            nutriValue = (int)newnutriValue;
            nutriValueText.text = nutriValue.ToString();
        }

        /// <summary>
        /// Slider for the Entity's minimum time between offsprings. Updates the timeBeOfText.
        /// </summary>
        /// <param name="newtimeBeOf">Value of the new specie's time between offspring</param>
        public void TimeBeOfSliderChanged(float newtimeBeOf)
        {
            timeBeOf = newtimeBeOf/2;
            timeBeOfText.text = timeBeOf.ToString("F");
        }

        /// <summary>
        /// Slider for the Entity's maximum life expectancy. Updates the lifeMaxText.
        /// </summary>
        /// <param name="newlifeMax">Value of the new specie's maxim life expectancy</param>
        public void MaxLifeChanged(float newlifeMax)
        {
            lifeMax = newlifeMax*10;
            lifeMaxText.text = lifeMax.ToString();
        }

        /// <summary>
        /// Slider for the Entity's size. Updates the sizeText.
        /// </summary>
        /// <param name="newsize">Value of the new specie's size</param>
        public void SizeSliderChanged(float newsize)
        {
            size = newsize/4;
            sizeText.text = size.ToString("F");
        }

        /// <summary>
        /// Slider for the Entity's mutation strength. Updates the mutationStrText.
        /// </summary>
        /// <param name="newmutationStr">Value of the new specie's mutation strength</param>
        public void MutationStrSliderChanged(float newmutationStr)
        {
            mutationStr = (int)newmutationStr;
            mutationStrText.text = mutationStr.ToString();
        }

        /// <summary>
        /// Color selection 
        /// Slider for the Entity's hue. Updates the color selection parts of the UI.
        /// </summary>
        public void HueSliderChanged(float newhue)
        {
            hue = newhue / 360f;
            color = Color.HSVToRGB(hue, saturation, value);
            colorRawImage.color = color;
            valueColorRawImage.color = Color.HSVToRGB(hue, saturation, 1);
            saturationColorRawImage.color = Color.HSVToRGB(hue, 1, value);
        }

        /// <summary>
        /// Color selection 
        /// Slider for the Entity's saturation. Updates the color selection parts of the UI.
        /// </summary>
        public void SaturationStrSliderChanged(float newsaturation)
        {
            saturation = newsaturation / 100f;
            color = Color.HSVToRGB(hue, saturation, value);
            colorRawImage.color = color;
            valueColorRawImage.color = Color.HSVToRGB(hue, saturation, 1);
        }

        /// <summary>
        /// Color selection 
        /// Slider for the Entity's value. Updates the color selection parts of the UI.
        /// </summary>
        public void ValueSliderChanged(float newvalue)
        {
            value = newvalue / 100f;
            color = Color.HSVToRGB(hue, saturation, value);
            colorRawImage.color = color;
            saturationColorRawImage.color = Color.HSVToRGB(hue, 1, value);
            BWSaturationColorRawImage.color = Color.HSVToRGB(hue, 0, value);
        }

        /// <summary>
        /// Animal only
        /// Slider for the Entity's senses. Updates the sensesText.
        /// </summary>
        /// <param name="newsenses">Value of the new specie's senses</param>
        public void SensesSliderChanged(float newsenses)
        {
            senses = (int)newsenses*5;
            sensesText.text = senses.ToString();
        }

        /// <summary>
        /// Animal only
        /// Slider for the Entity's speed. Updates the speedText.
        /// </summary>
        /// <param name="newspeed">Value of the new specie's speed</param>
        public void SpeedSliderChanged(float newspeed)
        {
            speed = (int)newspeed;
            speedText.text = speed.ToString();
        }

        /// <summary>
        /// Animal only
        /// Slider for the Entity's maximum food capacity. Updates the foodCapacityText.
        /// </summary>
        /// <param name="newfoodCapacity">Value of the new specie's maximum food capacity</param>
        public void FoodCapacitySliderChanged(float newfoodCapacity)
        {
            foodCapacity = (int)newfoodCapacity*5;
            foodCapacityText.text = foodCapacity.ToString();
        }

        /// <summary>
        /// Animal only
        /// Slider for the Entity's food percentage necessary for breeding. Updates the foodToBreedText.
        /// </summary>
        /// <param name="newfoodToBreed">Value of the new specie's percentage of food capacity to be filled to allow breeding</param>
        public void FoodToBreedSliderChanged(float newfoodToBreed)
        {
            foodToBreed = (int)newfoodToBreed*5;
            foodToBreedText.text = foodToBreed.ToString();
        }

        /// <summary>
        /// Animal only
        /// Turns the carnivore button green and the herbivore buton red.
        /// Selects the carnovore shape and behaviour
        /// </summary>
        public void CarnivoreButtonClicked()
        {
            isCarnivore = true;
            carnivoreButton.GetComponent<Image>().color = Color.green;
            herbivoreButton.GetComponent<Image>().color = Color.red;
        }

        /// <summary>
        /// Animal only
        /// Turns the herbivore button green and the carnivore buton red.
        /// Selects the herbivore shape and behaviour
        /// </summary>
        public void HerbivoreButtonClicked()
        {
            isCarnivore = false;
            herbivoreButton.GetComponent<Image>().color = Color.green;
            carnivoreButton.GetComponent<Image>().color = Color.red;
        }

        /// <summary>
        /// Turns on placing, saves the color and closes the UI to allow for placing the created entity
        /// </summary>
        public void CreateAncestorButtonClicked()
        {
            placing = true;
            color = Color.HSVToRGB(hue, saturation, value);
            EntityUIButtonClicked();
        }

        /// <summary>
        /// Calls the relevant *SetterDelegate
        /// </summary>
        public void PropagateEntityInfo()
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