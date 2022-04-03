using System;
using UnityEngine;

namespace Behaviors
{
    public class MainMenu : MonoBehaviour
    {
        public event Action PlayButtonPressedEvent;

        private void Start()
        {
            PlayButtonPressedEvent += () => gameObject.SetActive(false);
        }
        public void ReadyToPlay()
        {
            PlayButtonPressedEvent?.Invoke();
        }
    }
}