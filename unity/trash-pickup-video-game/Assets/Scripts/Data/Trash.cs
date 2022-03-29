using Models;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Trash", menuName = "Game/new Trash", order = 0)]
    public class Trash : ScriptableObject, ITrash
    {
        public float weightInGallons = 1.0f;
        public float score = 1.0f;

        public float WeightAddInGallons => weightInGallons;
        public AudioClip collectSound;

        public float Score => score;

        public AudioClip CollectSound => collectSound;
    }
}