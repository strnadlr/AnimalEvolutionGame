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
        private bool active;
        private bool entityCreationActive;

        private void Start()
        {
            entityCreationButton.interactable = entityCreationActive;
        }

        public void EnableSwitch(bool target)
        {
            active = target;
            panel.SetActive(active);
        }

        public void EnableEntityCreationSwitch(bool target)
        {
            entityCreationActive = target;
            entityCreationButton.interactable = entityCreationActive;
        }

        public void SpeedSliderChanged(float speedSliderValue)
        {
            Controller.speed = speedSliderValue / 10f;
            currentSpeedText.text = Controller.speed.ToString("F1");
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