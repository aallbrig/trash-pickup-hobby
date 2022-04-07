using System;
using UnityEngine;

namespace Behaviors
{
    public class MainMenu : MonoBehaviour
    {
        public event Action PlayButtonPressedEvent;

        private void Start()
        {
            PlayButtonPressedEvent += HideMenu;
        }
        private void HideMenu()
        {
            gameObject.SetActive(false);
        }
        public void ShowMenu()
        {
            gameObject.SetActive(true);
        }
        public void ReadyToPlay()
        {
            PlayButtonPressedEvent?.Invoke();
        }
    }
}