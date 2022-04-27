using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Models;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviors
{
    public delegate void TrashBagTrashAdded(ITrash trash);

    public delegate void TrashBagFilledFull();

    public delegate void TrashBagEmptied(List<ITrash> emptiedTrash);
    public delegate void TrashBagHasReset();

    [RequireComponent(typeof(RawImage))]
    public class TrashBag : MonoBehaviour
    {
        public Texture2D trashBagEmpty;
        public Texture2D trashBag25PercentFull;
        public Texture2D trashBag50PercentFull;
        public Texture2D trashBag75PercentFull;
        public Texture2D trashBagFull;
        public float trashBagCapacityInGallons = 25.0f;
        private readonly List<ITrash> _currentTrash = new List<ITrash>();
        private RawImage _buttonTexture2d;
        public CinemachineImpulseSource impulseSource;
        public MainMenu mainMenu;

        private float TrashBagCurrentCapacity => _currentTrash.Count == 0
            ? 0f
            : _currentTrash.Select(trash => trash.WeightAddInGallons).Aggregate((sum, next) => sum + next);

        private void SyncTrashBagImage()
        {
            var percentFull = FullPercentInDecimal();
            if (percentFull < 0.25f)
            {
                _buttonTexture2d.texture = trashBagEmpty;
            } else if (percentFull > 0.25f && percentFull < 0.5f)
            {
                _buttonTexture2d.texture = trashBag25PercentFull;
            } else if (percentFull > 0.5f && percentFull < 0.75f)
            {
                _buttonTexture2d.texture = trashBag50PercentFull;
            } else if (percentFull > 0.75f && percentFull < 0.9f)
            {
                _buttonTexture2d.texture = trashBag75PercentFull;
            } else if (percentFull > 0.9f)
            {
                _buttonTexture2d.texture = trashBagFull;
            }
        }

        private void Start()
        {
            _buttonTexture2d = GetComponent<RawImage>();
            impulseSource ??= GetComponent<CinemachineImpulseSource>();
            mainMenu ??= FindObjectOfType<MainMenu>();

            if (mainMenu) mainMenu.PlayButtonPressedEvent += ResetTrashBag;
            TrashBagFullEvent += () =>
            {
                SyncTrashBagImage();
                TriggerScreenShake();
            };
            TrashAddEvent += _ =>
            {
                SyncTrashBagImage();
                TriggerScreenShake();
            };
            TrashBagEmptyEvent += _ =>
            {
                SyncTrashBagImage();
                TriggerScreenShake();
            };
        }

        public event TrashBagTrashAdded TrashAddEvent;

        public event TrashBagFilledFull TrashBagFullEvent;

        public event TrashBagEmptied TrashBagEmptyEvent;
        public event TrashBagHasReset TrashBagHasResetEvent;

        private void ResetTrashBag()
        {
            _currentTrash.Clear();
            SyncTrashBagImage();
            TrashBagHasResetEvent?.Invoke();
        }
        public void Add(ITrash trash)
        {
            // Only add trash if the trash bag is not full
            if (CanAddTrash())
            {
                _currentTrash.Add(trash);
                TrashAddEvent?.Invoke(trash);

                if (TrashBagCurrentCapacity >= trashBagCapacityInGallons)
                    TrashBagFullEvent?.Invoke();
            }
        }
        // Ignore if the trash will set the capacity over 100%
        public bool CanAddTrash() => FullPercentInDecimal() < 1.5f;
        private void TriggerScreenShake()
        {
            if (impulseSource) impulseSource.GenerateImpulse();
        }
        public void Empty()
        {
            if (_currentTrash.Count <= 0) return;

            var trashCopy = _currentTrash.Select(_ => _).ToList();
            ResetTrashBag();
            TrashBagEmptyEvent?.Invoke(trashCopy);
        }
        public float FullPercentInDecimal() => TrashBagCurrentCapacity == 0f ? 0f : TrashBagCurrentCapacity / trashBagCapacityInGallons;
    }
}