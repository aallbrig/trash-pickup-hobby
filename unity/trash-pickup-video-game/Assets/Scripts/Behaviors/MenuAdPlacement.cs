using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Behaviors
{
    public class MenuAdPlacement : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void ConfigureAdvertisementSystem();

        [DllImport("__Internal")]
        private static extern void PlayStartGameAdvertisement();

        private void Start()
        {
            ConfigureAdvertisement();
            LoadAdvertisement();
        }
        public Action AdvertisementSystemConfiguredEvent;
        public Action AdvertisementRequestedEvent;
        private void ConfigureAdvertisement()
        {
            #if UNITY_EDITOR
            if (typeof(MenuAdPlacement).GetMethod("ConfigureAdvertisementSystem") != null)
            #endif
                ConfigureAdvertisementSystem();
            AdvertisementSystemConfiguredEvent?.Invoke();
        }
        private void LoadAdvertisement()
        {
            #if UNITY_EDITOR
            if (typeof(MenuAdPlacement).GetMethod("PlayStartGameAdvertisement") != null)
            #endif
                PlayStartGameAdvertisement();
            AdvertisementRequestedEvent?.Invoke();
        }
    }
}