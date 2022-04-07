using Models;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Compliment", menuName = "Game/new Compliment", order = 0)]
    public class Compliment : ScriptableObject, ICompliment
    {
        public AudioClip audioClip;
        public float timeAddInSeconds = 5f;

        public AudioClip Audio => audioClip;
        public float TimeGainInSeconds => timeAddInSeconds;
    }
}