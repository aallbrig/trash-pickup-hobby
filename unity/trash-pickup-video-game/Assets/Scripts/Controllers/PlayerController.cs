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
        private void Awake() => _controls = new PlayerControls();
        private void Start()
        {
            mainCamera ??= Camera.current;
            mainCamera ??= Camera.main;

            trashBag ??= FindObjectOfType<Trashbag>();
            trashBag ??= new GameObject().AddComponent<Trashbag>();

            _controls.Gameplay.Interact.started += InteractionStarted;
            _controls.Gameplay.Interact.canceled += InteractionStopped;

            InteractStartEvent += DetectTapOnTrash;
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
        }
        #endif
        public event InteractionStarted InteractStartEvent;

        public event InteractionEnded InteractEndEvent;

        public event PlayerTrashPickup PlayerTrashPickupEvent;

        private void DetectTapOnTrash(InteractionData data)
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
        private void InteractionStarted(InputAction.CallbackContext ctx)
        {
            pointerBeginScreenInput = _controls.Gameplay.Position.ReadValue<Vector2>();
            InteractStartEvent?.Invoke(new InteractionData { ScreenPosition = pointerBeginScreenInput, Timing = Time.time });
        }
        private void InteractionStopped(InputAction.CallbackContext ctx)
        {
            pointerEndScreenInput = _controls.Gameplay.Position.ReadValue<Vector2>();
            InteractEndEvent?.Invoke(new InteractionData { ScreenPosition = pointerEndScreenInput, Timing = Time.time });
        }
    }
}