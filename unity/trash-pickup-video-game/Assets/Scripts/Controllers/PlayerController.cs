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
        public int trashLayerMask = 8;
        public Camera mainCamera;
        public Trashbag trashBag;
        [SerializeField] private Vector2 pointerBeginScreenInput;
        [SerializeField] private Vector2 pointerEndScreenInput;
        private PlayerControls _controls;
        private Plane _intersectingPlane = new Plane(Vector3.forward, Vector3.zero);
        [SerializeField] private Vector3 startIntersection;
        [SerializeField] private Vector3 endIntersection;
        private void Awake() => _controls = new PlayerControls();
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
            DetectTrashPickupOnTap(data);
            DetectTrashPickupOnSwipe();
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
            if (startIntersection != Vector3.zero) Gizmos.DrawSphere(startIntersection, 1f);
            if (endIntersection != Vector3.zero) Gizmos.DrawSphere(endIntersection, 1f);
            Gizmos.DrawLine(startIntersection, endIntersection);
        }
        #endif
        public event InteractionStarted InteractionStartedEvent;

        public event InteractionEnded InteractionEndedEvent;

        public event PlayerTrashPickup PlayerTrashPickupEvent;

        private void OnInteractionStart(InteractionData data) => DetectTrashPickupOnTap(data);
        private void DetectTrashPickupOnTap(InteractionData data)
        {
            var screenPointToRay = mainCamera.ScreenPointToRay(data.ScreenPosition);
            if (Physics.Raycast(screenPointToRay, out var hit, 100f))
            {
                var trash = hit.collider.transform.GetComponent<Behaviors.Trash>();
                if (trash)
                {
                    trashBag.Add(trash.trashData);
                    PlayerTrashPickupEvent?.Invoke(trash.trashData);
                    trash.Reset();
                }
            }
        }

        private void DetectTrashPickupOnSwipe()
        {
            // Get position for swipe start
            var startRay = mainCamera.ScreenPointToRay(pointerBeginScreenInput);
            if (_intersectingPlane.Raycast(startRay, out var distance))
                startIntersection = startRay.GetPoint(distance);
            // Get position for swipe end
            var endRay = mainCamera.ScreenPointToRay(pointerEndScreenInput);
            if (_intersectingPlane.Raycast(endRay, out distance))
                endIntersection = endRay.GetPoint(distance);

            var distanceBetweenIntersection = Vector3.Distance(startIntersection, endIntersection);
            var raycastHits = Physics.RaycastAll(startIntersection, (endIntersection - startIntersection).normalized,  distanceBetweenIntersection);
            foreach (var hit in raycastHits)
            {
                var trash = hit.collider.transform.GetComponent<Behaviors.Trash>();
                if (trash)
                {
                    trashBag.Add(trash.trashData);
                    PlayerTrashPickupEvent?.Invoke(trash.trashData);
                    trash.Reset();
                }
            }
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