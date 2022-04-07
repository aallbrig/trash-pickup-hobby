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
        private float _currentGameTimeInSeconds = 0f;
        private float _previousSignificantNumber;
        private float _currentSignificantNumber;
        private TextMeshProUGUI _displayText;
        private void ResetCurrentGameTime()
        {
            _currentGameTimeInSeconds = gameTimeInSeconds;
            _currentSignificantNumber = Mathf.Floor(_currentSignificantNumber);
        }
        private void Start()
        {
            _displayText = GetComponent<TextMeshProUGUI>();
            ResetCurrentGameTime();
            SyncDisplayText(_currentSignificantNumber);

            if (mainMenu) mainMenu.PlayButtonPressedEvent += () =>
            {
                ResetCurrentGameTime();
                SyncDisplayText(_currentSignificantNumber);
            };

            CountDownByOneSecondEvent += SyncDisplayText;
        }
        private void SyncDisplayText(float currentCount)
        {
            _displayText.text = currentCount.ToString(CultureInfo.CurrentCulture);
        }
        private void Update()
        {
            if (_currentGameTimeInSeconds == 0) return;

            _previousSignificantNumber = Mathf.Floor(_currentGameTimeInSeconds);
            _currentGameTimeInSeconds = Mathf.Clamp(_currentGameTimeInSeconds - Time.deltaTime, 0, gameTimeInSeconds);
            _currentSignificantNumber = Mathf.Floor(_currentGameTimeInSeconds);
            if (_currentSignificantNumber < _previousSignificantNumber)
                CountDownByOneSecondEvent?.Invoke(_currentSignificantNumber);
            if (_currentGameTimeInSeconds == 0)
                OutOfTimeEvent?.Invoke();
        }
    }
}