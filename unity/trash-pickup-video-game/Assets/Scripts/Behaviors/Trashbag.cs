using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

namespace Behaviors {
    public delegate void TrashbagTrashAdded(ITrash trash);
    public delegate void TrashbagFilledFull();
    public delegate void TrashbagEmptied(List<ITrash> emptiedTrash);

    public class Trashbag : MonoBehaviour
    {
        public event TrashbagTrashAdded TrashAddEvent;
        public event TrashbagFilledFull TrashbagFullEvent;
        public event TrashbagEmptied TrashbagEmptyEvent;
        public float trashbagCapacityInGallons = 25.0f;

        private float _trashbagCurrentCapacity => _currentTrash.Count == 0 ? 0f : 
            _currentTrash.Select(trash => trash.WeightAddInGallons).Aggregate((sum, next) => sum + next);
        private readonly List<ITrash> _currentTrash = new();
        private void ResetTrashbag()
        {
            var trashCopy = _currentTrash.Select(_ => _).ToList();
            TrashbagEmptyEvent?.Invoke(trashCopy);
            _currentTrash.Clear();
        }
        public void Add(ITrash trash)
        {
            _currentTrash.Add(trash);
            TrashAddEvent?.Invoke(trash);
            if (_trashbagCurrentCapacity >= trashbagCapacityInGallons)
                TrashbagFullEvent?.Invoke();
        }
        public void Empty()
        {
            ResetTrashbag();
        }
    }
}