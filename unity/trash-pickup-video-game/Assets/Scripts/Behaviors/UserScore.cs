using System.Collections.Generic;
using System.Linq;
using Models;
using TMPro;
using UnityEngine;

namespace Behaviors
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UserScore : MonoBehaviour
    {
        public Trashbag trashbag;
        private TextMeshProUGUI _displayText;

        public float CurrentScore { get; private set; }

        private void Start()
        {
            _displayText = GetComponent<TextMeshProUGUI>();
            trashbag ??= FindObjectOfType<Trashbag>();
            trashbag ??= new GameObject().AddComponent<Trashbag>();

            trashbag.TrashAddEvent += OnTrashAdd;
            trashbag.TrashbagEmptyEvent += OnTrashbagEmpty;
            SyncDisplayText();
        }
        private void OnTrashAdd(ITrash trash)
        {
            CurrentScore += 5.0f;
            SyncDisplayText();
        }
        private void SyncDisplayText() => _displayText.text = $"Score: {CurrentScore}";
        private void OnTrashbagEmpty(List<ITrash> emptiedTrash)
        {
            CurrentScore +=
                emptiedTrash.Select(trash => trash.Score).Aggregate((score1, score2) => score1 + score2);
            SyncDisplayText();
        }
    }
}