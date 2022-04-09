using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Models;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviors
{
    public delegate void TrashbagTrashAdded(ITrash trash);

    public delegate void TrashbagFilledFull();

    public delegate void TrashbagEmptied(List<ITrash> emptiedTrash);

    [RequireComponent(typeof(RawImage))]
    public class Trashbag : MonoBehaviour
    {
        public Texture2D trashbagEmpty;
        public Texture2D trashbag25PercentFull;
        public Texture2D trashbag50PercentFull;
        public Texture2D trashbag75PercentFull;
        public Texture2D trashbagFull;
        public float trashbagCapacityInGallons = 25.0f;
        private readonly List<ITrash> _currentTrash = new List<ITrash>();
        private RawImage _buttonTexture2d;
        public CinemachineImpulseSource impulseSource;
        public MainMenu mainMenu;

        private float TrashbagCurrentCapacity => _currentTrash.Count == 0
            ? 0f
            : _currentTrash.Select(trash => trash.WeightAddInGallons).Aggregate((sum, next) => sum + next);

        private void SyncTrashbagImage()
        {
            var percentFull = FullPercentInDecimal();
            if (percentFull < 0.25f)
            {
                _buttonTexture2d.texture = trashbagEmpty;
            } else if (percentFull > 0.25f && percentFull < 0.5f)
            {
                _buttonTexture2d.texture = trashbag25PercentFull;
            } else if (percentFull > 0.5f && percentFull < 0.75f)
            {
                _buttonTexture2d.texture = trashbag50PercentFull;
            } else if (percentFull > 0.75f && percentFull < 0.9f)
            {
                _buttonTexture2d.texture = trashbag75PercentFull;
            } else if (percentFull > 0.9f)
            {
                _buttonTexture2d.texture = trashbagFull;
            }
        }

        private void Start()
        {
            _buttonTexture2d = GetComponent<RawImage>();
            impulseSource ??= GetComponent<CinemachineImpulseSource>();
            mainMenu ??= FindObjectOfType<MainMenu>();
            SyncTrashbagImage();

            if (mainMenu) mainMenu.PlayButtonPressedEvent += ResetTrashbag;
            TrashbagFullEvent += () =>
            {
                SyncTrashbagImage();
                TriggerScreenShake();
            };
            TrashAddEvent += _ =>
            {
                SyncTrashbagImage();
                TriggerScreenShake();
            };
            TrashbagEmptyEvent += _ =>
            {
                SyncTrashbagImage();
                TriggerScreenShake();
            };
        }

        public event TrashbagTrashAdded TrashAddEvent;

        public event TrashbagFilledFull TrashbagFullEvent;

        public event TrashbagEmptied TrashbagEmptyEvent;

        private void ResetTrashbag()
        {
            _currentTrash.Clear();
            SyncTrashbagImage();
        }
        public void Add(ITrash trash)
        {
            // Only add trash if the trash bag is not full
            if (FullPercentInDecimal() >= 1.0f) return;

            _currentTrash.Add(trash);
            TrashAddEvent?.Invoke(trash);

            if (TrashbagCurrentCapacity >= trashbagCapacityInGallons)
                TrashbagFullEvent?.Invoke();
        }
        private void TriggerScreenShake()
        {
            if (impulseSource) impulseSource.GenerateImpulse();
        }
        public void Empty()
        {
            if (_currentTrash.Count <= 0) return;

            var trashCopy = _currentTrash.Select(_ => _).ToList();
            ResetTrashbag();
            TrashbagEmptyEvent?.Invoke(trashCopy);
        }
        public float FullPercentInDecimal() => TrashbagCurrentCapacity == 0f ? 0f : TrashbagCurrentCapacity / trashbagCapacityInGallons;
    }
}