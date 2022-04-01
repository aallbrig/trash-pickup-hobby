using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;

namespace Behaviors
{
    public delegate void TrashbagInstructionTextActivated();
    public delegate void TrashbagInstructionTextDismissed();
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TrashbagInstructionText : MonoBehaviour
    {
        public TrashbagInstructionTextDismissed InstructionsDismissedEvent;
        public TrashbagInstructionTextActivated InstructionsActivatedEvent;
        public Trashbag trashbag;
        private TextMeshProUGUI _text;
        private bool _firstTime = true;

        private void Start()
        {
            trashbag ??= GetComponent<Trashbag>();
            trashbag ??= FindObjectOfType<Trashbag>();
            if (trashbag == null)
            {
                #if UNITY_EDITOR
                Debug.LogError("Trashbag is required for this monobehaviour.");
                #endif
                return;
            }

            _text = GetComponent<TextMeshProUGUI>();
            // Setup expected initial text component state
            DismissText();

            trashbag.TrashbagEmptyEvent += OnTrashbagEmpty;
            trashbag.TrashbagFullEvent += OnTrashbagFull;
        }
        private void OnTrashbagFull()
        {
            if (_firstTime)
                ActivateText();
        }
        private void ActivateText()
        {
            _text.enabled = true;
            InstructionsActivatedEvent?.Invoke();
        }

        private void OnTrashbagEmpty(List<ITrash> emptiedtrash)
        {
            if (_firstTime)
            {
                DismissText();
                _firstTime = false;
            }
        }

        private void DismissText()
        {
            _text.enabled = false;
            InstructionsDismissedEvent?.Invoke();
        }
    }
}