using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Behaviors
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class GameTimer : MonoBehaviour
    {
        public Action<float> CountDownByOneSecondEvent;
        public Action OutOfTimeEvent;
        public float gameTimeInSeconds = 90;
        public MainMenu mainMenu;
        public bool countDown;
        private float _currentGameTimeInSeconds = 0f;
        private float _previousSignificantNumber;
        private float _currentSignificantNumber;
        private TextMeshProUGUI _displayText;
        private bool _countDown;
        private void ResetCountdown()
        {
            _currentGameTimeInSeconds = gameTimeInSeconds;
            _currentSignificantNumber = Mathf.Floor(_currentSignificantNumber);
            SyncDisplayText(_currentSignificantNumber);
        }
        private void Start()
        {
            _displayText = GetComponent<TextMeshProUGUI>();
            _countDown = countDown;
            ResetCountdown();

            if (mainMenu)
                mainMenu.PlayButtonPressedEvent += () =>
                {
                    ResetCountdown();
                    StartCounting();
                };

            CountDownByOneSecondEvent += SyncDisplayText;
        }
        private void SyncDisplayText(float currentCount)
        {
            _displayText.text = currentCount.ToString(CultureInfo.CurrentCulture);
        }
        private void Update()
        {
            if (_countDown == false || _currentGameTimeInSeconds == 0) return;

            _previousSignificantNumber = Mathf.Floor(_currentGameTimeInSeconds);
            _currentGameTimeInSeconds = Mathf.Clamp(_currentGameTimeInSeconds - Time.deltaTime, 0, gameTimeInSeconds);
            _currentSignificantNumber = Mathf.Floor(_currentGameTimeInSeconds);
            if (_currentSignificantNumber < _previousSignificantNumber)
                CountDownByOneSecondEvent?.Invoke(_currentSignificantNumber);

            if (_currentGameTimeInSeconds == 0)
            {
                StopCounting();
                OutOfTimeEvent?.Invoke();
            }
        }
        private void StopCounting()
        {
            _countDown = false;
        }
        private void StartCounting()
        {
            _countDown = true;
        }
    }
}