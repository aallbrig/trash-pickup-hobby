using System.Collections.Generic;
using UnityEngine;

namespace Behaviors
{
    public class GameManager: MonoBehaviour
    {
        public MainMenu mainMenu;
        public List<GarbageGenerator> garbageGenerators = new List<GarbageGenerator>();
        private void Start()
        {
            #if UNITY_EDITOR
            Debug.Assert(mainMenu != null, "Main menu is required for component to operate");
            #endif
            
            mainMenu.PlayButtonPressedEvent += OnPlayButtonPressed;
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
