using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITerrainAndWater : MonoBehaviour
{
    public Slider widthSlider;
    public Text widthText;
    public Slider lengthSlider;
    public Text lengthText;
    public Slider heightSlider;
    public Text heightText;
    public Slider waterSlider;
    public Text waterText;
    public InputField seedField;
    public Text seedText;
    public setupTerrainDelegate setupTerrainDelegate;
    public terrainRegenerate terrainRegenerate;
    public GameObject panel;

    public int width = 100;
    public int length = 100;
    public int height = 40;
    public int water = 20;
    public int seed = 0;
    bool seedset = false;

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
                seed = r.Next(0, 1000);
            }
            seedText.text = seed.ToString();
            setupTerrainDelegate(width, length, height, water, seed);
            terrainRegenerate();
        }
    }

    public void ContinueButtonClicked()
    {
        panel.SetActive(false);
    }

    public void WidthSliderChanged(float newwidth)
    {
        width = (int)newwidth*5;
        widthText.text = width.ToString();
    }

    public void LengthSliderChanged(float newlength)
    {
        length = (int)newlength*5;
        lengthText.text = length.ToString();
    }
    public void HeightSliderChanged(float newheight)
    {
        height = (int)newheight*5;
        heightText.text = height.ToString();
    }
    public void WaterSliderChanged(float newwater)
    {
        water = (int)newwater*5;
        waterText.text = water.ToString();
    }
    public void SeedFieldChanged(string newSeed)
    {
        if (newSeed != null && newSeed.Length!=0)
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
