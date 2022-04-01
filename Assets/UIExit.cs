using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AnimalEvolution
{
    public class UIExit : MonoBehaviour
    {
        public BoolSwitchDelegate cameraSwitch;
        public BoolSwitchDelegate speedControlsSwitch;
        public GameObject panel;
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
            Controller.paused = active;
        }

        public void QuitButtonClicked()
        {
            Application.Quit();
        }
    }
}