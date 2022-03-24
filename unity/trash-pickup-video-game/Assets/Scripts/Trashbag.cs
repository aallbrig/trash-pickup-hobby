using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void TrashbagTrashAdded();
public delegate void TrashbagFilledFull();
public delegate void TrashbagEmptied();

public interface ITrash
{
    float WeightAddInGallons { get; }
}
public class Trashbag : MonoBehaviour
{
    public event TrashbagTrashAdded TrashbagAddEvent;
    public event TrashbagFilledFull TrashbagFullEvent;
    public event TrashbagEmptied TrashbagEmptyEvent;
    public float trashbagCapacityInGallons = 25.0f;

    private float _trashbagCurrentCapacity => _trash.Count == 0 ? 0f : 
        _trash.Select(trash => trash.WeightAddInGallons).Aggregate((sum, next) => sum + next);
    private readonly List<ITrash> _trash = new();
    private void Start()
    {
        ResetTrashbag();
    }
    private void ResetTrashbag()
    {
        _trash.Clear();
        TrashbagEmptyEvent?.Invoke();
    }
    public void Add(ITrash trash)
    {
        TrashbagAddEvent?.Invoke();
        _trash.Add(trash);
        if (_trashbagCurrentCapacity >= trashbagCapacityInGallons)
            TrashbagFullEvent?.Invoke();
    }
    public void Empty()
    {
        ResetTrashbag();
    }
}
