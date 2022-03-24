using Behaviors;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Trash", menuName = "Game/new Trash", order = 0)]
    public class Trash : ScriptableObject, ITrash
    {
        public float weightInGallons = 1.0f;

        public float WeightAddInGallons => weightInGallons;
    }
}