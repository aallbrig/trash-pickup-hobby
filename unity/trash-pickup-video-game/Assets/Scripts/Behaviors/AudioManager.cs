using Models;
using UnityEngine;

namespace Behaviors
{
    public delegate void CollectAudioStarted();
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public CollectAudioStarted CollectAudioStartedEvent;
        public TrashBag trashBag;
        private AudioSource _audioSource;
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            trashBag.TrashAddEvent += OnTrashCollected;
        }
        private void OnTrashCollected(ITrash trash)
        {
            _audioSource.clip = trash.CollectSound;
            if (_audioSource.clip)
            {
                _audioSource.Play(0);
                CollectAudioStartedEvent?.Invoke();
            }
        }
    }
}