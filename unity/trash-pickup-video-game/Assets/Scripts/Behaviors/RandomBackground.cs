using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviors
{
    [RequireComponent(typeof(Renderer))]
    public class RandomBackground : MonoBehaviour
    {
        public Action BackgroundSelectedEvent;
        public List<Material> possibleBackgrounds = new List<Material>();
        public MainMenu mainMenu;
        private Renderer _renderer;
        private Material _lastMaterial;
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            if (mainMenu) mainMenu.PlayButtonPressedEvent += ChooseBackground;
            else ChooseBackground();
        }
        private void ChooseBackground()
        {
            Material backgroundMaterial;
            if (_lastMaterial == null)
                backgroundMaterial = possibleBackgrounds[Random.Range(0, possibleBackgrounds.Count)];
            else
                do backgroundMaterial = possibleBackgrounds[Random.Range(0, possibleBackgrounds.Count)];
                while (backgroundMaterial == _lastMaterial);
            _renderer.material = backgroundMaterial;
            _lastMaterial = backgroundMaterial;
            BackgroundSelectedEvent?.Invoke();
        }
    }
}