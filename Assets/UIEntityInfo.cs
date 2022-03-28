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
        public Button feedButton;
        public Button starveButton;
        public ChangeMyProperties changeTarget;
        public void EntityInfoUIXButtonClicked()
        {
            panel.SetActive(false);
        }

        public void displayText(string text)
        {
            entityInfoText.text = text;
            panel.SetActive(true);
        }

        public void RejuvinateButtonClicked()
        {
            changeTarget(0);
        }

        public void FeedButtonClicked()
        {
            changeTarget(1);
        }

        public void StarveButtonClicked()
        {
            changeTarget(2);
        }

        public void KillButtonClicked()
        {
            changeTarget(3);
            panel.SetActive(false);
        }

        public void BreedButtonClicked()
        {
            changeTarget(4);
        }

    }
}