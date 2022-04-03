using System.Collections.Generic;
using Behaviors;
using Generated;
using UnityEngine;
using UnityEngine.InputSystem;
using Trash = Data.Trash;

namespace Controllers
{
    public struct InteractionData
    {
        public Vector2 ScreenPosition;
        public float Timing;
    }

    public delegate void InteractionStarted(InteractionData data);

    public delegate void InteractionEnded(InteractionData data);

    public delegate void PlayerTrashPickup(Trash trash);

    public class PlayerController : MonoBehaviour
    {
        public Camera mainCamera;
        public Trashbag trashBag;
        public float sampleTiming = 0.01f;
        [SerializeField] private Vector2 pointerBeginScreenInput;
        [SerializeField] private Vector2 pointerEndScreenInput;
        private PlayerControls _controls;
        #if UNITY_EDITOR
        private List<Ray> _rays = new List<Ray>();
        #endif
        private bool _listen = false;
        private float _lastSampleTime;
        private void Awake() => _controls = new PlayerControls();
        private void Update()
        {
            if (_listen == false) return;
            if (Time.time - _lastSampleTime > sampleTiming)
            {
                _lastSampleTime = Time.time;
                SampleFromCurrentPointerPosition();
            }
        }
        private void SampleFromCurrentPointerPosition()
        {
            var currentPosition = _controls.Gameplay.Position.ReadValue<Vector2>();
            var rayFromCurrentPosition = mainCamera.ScreenPointToRay(currentPosition);
            #if UNITY_EDITOR
            _rays.Add(rayFromCurrentPosition);
            #endif
            PickupTrashOnRaycastIntercept(rayFromCurrentPosition);
        }
        private void Start()
        {
            mainCamera ??= Camera.current;
            mainCamera ??= Camera.main;

            trashBag ??= FindObjectOfType<Trashbag>();
            trashBag ??= new GameObject().AddComponent<Trashbag>();

            _controls.Gameplay.Interact.started += InteractionStarted;
            _controls.Gameplay.Interact.canceled += InteractionStopped;

            InteractionStartedEvent += OnInteractionStart;
            InteractionEndedEvent += OnInteractionEnd;
        }
        private void OnInteractionEnd(InteractionData data)
        {
            StopSwipeSampling();
            DetectTrashPickupOnTap(data);
        }
        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Enable();
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var startRay = mainCamera.ScreenPointToRay(pointerBeginScreenInput);
            Gizmos.DrawRay(startRay.origin, startRay.direction * 100f);
            Gizmos.color = Color.blue;
            var endRay = mainCamera.ScreenPointToRay(pointerEndScreenInput);
            Gizmos.DrawRay(endRay.origin, endRay.direction * 100f);

            Gizmos.color = Color.yellow;
            // For multiple raycasts based on sampled swipe technique
            if (_rays.Count > 0) _rays.ForEach(ray => Gizmos.DrawRay(ray.origin, ray.direction * 100f));
        }
        #endif
        public event InteractionStarted InteractionStartedEvent;

        public event InteractionEnded InteractionEndedEvent;

        public event PlayerTrashPickup PlayerTrashPickupEvent;

        private void OnInteractionStart(InteractionData data)
        {
            StartSwipeSampling();
        }
        private void StartSwipeSampling()
        {
            _listen = true;
            _lastSampleTime = Time.time;
            SampleFromCurrentPointerPosition();
        }
        private void StopSwipeSampling()
        {
            _listen = false;
            #if UNITY_EDITOR
            _rays.Clear();
            #endif
        }
        private void DetectTrashPickupOnTap(InteractionData data)
        {
            PickupTrashOnRaycastIntercept(mainCamera.ScreenPointToRay(data.ScreenPosition));
        }
        private void PickupTrashOnRaycastIntercept(Ray ray)
        {
            if (Physics.Raycast(ray, out var hit, 100f))
            {
                var trash = hit.collider.transform.GetComponent<Behaviors.Trash>();
                if (trash) PickupTrash(trash);
            }
        }
        private void PickupTrash(Behaviors.Trash trash)
        {
            trashBag.Add(trash.trashData);
            PlayerTrashPickupEvent?.Invoke(trash.trashData);
            trash.Reset();
        }
        private void InteractionStarted(InputAction.CallbackContext ctx)
        {
            pointerBeginScreenInput = _controls.Gameplay.Position.ReadValue<Vector2>();
            InteractionStartedEvent?.Invoke(new InteractionData { ScreenPosition = pointerBeginScreenInput, Timing = Time.time });
        }
        private void InteractionStopped(InputAction.CallbackContext ctx)
        {
            pointerEndScreenInput = _controls.Gameplay.Position.ReadValue<Vector2>();
            InteractionEndedEvent?.Invoke(new InteractionData { ScreenPosition = pointerEndScreenInput, Timing = Time.time });
        }
    }
}