using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AnimalEvolution
{
    public class UIEntityInfo : MonoBehaviour
    {
        public GameObject panel;
        public Text entityInfoText;
        public void EntityInfoUIButtonClicked()
        {
            panel.SetActive(false);
        }

        public void displayText(string text)
        {
            entityInfoText.text = text;
            panel.SetActive(true);
        }

    }
}