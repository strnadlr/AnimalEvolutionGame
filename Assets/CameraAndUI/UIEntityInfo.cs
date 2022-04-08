﻿using System;
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

        public void EntityInfoUIXButtonClicked()
        {
            active = !active;
            panel.SetActive(active);
        }

        public void DisplayText(string text)
        {
            entityInfoText.text = text;
            active = true;
            panel.SetActive(active);
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
            active = false;
            panel.SetActive(active);
        }

        public void BreedButtonClicked()
        {
            changeTarget(4);
        }

        internal bool EnableSwitch(bool target)
        {
            bool prev = active;
            active = target;
            panel.SetActive(active);
            return prev;
        }
    }
}