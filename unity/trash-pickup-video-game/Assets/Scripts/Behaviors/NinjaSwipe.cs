using System;
using Controllers;
using UnityEngine;

namespace Behaviors
{
    public class NinjaSwipe : MonoBehaviour
    {
        public Action NinjaSwipeListeningEvent;
        public Action NinjaSwipeStopListeningEvent;
        public Action NinjaSwipeSyncedEvent;
        // to enable/disable to prevent janky line effects
        public Camera mainCamera;
        public Transform lineEffects;
        public PlayerController playerController;
        public Vector3 offset = new Vector3(0, 0, 15);
        private bool _listening;
        private Plane _intersectingPlane;
        private void Start()
        {
            mainCamera ??= Camera.main;
            mainCamera ??= new GameObject().AddComponent<Camera>();
            _intersectingPlane = new Plane(Vector3.forward, mainCamera.transform.position + offset);
            lineEffects ??= new GameObject { transform = { parent = transform } }.AddComponent<RectTransform>();
            if (playerController == null)
            {
                #if UNITY_EDITOR
                Debug.LogError("Player controller is required for Ninja Swipe to work properly");
                #endif
                return;
            }

            playerController.InteractionStartedEvent += _ =>
            {
                _listening = true;
                NinjaSwipeListeningEvent?.Invoke();
            };
            playerController.CurrentPositionSampledEvent += data =>
            {
                var ray = mainCamera.ScreenPointToRay(data.ScreenPosition);
                if (_intersectingPlane.Raycast(ray, out var distance))
                    lineEffects.position = ray.GetPoint(distance);
                NinjaSwipeSyncedEvent?.Invoke();
            };
            playerController.InteractionEndedEvent += _ =>
            {
                _listening = false;
                NinjaSwipeStopListeningEvent?.Invoke();
            };
        }
    }
}