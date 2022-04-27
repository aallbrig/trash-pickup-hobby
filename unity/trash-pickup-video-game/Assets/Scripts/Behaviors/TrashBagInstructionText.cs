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
        public bool onlyDisplayOnce = true;
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
            trashBag.TrashBagHasResetEvent += () =>
            {
                if (onlyDisplayOnce) _firstTime = true;
                DismissText();
            };
        }
        private void OnTrashBagFull()
        {
            if (onlyDisplayOnce)
            {
                if (_firstTime)
                    ActivateText();
            }
            else
                ActivateText();
        }
        private void ActivateText()
        {
            _text.enabled = true;
            InstructionsActivatedEvent?.Invoke();
        }

        private void OnTrashBagEmpty(List<ITrash> emptiedTrash)
        {
            if (onlyDisplayOnce && _firstTime)
                _firstTime = false;
            DismissText();
        }

        private void DismissText()
        {
            _text.enabled = false;
            InstructionsDismissedEvent?.Invoke();
        }
    }
}