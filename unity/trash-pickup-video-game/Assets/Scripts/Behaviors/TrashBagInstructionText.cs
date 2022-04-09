using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;

namespace Behaviors
{
    public delegate void TrashBagInstructionTextActivated();
    public delegate void TrashBagInstructionTextDismissed();
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TrashBagInstructionText : MonoBehaviour
    {
        public TrashBagInstructionTextDismissed InstructionsDismissedEvent;
        public TrashBagInstructionTextActivated InstructionsActivatedEvent;
        public TrashBag trashBag;
        private TextMeshProUGUI _text;
        private bool _firstTime = true;

        private void Start()
        {
            trashBag ??= GetComponent<TrashBag>();
            trashBag ??= FindObjectOfType<TrashBag>();
            if (trashBag == null)
            {
                #if UNITY_EDITOR
                Debug.LogError("TrashBag is required for this monobehaviour.");
                #endif
                return;
            }

            _text = GetComponent<TextMeshProUGUI>();
            // Setup expected initial text component state
            DismissText();

            trashBag.TrashBagEmptyEvent += OnTrashBagEmpty;
            trashBag.TrashBagFullEvent += OnTrashBagFull;
        }
        private void OnTrashBagFull()
        {
            if (_firstTime)
                ActivateText();
        }
        private void ActivateText()
        {
            _text.enabled = true;
            InstructionsActivatedEvent?.Invoke();
        }

        private void OnTrashBagEmpty(List<ITrash> emptiedtrash)
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