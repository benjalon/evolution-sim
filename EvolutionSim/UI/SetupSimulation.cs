using EvolutionSim.Data;
using EvolutionSim.Sprites;
using GeonBit.UI;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace EvolutionSim.UI
{
    /// <summary>
    /// The overlay which is displayed within the simulation itself.
    /// </summary>
    public class SetupSimulation
    {
        private Panel mainPanel;

        private Button setupComplete;


        public static Boolean SetupFinished = false;
        public Attributes startingArributes;
        public int InitPopulation;

        public List<Attributes> species;

        private TextInput numSpeciesInput;
        private int numSpecies = 0;


        public SetupSimulation()
        {
            UserInterface.Active.Clear();

            UserInterface.Active.UseRenderTarget = true;
            UserInterface.Active.CursorScale = 0.5f;

                this.mainPanel = new Panel(new Vector2(Graphics.WINDOW_WIDTH / 2, Graphics.WINDOW_HEIGHT / 1.25f));

                Label input = new Label("Number of different Species 1-10: ");
                this.numSpeciesInput = new TextInput();
                numSpeciesInput.Validators.Add(new GeonBit.UI.Entities.TextValidators.TextValidatorNumbersOnly());

                Button continueSetup = new Button("Continue");
                continueSetup.OnClick = (Entity entity) =>
                {
                    numSpecies = Convert.ToInt32(numSpeciesInput.Value);
                    CheckSpeciesNumber();
                };
                mainPanel.AddChild(input);
                mainPanel.AddChild(numSpeciesInput);
                mainPanel.AddChild(continueSetup);

 

            // tab = tabs.AddTab("Tab 2");
            //tab.panel = new SetupSimulationPanel();

            UserInterface.Active.AddEntity(mainPanel);

            //this.CreatePanels();


        }

        private void CheckSpeciesNumber()
        {
            if (numSpecies > 0 && numSpecies < 6)
            {
                UserInterface.Active.Clear();
                species = new List<Attributes>();

                this.mainPanel = new Panel(new Vector2(Graphics.WINDOW_WIDTH / 2, Graphics.WINDOW_HEIGHT / 1.25f));
                setupComplete = new Button("Finish");
 
                UserInterface.Active.AddEntity(setupComplete);
                //this.mainPanel = new Panel();

                PanelTabs tabs = new PanelTabs();
                this.mainPanel.AddChild(tabs);
                TabData tab;

                for (int i = 0; i < numSpecies; i++)
                {
                    tab = tabs.AddTab("Organism " + (i + 1), PanelSkin.Golden);
                    tab.panel.AddChild(new SetupSimulationPanel(tab.button.Size.Y));

                }
                setupComplete.OnClick = (Entity button) =>
                {
                    TabData speciesTab;
                    for (int i = 0; i < numSpecies; i++)
                    {
                        tabs.SelectTab("Organism " + (i + 1));
                        speciesTab = tabs.ActiveTab;
                        Attributes starting;
                        starting = ((SetupSimulationPanel)speciesTab.panel.Children.First()).GetPanelData();
                        species.Add(starting);
                       
                    }

                };
                UserInterface.Active.AddEntity(mainPanel);
  


            }
        }
        /// <summary>
        /// Take input from input devices
        /// </summary>
        /// <param name="gameTime">Time elapsed since last update call</param>
        public void Update(GameTime gameTime)
        {


            UserInterface.Active.Update(gameTime);

        }

        /// <summary>
        /// Draw the UI to the screen
        /// </summary>
        /// <param name="spriteBatch">The spritebatch within which this should be drawn</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            UserInterface.Active.Draw(spriteBatch);
        }



        //private void SetFinishButton()
        //{
        //    this.finishedButton.OnClick = (Entity btn) =>
        //    {
        //        SetupFinished = true;
        //        this.startingArributes = new Attributes();
        //        this.startingArributes.Species = speciesName.Value;
        //        this.startingArributes.Texture = textureImage.Texture;
        //        switch (dietTypeChoice.SelectedValue)
        //        {
        //            case "Omnivore":
        //                startingArributes.DietType = DietTypes.Omnivore;
        //                break;
        //            case "Herbivore":
        //                startingArributes.DietType = DietTypes.Herbivore;
        //                break;
        //            case "Carnivore":
        //                startingArributes.DietType = DietTypes.Canivore;
        //                break;
        //        }
        //        this.startingArributes.MaxHealth = Convert.ToInt32(startHealth.Value);
        //        this.startingArributes.Speed = Convert.ToInt32(startSpeed.Value);
        //        this.startingArributes.Strength = Convert.ToInt32(startStrength.Value);
        //        this.startingArributes.ResistHeat = Convert.ToBoolean(resistHeatChoice.SelectedValue);
        //        this.startingArributes.ResistCold = Convert.ToBoolean(resistColdChoice.SelectedValue);
        //        this.InitPopulation = Convert.ToInt32(startPopulation.Value);
        //        //LoadContent();
        //    };
        //}

    }
}
