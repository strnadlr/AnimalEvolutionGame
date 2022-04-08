using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AnimalEvolution
{
    public class UIExit : MonoBehaviour
    {
        public boolSwitchDelegate cameraSwitch;
        public boolSwitchDelegate speedControlsSwitch;
        public GameObject panel;
        private bool pausedBefore;
        private bool active = false;

        // Start is called before the first frame update
        void Start()
        {
            panel.SetActive(active);
        }

        public void ResumeButtonClicked()
        {
            active = !active;
            panel.SetActive(active);
            cameraSwitch(!active);
            speedControlsSwitch(!active);
            if (active)
            {
                pausedBefore = Controller.paused;
                Controller.paused = active;
            }
            else
            {
                Controller.paused = pausedBefore;
            }
        }

        public void QuitButtonClicked()
        {
            Application.Quit();
        }
    }
}