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

        /// <summary>
        /// Used to allow the UI to be showed (such us when other UI, hiding this one, should be displayed).
        /// </summary>
        /// <param name="target">True if UI should be allowed.</param>
        /// <returns></returns>
        public bool EnableSwitch(bool target)
        {
            bool prev = active;
            active = target;
            panel.SetActive(active);
            return prev;
        }

        /// <summary>
        /// Used to enable/disable the Entity Creation opening button.
        /// </summary>
        /// <param name="target">True if interactions should be allowed.</param>
        /// <returns></returns>
        public bool EnableEntityCreationSwitch(bool target)
        {
            bool prev = entityCreationActive;
            entityCreationActive = target;
            entityCreationButton.interactable = entityCreationActive;
            return prev;
        }

        /// <summary>
        /// Allows to control the speed of the simulation.
        /// </summary>
        /// <param name="speedSliderValue">Speed of the simulation.</param>
        public void SpeedSliderChanged(float speedSliderValue)
        {
            Controller.simulationSpeed = speedSliderValue / 10f;
            currentSpeedText.text = Controller.simulationSpeed.ToString("F1");
        }

        /// <summary>
        /// Pauses/resumes the game and updates the text of the button to show the current status.
        /// </summary>
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