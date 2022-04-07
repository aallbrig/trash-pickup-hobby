using System.Collections.Generic;
using UnityEngine;

namespace Behaviors
{
    public class GameManager: MonoBehaviour
    {
        public MainMenu mainMenu;
        public GameTimer gameTimer;
        public List<GarbageGenerator> garbageGenerators = new List<GarbageGenerator>();
        private void Start()
        {
            #if UNITY_EDITOR
            Debug.Assert(mainMenu != null, "Main menu is required for component to operate");
            Debug.Assert(gameTimer != null, "Game timer is required for this component to operate");
            #endif
            
            mainMenu.PlayButtonPressedEvent += OnPlayButtonPressed;
            gameTimer.OutOfTimeEvent += OnOutOfTime;
        }
        private void OnOutOfTime()
        {
            #if UNITY_EDITOR
            Debug.Log("On game out of time");
            #endif
            // TODO: Show game over screen with screen instead of main menu
            garbageGenerators.ForEach(garbageGenerator => garbageGenerator.StopGeneration());
            mainMenu.ShowMenu();
        }
        private void OnPlayButtonPressed()
        {
            #if UNITY_EDITOR
            Debug.Log("On Play Button Pressed");
            #endif
            garbageGenerators.ForEach(garbageGenerator => garbageGenerator.StartGeneration());
        }
    }
}
