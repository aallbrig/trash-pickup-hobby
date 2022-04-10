using System;
using UnityEngine;

namespace Behaviors
{
    public class MovingBackground : MonoBehaviour
    {
        public Action BackgroundResetEvent;
        public Vector3 startPosition = Vector3.down * 10;
        public Vector3 endPosition = Vector3.up * 10;
        public float speedInSeconds = 45f;
        public MainMenu mainMenu;
        private float _gameStartTime;
        private float _gameCurrentTime;
        private void Start()
        {
            if (mainMenu)
                mainMenu.PlayButtonPressedEvent += () =>
                {
                    _gameCurrentTime = 0;
                    Reset();
                };
        }
        private void Update()
        {
            _gameCurrentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, _gameCurrentTime / speedInSeconds);
        }
        private void Reset()
        {
            transform.position = startPosition;
            BackgroundResetEvent?.Invoke();
        }
    }
}