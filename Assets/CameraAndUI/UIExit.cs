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

        /// <summary>
        /// Used to hide this UI and show/enable all other UI and camera.
        /// </summary>
        public void ResumeButtonClicked()
        {
            active = false;
            ActivitySwitch();
        }

        /// <summary>
        /// Used to show this UI and hide/diable all other UI and camera.
        /// </summary>
        public void showUIExit()
        {
            active = true;
            ActivitySwitch();
        }

        private void ActivitySwitch()
        {
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

        /// <summary>
        /// Finalizes the lof and closes the game.
        /// </summary>
        public void QuitButtonClicked()
        {
            Methods.FinalizeLog();
            Application.Quit();
        }
    }
}