using UnityEngine;

namespace Models
{
    public interface ICompliment
    {
        public AudioClip Audio { get; }
        public float TimeGainInSeconds { get; }
    }
}