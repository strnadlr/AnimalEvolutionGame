using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnimalEvolution
{
    public class UISpeedControls : MonoBehaviour
    {
        public Text currentSpeedText;
        public Text pauseButtonText;
        public Slider speedSlider;
        public Button pauseButton;
        public Button entityCreationButton;

        public GameObject panel;
        private bool active = true;
        private bool entityCreationActive;

        private void Start()
        {
            entityCreationButton.interactable = entityCreationActive;
        }

        public bool EnableSwitch(bool target)
        {
            bool prev = active;
            active = target;
            panel.SetActive(active);
            return prev;
        }

        public bool EnableEntityCreationSwitch(bool target)
        {
            bool prev = entityCreationActive;
            entityCreationActive = target;
            entityCreationButton.interactable = entityCreationActive;
            return prev;
        }

        public void SpeedSliderChanged(float speedSliderValue)
        {
            Controller.simulationSpeed = speedSliderValue / 10f;
            currentSpeedText.text = Controller.simulationSpeed.ToString("F1");
        }

        public void PauseButtonClicked()
        {
            Controller.paused = !Controller.paused;
            if (Controller.paused)
            {
            pauseButtonText.text = ">";
            speedSlider.interactable = false;
            }
            else
            {
                pauseButtonText.text = "| |";
                speedSlider.interactable = true;
            }

        }

    }
}