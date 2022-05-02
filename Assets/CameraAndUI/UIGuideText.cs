using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnimalEvolution
{
    public class UIGuideText : MonoBehaviour
    {
        public Image guideImage;
        public Text guideText;
        public Button previousButton;
        public Button nextButton;
        public Sprite[] sprites;
        int currentPage = 0;

        public boolSwitchDelegate cameraSwitch;
        public boolSwitchDelegate terrainCreationSwitch;
        public boolSwitchDelegate speedControlsSwitch;
        public boolSwitchDelegate entityCreationSwitch;
        public boolSwitchDelegate entityInfoSwitch;
        private bool cameraBefore;
        private bool terrainCreationBefore;
        private bool speedControlsBefore;
        private bool entityCreationBefore;
        private bool entityInfoBefore;
        private bool pauseBefore;

        public GameObject panel;
        private bool active = false;


        private void Start()
        {
            panel.SetActive(active);
            PreviousButtonClicked();
        }

        /// <summary>
        /// Used to show and hide the guide and enable/disable all other UI.
        /// </summary>
        public void GuideButtonClicked()
        {
            active = !active;
            panel.SetActive(active);
            if (active)
            {
                pauseBefore = Controller.paused;
                Controller.paused = active;
                cameraBefore = cameraSwitch(!active);
                terrainCreationBefore = terrainCreationSwitch(!active);
                speedControlsBefore = speedControlsSwitch(!active);
                entityCreationBefore = entityCreationSwitch(!active);
                entityInfoBefore = entityInfoSwitch(!active);
            }
            else
            {
                Controller.paused = pauseBefore;
                cameraSwitch(cameraBefore);
                terrainCreationSwitch(terrainCreationBefore);
                speedControlsSwitch(speedControlsBefore);
                entityCreationSwitch(entityCreationBefore);
                entityInfoSwitch(entityInfoBefore);
            }
        }

        /// <summary>
        /// Decreases the page number and updates the page UI if it's within the page count.
        /// </summary>
        public void PreviousButtonClicked()
        {
            currentPage = Mathf.Max(0, currentPage - 1);
            SwitchPage();
            if (currentPage == 0)
            {
                previousButton.interactable=false;
            }
            nextButton.interactable = true;
        }

        /// <summary>
        /// Increases the page number and updates the page UI if it's within the page count.
        /// </summary>
        public void NextButtonClicked()
        {
            currentPage = Mathf.Min(4, currentPage + 1);
            SwitchPage();
            if (currentPage == 4)
            {
                nextButton.interactable = false;
            }
            previousButton.interactable = true;
        }


        /// <summary>
        /// The contents of the Guide.
        /// </summary>
        void SwitchPage()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            switch (currentPage)
            {
                case 0:
                    guideImage.sprite = sprites[currentPage];
                    result.Append("<size=50>Map Creation</size>\n");
                    result.Append("<size=25>• The <b>MAP WIDTH</b> and <b>MAP LENGTH</b> sliders define the dimensions of the map, how far right and forward the world will span</size>\n");
                    result.Append("<size=25>• The <b>MAP HEIGHT</b> slider defines the height of the highest mountainous terrain</size>\n");
                    result.Append("<size=25>• The <b>WATER LEVEL</b> slider defines to what percentage of height the map will be flooded with water</size>\n");
                    result.Append("<size=25>• The <b>MAP SEED</b> field allows to player to type in a number that will represent the seed used for map generation (it will also display the current used seed if the player wishes to remember it for later use)</size>\n");
                    result.Append("<size=25>• The <b>COPY</b> Button copies the randomly generated seed into the seed input field (use in case you like the general shape of the map and want to change the other options)</size>\n");
                    result.Append("<size=25>• The <b>Generate Terrain</b> button generates a new terrain based on the input as described above. If the seed field is unchanged by the player, it will generate a new seed with every click of the button</size>\n");
                    result.Append("<size=25>• The <b>Continue</b> button locks in the current terrain for this game session</size>\n");
                    break;
                case 1:
                    guideImage.sprite = sprites[currentPage];
                    result.Append("<size=50>Key Controls</size>\n");
                    result.Append("<size=25>• <b>W A S D</b> are used to move the view forward, backward, left and right respectively</size>\n");
                    result.Append("<size=25>• <b>Q E</b> are used to rotate clockwise and anticlockwise respectively</size>\n");
                    result.Append("<size=25>• <b>R F</b> are used to zoom in and zoom out respectively</size>\n");
                    result.Append("<size=25>• <b>H</b> is used to return the camera to its original position</size>\n");
                    result.Append("<size=25>• <b>X</b> is used to stop all plants and animals from reproducing</size>\n");
                    result.Append("<size=25>• <b>Ecs</b> pauses the game and is used to open the <b>Exit Game menu</b></size>\n");
                    result.Append("<size=25>• <b>Left click</b> on any entity to open its <b>Entity Info Panel</b></size>\n\n\n\n");
                    result.Append("<size=50>On Screen Controls</size>\n");
                    result.Append("<size=25>• The <b>?</b> button in left bottom corner opens the guide</size>\n");
                    result.Append("<size=25>• The <b>slider</b> controls the speed at which time passes in game</size>\n");
                    result.Append("<size=25>• The <b>| | ></b> button pauses and unpauses the game</size>\n");
                    result.Append("<size=25>• The <b>Entity</b> button opens the <b>Entity creation menu</b></size>\n\n\n\n");
                    result.Append("<size=50>Exit Game menu</size>\n");
                    result.Append("<size=25>• The <b>YES</b> button exits the game.</size>\n");
                    result.Append("<size=25>• The <b>NO</b> button resumes the game.</size>\n");
                    break;
                case 2:
                    guideImage.sprite = sprites[currentPage];
                    result.Append("<size=50>Entity creation menu</size>\n");
                    result.Append("<size=25>• The <b>X</b> button closes this menu without creating an ancestor</size>\n");
                    result.Append("<size=25>• The <b>Plant</b> and <b>Animal</b> buttons switch between creating a plant and creating an animal.</size>\n");
                    result.Append("<size=25>• The <b>Species</b> field records the name to be used for a created entity and its children</size>\n");
                    result.Append("<size=25>• The <b>NUTRITIONAL VALUE</b> slider specifies how much food points will an animal gain by consuming this entity</size>\n");
                    result.Append("<size=25>• The <b>TIME BETWEEN OFFSPRINGS</b> slider specifies the minimum amount of time that needs to pass between the entity can reproduce</size>\n");
                    result.Append("<size=25>• The <b>TIME TO LIVE</b> slider represents the maximum time an entity can live, given favourable circumstances</size>\n");
                    result.Append("<size=25>• The <b>SIZE</b> slider controls the size of the entity (careful! carnivores can only eat animals smaller than themselves</size>\n");
                    result.Append("<size=25>• The <b>MUTATION STRENGTH</b> slider specifies how strongly the entity’s offspring will be affected by mutation.</size>\n");
                    result.Append("<size=25>• The <b>COLOUR</b> sliders define the entity’s colour.</size>\n");
                    result.Append("<size=25>• The <b>Create Ancestor</b> button closes the entity creation menu and clicking the terrain after pressing this button places the created species’s first member</size>\n");
                    break;
                case 3:
                    guideImage.sprite = sprites[currentPage];
                    result.Append("<size=50>Entity creation menu - Animal only options</size>\n");
                    result.Append("<size=25>• The <b>SENSES</b> slider specifies how far an animal is able to sense its potential food</size>\n");
                    result.Append("<size=25>• The <b>SPEED</b> slider specifies the speed of the animal</size>\n");
                    result.Append("<size=25>• The <b>MAX FOOD</b> slider defines the stomach capacity of the animal</size>\n");
                    result.Append("<size=25>• The <b>FOOD TO BREED</b> slider specifies how full the animal needs to be to be able to create an offspring (the animal also consumes half of this amount as it creates an offspring</size>\n");
                    result.Append("<size=25>• The <b>HERBIVORE</b> and <b>CARNIVORE</b> buttons determine the preferred diet of the animal</size>\n");
                    break;
                case 4:
                    guideImage.sprite = sprites[currentPage];
                    result.Append("<size=50>Entity Info Panel</size>\n");
                    result.Append("<size=25>• This panel displays the Entity’s important properties such as its name, remaining life, nutritional value, time between offsprings, size, mutation strength and for animals also type (herbivore or carnivore) and food status</size>\n");
                    result.Append("<size=25>• The <b>X</b> button closes this panel</size>\n");
                    result.Append("<size=25>• The <b>Rejuvenate</b> button adds 10% of maximum lifetime to the entity</size>\n");
                    result.Append("<size=25>• The <b>Feed </b>button adds 10% of maximum food to the entity (it is only available for animals)</size>\n");
                    result.Append("<size=25>• The <b>Starve</b> button removes 10% of maximum food from the entity (also only available for animals)</size>\n");
                    result.Append("<size=25>• The <b>Kill</b> button instantly kills the entity</size>\n");
                    result.Append("<size=25>• The <b>Breed</b> button instantly creates an offspring of the entity (this doesn’t consume food in case of animals)</size>\n");
                    break;
                default:
                    result.Append("Wrong page number.\n");
                    break;
            }
            guideText.text = result.ToString();
        }
    }
}