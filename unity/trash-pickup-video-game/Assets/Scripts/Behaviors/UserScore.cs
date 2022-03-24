using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
using TMPro;

namespace Behaviors
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UserScore : MonoBehaviour
    {
        public TextMeshProUGUI displayText;
        public Trashbag trashbag;
        private float _currentCurrentScore = 0.0f;
        public float CurrentScore => _currentCurrentScore;
        private void Start()
        {
            displayText ??= GetComponent<TextMeshProUGUI>();
            trashbag ??= FindObjectOfType<Trashbag>();
            trashbag ??= new GameObject().AddComponent<Trashbag>();

            trashbag.TrashAddEvent += OnTrashAdd;
            trashbag.TrashbagEmptyEvent += OnTrashbagEmpty;
            SyncDisplayText();
        }
        private void OnTrashAdd(ITrash trash)
        {
            _currentCurrentScore += 5.0f;
            SyncDisplayText();
        }
        private void SyncDisplayText() => displayText.text = $"Score: {CurrentScore}";
        private void OnTrashbagEmpty(List<ITrash> emptiedTrash)
        {
            _currentCurrentScore +=
                emptiedTrash.Select(trash => trash.Score).Aggregate((score1, score2) => score1 + score2);
            SyncDisplayText();
        }
    }
}