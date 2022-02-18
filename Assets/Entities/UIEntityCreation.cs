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
        public Text nutriValueText;
        public Text timeBeOfText;
        public Text lifeMaxText;
        public Text sizeText;
        public Text mutationStrText;
        public RawImage colorRawImage;
        public RawImage saturationColorRawImage;
        public RawImage valueColorRawImage;
        public RawImage BWSaturationColorRawImage;
        public plantPlacerDelegate plantPlacer;
        public BoolSwitchDelegate cameraSwitch;
        public plantSetterDelegate plantSetterDelegate;

        private string newName = "New Species";
        private int nutriValue = 50;
        private float timeBeOf = 3;
        private float lifeMax = 60;
        private int size = 10;
        private int mutationStr = 25;
        public float hue = 138 / 360;
        public float saturation = 77 / 100;
        public float value = 90 / 100;
        private Color color;
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
        }

        public void plantButtonClicked()
        {
            isPlant = true;
            plantButton.GetComponent<Image>().color = Color.green;
            animalButton.GetComponent<Image>().color = Color.red;
        }

        public void animalButtonClicked()
        {
            isPlant = false;
            animalButton.GetComponent<Image>().color = Color.green;
            plantButton.GetComponent<Image>().color = Color.red;
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
            timeBeOf = newtimeBeOf/10;
            timeBeOfText.text = timeBeOf.ToString("F");
        }

        public void MaxLifeChanged(float newlifeMax)
        {
            lifeMax = newlifeMax*10;
            lifeMaxText.text = lifeMax.ToString();
        }

        public void SizeSliderChanged(float newsize)
        {
            size = (int)newsize;
            sizeText.text = size.ToString();
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
                plantSetterDelegate(newName, nutriValue, timeBeOf, lifeMax, size / 10, mutationStr, color);
            }
            
        }

        private void Update()
        {
        
        }

    }
}