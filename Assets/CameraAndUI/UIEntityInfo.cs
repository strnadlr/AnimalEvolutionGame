using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AnimalEvolution
{
    public class UIEntityInfo : MonoBehaviour
    {
        public Text entityInfoText;
        public Button feedButton;
        public Button starveButton;
        public changeEntityProperties changeTarget;

        public GameObject panel;
        private bool active=false;

        /// <summary>
        /// The Button used to open the UI
        /// </summary>
        public void EntityInfoUIXButtonClicked()
        {
            active = !active;
            panel.SetActive(active);
        }

        /// <summary>
        /// Update the entityInfoText to the passed string.
        /// </summary>
        /// <param name="text"> Text to display</param>
        public void DisplayText(string text)
        {
            entityInfoText.text = text;
            active = true;
            panel.SetActive(active);
        }

        /// <summary>
        /// Increases the viewed entity's current remaining life
        /// </summary>
        public void RejuvinateButtonClicked()
        {
            changeTarget(0);
        }

        /// <summary>
        /// Increases the viewed entity's current food 
        /// </summary>
        public void FeedButtonClicked()
        {
            changeTarget(1);
        }

        /// <summary>
        /// Decreases the viewed entity's current food
        /// </summary>
        public void StarveButtonClicked()
        {
            changeTarget(2);
        }

        /// <summary>
        /// Kills the viewed entity
        /// </summary>
        public void KillButtonClicked()
        {
            changeTarget(3);
            active = false;
            panel.SetActive(active);
        }

        /// <summary>
        /// Creates an offspring for the viewed entity with no respect to the viewed entity's current breeding status.
        /// </summary>
        public void BreedButtonClicked()
        {
            changeTarget(4);
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
    }
}